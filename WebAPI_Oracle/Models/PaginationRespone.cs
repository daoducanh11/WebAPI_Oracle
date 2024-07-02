namespace WebAPI_Oracle.Models
{
    public class PaginationRespone<TEntity>
    {
        #region Constructor
        public PaginationRespone(int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
        }
        #endregion
        #region Property
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalItem { get; set; }
        public List<TEntity> Data { get; set; }
        #endregion
    }
}
