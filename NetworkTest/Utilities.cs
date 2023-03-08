using Newtonsoft.Json;
using System;

public class Serializer
{
    public static string GetJson(object from, Action<string> exceptionEvent = null)
    {
        try
        {
            string json = JsonConvert.SerializeObject(from);
            return json;
        }
        catch (Exception e)
        {
            exceptionEvent?.Invoke($"[XX] {e.Message}");
        }
        return "";
    }

    public static T GetObject<T>(string json, Action<string> exceptionEvent = null) where T : class
    {
        try
        {
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        catch (Exception e)
        {
            exceptionEvent?.Invoke($"[XX] {e.Message}");
        }
        return null;
    }
}