public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; } //Totalcount
    public int Page { get; set; } //Page
    public int PageSize { get; set; } //pagesize
}