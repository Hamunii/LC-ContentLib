using System;

namespace ContentLib.API.Exceptions.Util;

public class DebugUtils
{
    /// <summary>
    /// <p>Utility Method for taking in any Instance of Type T, and printing out the name & value of every property within
    /// the object or the Exception that is thrown from the attempt to obtain said object.</p>
    /// <i>Developer Note: The main use-case for utilising this Utility is for Custom Exceptions, to aid in logging
    /// exactly the properties that threw exceptions during some sort of logic that accessed them.</i></summary>
    /// <param name="invalidInstance">The instance that failed to register.</param>
    /// <typeparam name="T">The Type Parameter of the invalid instance.</typeparam>
    /// <returns>A string of all the properties within the Invalid Instance.</returns>
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
            catch(Exception ex)
            {
                result += $"{property.Name}: {ex.Message}";
            }
        }

        return result.TrimEnd(' ', ',');
    }
}