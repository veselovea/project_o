using Newtonsoft.Json;
using System;

internal class Serializer
{
    public static string GetJson(object from)
    {
        try
        {
            string json = JsonConvert.SerializeObject(from);
            return json;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static T GetObject<T>(string json) where T : class
    {
        try
        {
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}
