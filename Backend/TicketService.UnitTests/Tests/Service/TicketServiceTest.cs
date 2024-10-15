using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TicketService.Core.ServicesContracts;
using TicketService.Domain.Models;
using TicketService.Infrastructure.Repositories;

namespace TicketService.UnitTests.Tests.Service
{
    public class TicketServiceTest
    {
        private readonly ITicketService _ticketService;
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly Mock<ILogger<TicketService.Core.Services.TicketService>> _loggerMock;
        private readonly IFixture _fixture;

        public TicketServiceTest()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _loggerMock = new Mock<ILogger<TicketService.Core.Services.TicketService>>();
            _ticketService = new TicketService.Core.Services.TicketService(
                _ticketRepositoryMock.Object,
                _loggerMock.Object
            );
            _fixture = new Fixture();
        }

        [Fact]
        public async Task TestAddAsync()
        {
            var ticket = _fixture.Create<Ticket>();
            _ticketRepositoryMock.Setup(repo => repo.AddAsync(ticket)).ReturnsAsync(ticket);

            var result = await _ticketService.AddAsync(ticket);

            result.Should().BeEquivalentTo(ticket);
            _ticketRepositoryMock.Verify(repo => repo.AddAsync(ticket), Times.Once);
        }

        [Fact]
        public async Task TestCountAsync()
        {
            var filterQuery = "test";
            _ticketRepositoryMock.Setup(repo => repo.CountAsync(filterQuery)).ReturnsAsync(5);

            var count = await _ticketService.CountAsync(filterQuery);

            count.Should().Be(5);
            _ticketRepositoryMock.Verify(repo => repo.CountAsync(filterQuery), Times.Once);
        }

        [Fact]
        public async Task TestDeleteAsync_Success()
        {
            var ticketId = 1;
            _ticketRepositoryMock.Setup(repo => repo.DeleteAsync(ticketId)).ReturnsAsync(1);

            var result = await _ticketService.DeleteAsync(ticketId);

            result.Should().Be(1);
            _ticketRepositoryMock.Verify(repo => repo.DeleteAsync(ticketId), Times.Once);
        }

        [Fact]
        public async Task TestDeleteAsync_NotFound()
        {
            var ticketId = 1;
            _ticketRepositoryMock.Setup(repo => repo.DeleteAsync(ticketId)).ReturnsAsync(-1);

            var result = await _ticketService.DeleteAsync(ticketId);

            result.Should().Be(-1);
            _ticketRepositoryMock.Verify(repo => repo.DeleteAsync(ticketId), Times.Once);
        }

        [Fact]
        public async Task TestGetAllAsync()
        {
            var tickets = _fixture.CreateMany<Ticket>(2).ToList();
            _ticketRepositoryMock
                .Setup(repo => repo.GetAllAsync(0, 10, "Description", "ASC", ""))
                .ReturnsAsync(tickets);

            var result = await _ticketService.GetAllAsync(0, 10, "Description", "ASC", "");

            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(tickets);
            _ticketRepositoryMock.Verify(
                repo => repo.GetAllAsync(0, 10, "Description", "ASC", ""),
                Times.Once
            );
        }

        [Fact]
        public async Task TestGetAsync_Success()
        {
            var ticket = _fixture.Create<Ticket>();
            _ticketRepositoryMock.Setup(repo => repo.GetAsync(ticket.Id)).ReturnsAsync(ticket);

            var result = await _ticketService.GetAsync(ticket.Id);

            result.Should().BeEquivalentTo(ticket);
            _ticketRepositoryMock.Verify(repo => repo.GetAsync(ticket.Id), Times.Once);
        }

        [Fact]
        public async Task TestUpdateAsync_Success()
        {
            var ticket = _fixture.Create<Ticket>();
            _ticketRepositoryMock.Setup(repo => repo.UpdateAsync(ticket)).ReturnsAsync(1);

            var result = await _ticketService.UpdateAsync(ticket);

            result.Should().Be(1);
            _ticketRepositoryMock.Verify(repo => repo.UpdateAsync(ticket), Times.Once);
        }
    }
}
