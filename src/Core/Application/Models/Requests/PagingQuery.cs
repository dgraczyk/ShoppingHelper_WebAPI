namespace Application.Models.Requests
{
    public abstract class PagingQuery
    {
        private int pageNumber = 1;
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                pageNumber = (value > 0) ? value : 1;
            }
        }

        private int pageSize = 10;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > 0) ? value : 10;
            }
        }
    }
}
