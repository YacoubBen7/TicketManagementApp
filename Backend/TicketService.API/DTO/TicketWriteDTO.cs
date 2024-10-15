using System.ComponentModel.DataAnnotations;
using TicketService.API.Validators;
using TicketService.Domain.Enums;

namespace TicketService.API.DTO
{
    public record TicketCreateDTO([Required] string Description);

    public record TicketUpdateDTO(
        [Required] string Description,
        [StatusValidator<Status>] string Status
    );
}
