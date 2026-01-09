public class PagedResult<T>
{
    public List<T> Items { get; set; } = new(); //Items
    public int TotalCount { get; set; } //Totalcount123
    public int Page { get; set; } //Page
    public int PageSize { get; set; } //pagesize123
}