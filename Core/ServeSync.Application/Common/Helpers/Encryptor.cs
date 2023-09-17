using System.Text;
using System.Text.Json;

namespace ServeSync.Application.Common.Helpers;

public static class Encryptor
{
    public static string Base64Encode<T>(object obj)
    {
        var json = JsonSerializer.Serialize(obj, typeof(T));
        return Base64Encode(json);
    }
    
    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
}