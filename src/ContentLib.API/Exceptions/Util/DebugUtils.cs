namespace ContentLib.API.Exceptions.Util;

public class DebugUtils
{
    public static string GetFailedInstancePropertiesStatus<T>(T invalidInstance)
    {
        var properties = typeof(T).GetProperties();
        var result = "";

        foreach (var property in properties)
        {
            try
            {
                var value = property.GetValue(invalidInstance);
                result += $"{property.Name}: {value}, ";
            }
            catch
            {
                result += $"{property.Name}: THREW EXCEPTION, ";
            }
        }

        return result.TrimEnd(' ', ',');
    }
}