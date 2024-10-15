namespace TicketService.API.DTO.Templates;

public class PageDTO<T>
{
    public int PageSize { get; private set; }
    public int PageIndex { get; private set; }
    public int RecordCount { get; private set; }
    public int TotalCount { get; private set; }

    public ICollection<T>? Data { get; private set; }

    public static PageBuilder Builder()
    {
        return new PageBuilder();
    }

    public class PageBuilder
    {
        private PageDTO<T> _pageDTO;

        public PageBuilder()
        {
            _pageDTO = new PageDTO<T>
            {
                PageSize = 10,
                PageIndex = 0,
                RecordCount = 10,
                TotalCount = -1
            };
        }

        public PageBuilder WithPageSize(int pageSize)
        {
            _pageDTO.PageSize = pageSize;
            return this;
        }

        public PageBuilder WithPageIndex(int pageIndex)
        {
            _pageDTO.PageIndex = pageIndex;
            return this;
        }

        public PageBuilder WithRecordCount(int recordCount)
        {
            _pageDTO.RecordCount = recordCount;
            return this;
        }

        public PageBuilder WithTotalRecords(int totalCount)
        {
            _pageDTO.TotalCount = totalCount;
            return this;
        }

        public PageBuilder WithData(ICollection<T> data)
        {
            _pageDTO.Data = data;
            return this;
        }

        public PageDTO<T> Build()
        {
            return _pageDTO;
        }
    }
}
