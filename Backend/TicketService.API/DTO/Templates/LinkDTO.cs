namespace TicketService.API.DTO.Templates;


public class LinkDTO
{
    public string? Href { get; set; }
    public string? Rel { get; set; }
    public string? Type { get; set; }
    public static LinkBuilder Builder()
    {
        return new LinkBuilder();
    }
    public class LinkBuilder
    {
        private LinkDTO _linkDTO;
        public LinkBuilder()
        {
            _linkDTO = new LinkDTO();
        }
        public LinkBuilder WithHref(string href)
        {
            _linkDTO.Href = href;
            return this;
        }
        public LinkBuilder WithRel(string rel)
        {
            _linkDTO.Rel = rel;
            return this;
        }
        public LinkBuilder WithType(string type)
        {
            _linkDTO.Type = type;
            return this;
        }
        public LinkDTO Build()
        {
            return _linkDTO;
        }
    }
}
