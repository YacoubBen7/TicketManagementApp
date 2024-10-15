using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TicketService.Domain.Exception;
using TicketService.Domain.Models;
using TicketService.Infrastructure.Data;
using TicketService.Infrastructure.Repositories;

namespace TicketService.UnitTests.Tests.Repository
{
    public class TicketRepositoryTest
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketDbContext _dbContext;
        private readonly Mock<ILogger<TicketRepository>> _loggerMock;
        private readonly IFixture _fixture;

        public TicketRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<TicketDbContext>()
                .UseInMemoryDatabase(databaseName: "TestUserDb")
                .Options;

            _dbContext = new TicketDbContext(options); 
            _loggerMock = new Mock<ILogger<TicketRepository>>();
            _ticketRepository = new TicketRepository(_dbContext, _loggerMock.Object);
            _fixture = new Fixture();
        }



        private async Task ResetDatabase()
        {
            await ((TicketDbContext)_dbContext).Database.EnsureDeletedAsync();
            await ((TicketDbContext)_dbContext).Database.EnsureCreatedAsync();
        }

        [Fact]
        public async Task TestAddAsync()
        {
            await ResetDatabase();
            var ticket = _fixture
                .Build<Ticket>()
                .Without(u => u.Id)
                .With(u => u.CreatedAt, DateTime.Now)
                .With(u => u.UpdatedAt, DateTime.Now)
                .Create();

            await _ticketRepository.AddAsync(ticket);

            var ticketInDb = await _dbContext.Tickets.FirstOrDefaultAsync(t =>
                t.Description == ticket.Description
            );
            ticketInDb.Should().NotBeNull();
            ticketInDb.Should().BeEquivalentTo(ticket, options => options.Excluding(u => u.Id));

            _dbContext.Tickets.Count().Should().Be(1);

            _loggerMock.Verify(
                x =>
                    x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        null,
                        (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
                    ),
                Times.AtLeastOnce
            );
        }

        [Fact]
        public async Task TestCountAsync()
        {
            await ResetDatabase();

            var ticket1 = _fixture.Create<Ticket>();
            var ticket2 = _fixture.Create<Ticket>();
            await _ticketRepository.AddAsync(ticket1);
            await _ticketRepository.AddAsync(ticket2);

            var count = await _ticketRepository.CountAsync(null!);
            count.Should().Be(2);
        }

        [Fact]
        public async Task TestDeleteAsync_Success()
        {
            await ResetDatabase();

            var ticket = _fixture.Create<Ticket>();
            await _ticketRepository.AddAsync(ticket);

            var result = await _ticketRepository.DeleteAsync(ticket.Id);
            result.Should().Be(1);
            var deletedTicket = await _dbContext.Tickets.FindAsync(ticket.Id);
            deletedTicket.Should().BeNull();
        }

        [Fact]
        public async Task TestDeleteAsync_NotFound()
        {
            await ResetDatabase();

            var result = await _ticketRepository.DeleteAsync(1);
            result.Should().Be(-1);
        }

        [Fact]
        public async Task TestGetAllAsync()
        {
            await ResetDatabase();

            var ticket1 = _fixture.Create<Ticket>();
            var ticket2 = _fixture.Create<Ticket>();
            await _ticketRepository.AddAsync(ticket1);
            await _ticketRepository.AddAsync(ticket2);

            var tickets = await _ticketRepository.GetAllAsync(0, 10, "Description", "asc", "");
            tickets.Should().HaveCount(2);
            tickets.Should().Contain(t => t.Description == ticket1.Description);
            tickets.Should().Contain(t => t.Description == ticket2.Description);
        }

        [Fact]
        public async Task TestGetAsync_Success()
        {
            await ResetDatabase();

            var ticket = _fixture.Create<Ticket>();
            await _ticketRepository.AddAsync(ticket);

            var fetchedTicket = await _ticketRepository.GetAsync(ticket.Id);
            fetchedTicket.Should().BeEquivalentTo(ticket);
        }

        [Fact]
        public async Task TestGetAsync_NotFound()
        {
            await ResetDatabase();

            Func<Task> act = async () => await _ticketRepository.GetAsync(1);
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task TestUpdateAsync_Success()
        {
            await ResetDatabase();

            var ticket = _fixture.Create<Ticket>();
            await _ticketRepository.AddAsync(ticket);
            ticket.Description = "Updated Description";

            var result = await _ticketRepository.UpdateAsync(ticket);
            result.Should().Be(1);
            var updatedTicket = await _ticketRepository.GetAsync(ticket.Id);
            updatedTicket.Description.Should().Be("Updated Description");
        }

        [Fact]
        public async Task TestUpdateAsync_NotFound()
        {
            await ResetDatabase();

            var ticket = _fixture.Create<Ticket>();
            Func<Task> act = async () => await _ticketRepository.UpdateAsync(ticket);
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
