namespace ServeSync.API.Dtos.Shared;

public class SimpleIdResponse<T>
{
    public T Id { get; set; }

    public static SimpleIdResponse<T> Create(T id)
    {
        return new SimpleIdResponse<T>
        {
            Id = id
        };
    }
}