namespace ServeSync.Application.Identity.Dtos;

public class IdentityResult<T>
{
    public bool IsSuccess { get; private set; }
    
    public string? Error { get; private set; }
    public string? ErrorCode { get; private set; }
    
    public T? Data { get; private set; }
    
    public static IdentityResult<T> Success(T? data)
    {
        return new IdentityResult<T>
        {
            IsSuccess = true,
            Data = data
        };
    }
    
    public static IdentityResult<T> Failed(string errorCode, string error, T? data = default(T))
    {
        return new IdentityResult<T>
        {
            IsSuccess = false,
            ErrorCode = errorCode,
            Error = error,
            Data = data
        };
    }
}