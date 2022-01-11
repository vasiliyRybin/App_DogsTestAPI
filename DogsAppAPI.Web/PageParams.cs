namespace DogsAppAPI.Web
{
    public class PageParams
    {
        const int maxPageSize = 100;
        private int _pageSize = 10;
        private int _pageNumber = 1;
        public string Attribute { get; set; }
        private string _order = "asc";

        public string Order
        {
            get { return _order; }
            set { _order = (value.ToLower() == "desc") ? _order = value.ToLower() : _order; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : (value < 0) ? _pageSize = 1 : value; }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = (value < 0) ? _pageNumber : value; }
        }
    }
}
