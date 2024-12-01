using System;
using ContentLib.API.Model.Event;

namespace ContentLib.API.Exceptions.Core.Events;
/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class InvalidEventSubscriptionException<T>(T invalidEvent, Exception exception) : Exception where T : IGameEvent
{
    private readonly T _invalidEvent = invalidEvent;
    public T InvalidEvent => _invalidEvent;
    //TODO Make a util class for this as its likely gonna be used a lot. 
    public string GetFailedInstancePropertiesStatus()
    {
        var properties = typeof(T).GetProperties();
        var result = "";

        foreach (var property in properties)
        {
            try
            {
                var value = property.GetValue(InvalidEvent);
                result += $"{property.Name}: {value}, ";
            }
            catch
            {
                result += $"{property.Name}: THREW EXCEPTION, ";
            }
        }

        return result.TrimEnd(' ', ',');
    }
    public override string Message => $"Subscription of {_invalidEvent.GetType().Name} failed!";
}