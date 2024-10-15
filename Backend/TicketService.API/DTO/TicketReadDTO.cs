namespace TicketService.API.DTO
{
    public record TicketResponseDTO(
        int Id,
        string Description,
        string Status,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt
    );
}
