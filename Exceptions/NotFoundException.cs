namespace CSBlog.Exceptions;

public class NotFoundException : HttpRequestException
{
    public NotFoundException(string message) : base(message) { }
}
