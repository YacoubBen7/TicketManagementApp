using System.ComponentModel.DataAnnotations;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using TicketService.API.DTO;
using TicketService.API.DTO.Templates;
using TicketService.API.Validators;
using TicketService.Core.ServicesContracts;
using TicketService.Domain.Models;

namespace TicketService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RestDTO<TicketResponseDTO>), StatusCodes.Status201Created)]
        public async Task<ActionResult<RestDTO<TicketResponseDTO>>> CreateAsync(
            TicketCreateDTO ticketCreateDTO
        )
        {
            _logger.LogInformation(
                "Adding ticket Endpoint has been called with {ticketCreateDTO}",
                ticketCreateDTO
            );

            var ticketResponse = (
                await _ticketService.AddAsync(ticketCreateDTO.Adapt<Ticket>())
            ).Adapt<TicketResponseDTO>();
            return CreatedAtAction(
                nameof(GetById),
                new { id = ticketResponse.Id },
                RestDTO<TicketResponseDTO>
                    .Builder()
                    .WithData(ticketResponse)
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithRel("self")
                            .WithHref(Url.Action(nameof(GetById), new { id = ticketResponse.Id })!)
                            .WithType("GET")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(GetAll))!)
                            .WithRel("all")
                            .WithType("GET")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(Patch), new { id = ticketResponse.Id })!)
                            .WithRel("update")
                            .WithType("PATCH")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(
                                Url.Action(nameof(DeleteById), new { id = ticketResponse.Id })!
                            )
                            .WithRel("delete")
                            .WithType("DELETE")
                            .Build()
                    )
                    .Build()
            );
        }

        [HttpGet]
        [ResponseCache(NoStore = true)]
        [ProducesResponseType(typeof(RestDTO<TicketResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<RestDTO<List<TicketResponseDTO>>>> GetAll(
            int pageIndex = 0,
            [Range(0, 100)] int pageSize = 10,
            [SortColumnValidator<Ticket>] string sortColumn = "Id",
            [SortOrderValidator()] string sortOrder = "ASC",
            string filterQuery = ""
        )
        {
            _logger.LogInformation("Getting all tickets Endpoint has been called");

            var tickets = await _ticketService.GetAllAsync(
                pageIndex,
                pageSize,
                sortColumn,
                sortOrder,
                filterQuery
            );
            return Ok(
                RestDTO<PageDTO<TicketResponseDTO>>
                    .Builder()
                    .WithData(
                        PageDTO<TicketResponseDTO>
                            .Builder()
                            .WithPageSize(pageSize)
                            .WithPageIndex(pageIndex)
                            .WithRecordCount(tickets.Count)
                            .WithTotalRecords(await _ticketService.CountAsync(filterQuery))
                            .WithData(tickets.Adapt<List<TicketResponseDTO>>())
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(GetAll))!)
                            .WithRel("all")
                            .WithType("GET")
                            .Build()
                    )
                    .Build()
            );
        }

        [HttpGet("{Id:int}")]
        [ResponseCache(NoStore = true)]
        [ProducesResponseType(typeof(RestDTO<TicketResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<RestDTO<TicketResponseDTO>>> GetById(int Id)
        {
            var ticketResponse = (await _ticketService.GetAsync(Id)).Adapt<TicketResponseDTO>();
            return Ok(
                RestDTO<TicketResponseDTO>
                    .Builder()
                    .WithData(ticketResponse)
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(GetAll))!)
                            .WithRel("all")
                            .WithType("GET")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref("/api/Ticket")
                            .WithRel("create")
                            .WithType("POST")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(Patch), new { id = ticketResponse.Id })!)
                            .WithRel("update")
                            .WithType("PATCH")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(
                                Url.Action(nameof(DeleteById), new { id = ticketResponse.Id })!
                            )
                            .WithRel("delete")
                            .WithType("DELETE")
                            .Build()
                    )
                    .Build()
            );
        }

        [HttpPatch("{Id:int}", Name = nameof(Patch))]
        [ResponseCache(NoStore = true)]
        [ProducesResponseType(typeof(RestDTO<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<RestDTO<int>>> Patch(int Id, TicketUpdateDTO ticket)
        {
            var tmp = ticket.Adapt<Ticket>();
            tmp.Id = Id;
            var response = await _ticketService.UpdateAsync(tmp);
            return Ok(
                RestDTO<int>
                    .Builder()
                    .WithData(response)
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(GetAll))!)
                            .WithRel("all")
                            .WithType("GET")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref("/api/Ticket")
                            .WithRel("create")
                            .WithType("POST")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(Patch), new { id = Id })!)
                            .WithRel("update")
                            .WithType("PATCH")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(DeleteById), new { id = Id })!)
                            .WithRel("delete")
                            .WithType("DELETE")
                            .Build()
                    )
                    .Build()
            );
        }

        [HttpDelete("{Id:int}")]
        [ResponseCache(NoStore = true)]
        [ProducesResponseType(typeof(RestDTO<int>), StatusCodes.Status200OK)]
        public async Task<ActionResult<RestDTO<int>>> DeleteById(int Id)
        {
            return Ok(
                RestDTO<int>
                    .Builder()
                    .WithData(await _ticketService.DeleteAsync(Id))
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref(Url.Action(nameof(GetAll))!)
                            .WithRel("all")
                            .WithType("GET")
                            .Build()
                    )
                    .WithLink(
                        LinkDTO
                            .Builder()
                            .WithHref("/api/Ticket")
                            .WithRel("create")
                            .WithType("POST")
                            .Build()
                    )
                    .Build()
            );
        }
    }
}
