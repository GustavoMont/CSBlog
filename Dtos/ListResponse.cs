namespace CSBlog.Dtos;

public class ListResponse<T>
{
    public ListResponse(int page, int total, int take)
    {
        Page = page;
        var pageCount = (total / (double)take);
        PageCount = (int)Math.Ceiling(pageCount);
        Count = total;
    }

    public List<T> Results { get; set; }
    public int Page { get; set; }
    public int PageCount { get; set; }
    public int Count { get; set; }
}
