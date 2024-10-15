namespace TicketService.API.DTO.Templates;


public class RestDTO<T> 
{
    /// <summary>
    /// Gets or sets the list of links. Implementing the principle of HATEOAS.
    /// </summary>
    
    public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    public T Data { get; set; } = default!;
    public static RestBuilder Builder()
    {
        return new RestBuilder();
    }
    public class RestBuilder
    {
        private RestDTO<T> _restDTO;
        public RestBuilder()
        {
            _restDTO = new RestDTO<T>();
        }
        public RestBuilder WithLink(LinkDTO links)
        {
            _restDTO.Links.Add(links);
            return this;
        }
        public RestBuilder WithData(T data)
        {
            _restDTO.Data = data;
            return this;
        }
        public RestDTO<T> Build()
        {
            return _restDTO;
        }
    }

}
