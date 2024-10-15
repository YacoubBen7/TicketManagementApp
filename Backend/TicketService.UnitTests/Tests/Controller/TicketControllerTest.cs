using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using TicketService.API.Controllers;
using TicketService.API.DTO;
using TicketService.API.DTO.Templates;
using TicketService.Core.ServicesContracts;
using TicketService.Domain.Enums;
using TicketService.Domain.Models;
using Xunit;

public class TicketControllerTests
{
    private readonly Mock<ITicketService> _ticketServiceMock;
    private readonly TicketController _ticketController;
    private readonly Fixture _fixture;
    private readonly Mock<IUrlHelperFactory> _urlHelperFactoryMock;
    private readonly Mock<IUrlHelper> _urlHelperMock;

    public TicketControllerTests()
    {
        _ticketServiceMock = new Mock<ITicketService>();
        var loggerMock = new Mock<ILogger<TicketController>>();
        _urlHelperFactoryMock = new Mock<IUrlHelperFactory>();
        _urlHelperMock = new Mock<IUrlHelper>();

        _urlHelperFactoryMock
            .Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>()))
            .Returns(_urlHelperMock.Object);

        _ticketController = new TicketController(_ticketServiceMock.Object, loggerMock.Object)
        {
            Url = _urlHelperMock.Object,
        };
        _fixture = new Fixture();
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedAtAction_WithTicketResponse()
    {
        var ticketCreateDTO = new TicketCreateDTO("Test Description");
        var createdTicket = _fixture
            .Build<Ticket>()
            .With(t => t.Id, 1)
            .With(t => t.Description, "Test Description")
            .With(t => t.Status, Status.Open)
            .Create();

        _ticketServiceMock
            .Setup(service => service.AddAsync(It.IsAny<Ticket>()))
            .ReturnsAsync(createdTicket);

        var result = await _ticketController.CreateAsync(ticketCreateDTO);

        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);

        var restResult = createdResult.Value as RestDTO<TicketResponseDTO>;
        restResult.Should().NotBeNull();
        restResult!
            .Data.Should()
            .BeEquivalentTo(
                new TicketResponseDTO(
                    createdTicket.Id,
                    createdTicket.Description,
                    createdTicket.Status.ToString(),
                    createdTicket.CreatedAt,
                    createdTicket.UpdatedAt
                )
            );
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithTicketResponseList()
    {
        var pageIndex = 0;
        var pageSize = 10;
        var sortColumn = "Id";
        var sortOrder = "ASC";
        var filterQuery = "";

        var tickets = _fixture.CreateMany<Ticket>(5).ToList();
        _ticketServiceMock
            .Setup(service =>
                service.GetAllAsync(pageIndex, pageSize, sortColumn, sortOrder, filterQuery)
            )
            .ReturnsAsync(tickets);

        _ticketServiceMock
            .Setup(service => service.CountAsync(filterQuery))
            .ReturnsAsync(tickets.Count);

        var result = await _ticketController.GetAll(
            pageIndex,
            pageSize,
            sortColumn,
            sortOrder,
            filterQuery
        );

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var restResult = okResult.Value as RestDTO<PageDTO<TicketResponseDTO>>;
        restResult.Should().NotBeNull();
        restResult!.Data.Should().NotBeNull();
        restResult.Data.TotalCount.Should().Be(tickets.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WithTicketResponse()
    {
        // Arrange
        var ticketId = 1;
        var ticket = _fixture
            .Build<Ticket>()
            .With(t => t.Id, ticketId)
            .With(t => t.Description, "Test Description")
            .With(t => t.Status, Status.Open)
            .Create();

        _ticketServiceMock.Setup(service => service.GetAsync(ticketId)).ReturnsAsync(ticket);

        var result = await _ticketController.GetById(ticketId);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var restResult = okResult.Value as RestDTO<TicketResponseDTO>;
        restResult.Should().NotBeNull();
        restResult!
            .Data.Should()
            .BeEquivalentTo(
                new TicketResponseDTO(
                    ticket.Id,
                    ticket.Description,
                    ticket.Status.ToString(),
                    ticket.CreatedAt,
                    ticket.UpdatedAt
                )
            );
    }

    [Fact]
    public async Task Patch_ReturnsOkResult_WithUpdatedTicketId()
    {
        var ticketId = 1;
        var ticketUpdateDTO = new TicketUpdateDTO("Updated Description", Status.Closed.ToString());
        var updatedTicket = _fixture
            .Build<Ticket>()
            .With(t => t.Id, ticketId)
            .With(t => t.Description, ticketUpdateDTO.Description)
            .With(t => t.Status, Status.Closed)
            .Create();

        _ticketServiceMock
            .Setup(service => service.UpdateAsync(It.IsAny<Ticket>()))
            .ReturnsAsync(ticketId);

        var result = await _ticketController.Patch(ticketId, ticketUpdateDTO);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var restResult = okResult.Value as RestDTO<int>;
        restResult.Should().NotBeNull();
        restResult!.Data.Should().Be(ticketId);
    }

    [Fact]
    public async Task DeleteById_ReturnsOkResult_WithDeletedTicketId()
    {
        var ticketId = 1;
        _ticketServiceMock.Setup(service => service.DeleteAsync(ticketId)).ReturnsAsync(ticketId);

        var result = await _ticketController.DeleteById(ticketId);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);

        var restResult = okResult.Value as RestDTO<int>;
        restResult.Should().NotBeNull();
        restResult!.Data.Should().Be(ticketId);
    }
}
