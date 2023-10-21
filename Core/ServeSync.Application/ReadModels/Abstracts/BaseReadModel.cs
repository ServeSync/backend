namespace ServeSync.Application.ReadModels.Abstracts;

public class BaseReadModel<T> where T : IEquatable<T>
{
    public T Id { get; set; }
}