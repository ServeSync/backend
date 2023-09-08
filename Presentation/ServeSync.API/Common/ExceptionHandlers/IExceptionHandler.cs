namespace ServeSync.API.Common.ExceptionHandlers;

public interface IExceptionHandler
{
    Task HandleAsync(HttpContext context, Exception exception);
}