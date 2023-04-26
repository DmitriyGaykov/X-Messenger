using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lib.Converters;

public static class MyJsonConverter
{
    public static string ToJson(object? obj)
    {
        if (obj is null) throw new ArgumentNullException();
        
        string answer = JsonConvert.SerializeObject(obj);

        return answer;
    }

    public static T? FromJson<T>(string json) where T : class
    {
        try
        {
            T? answer = JsonConvert.DeserializeObject<T>(json) as T;
            return answer;
        }
        catch
        {
            return null;
        }
    }

    public static byte[] ToBytes(object? obj)
    {
        if (obj is null) return null;

        string json = ToJson(obj);
        var bytes = Encoding.UTF8.GetBytes(json);

        return bytes;
    }
}
