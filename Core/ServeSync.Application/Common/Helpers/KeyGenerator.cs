namespace ServeSync.Application.Common.Helpers;

public static class KeyGenerator
{
    public static string Generate<T>(string resourceName, T id) where T : IEquatable<T>
    {
        return $"{resourceName}:{id.ToString()}";
    }
}