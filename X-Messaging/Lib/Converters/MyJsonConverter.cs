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
        return JsonConvert.DeserializeObject<T>(json) as T;
    }
}
