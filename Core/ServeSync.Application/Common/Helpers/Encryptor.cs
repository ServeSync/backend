using System.Text;
using System.Text.Json;
using System.Web;

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
    
    public static T Base64Decode<T>(string base64EncodedData)
    { ;
        var json = Base64Decode(base64EncodedData);
        return JsonSerializer.Deserialize<T>(json);
    }
    
    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(HttpUtility.UrlDecode(base64EncodedData));
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
}