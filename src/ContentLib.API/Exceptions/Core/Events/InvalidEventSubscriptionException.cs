using System;
using ContentLib.API.Exceptions.Util;
using ContentLib.API.Model.Event;

namespace ContentLib.API.Exceptions.Core.Events;
/// <summary>
/// Exception thrown when a triggering of an in-game event is invalid. The to string method of this exception produces
/// the initial exception message, along with properties of the invalid event.
/// </summary>
/// <typeparam name="T">The Type Parameter of the in-game event that failed to be triggered to.</typeparam>
public class InvalidEventTriggerException<T>(T invalidEvent, Exception exception) : Exception where T : IGameEvent
{
    /// <summary>
    /// The Invalid Event that failed to trigger.
    /// </summary>
    private readonly T _invalidEvent = invalidEvent;
    
    /// <summary>
    /// The exception that caused the triggering of the event to be invalid. 
    /// </summary>
    private readonly Exception _exception = exception;
    /// <summary>
    /// Getter for the Invalid Event.
    /// </summary>
    public T InvalidEvent => _invalidEvent;

    /// <inheritdoc />
    public override string Message => $"Triggering of {_invalidEvent.GetType().Name} failed!";
    /// <summary>
    /// Overridden ToString method instead shows the failure message, the properties of the failed Event, the reason
    /// for the failure (the original exception message) and the stacktrace. 
    /// </summary>
    /// <returns>The Exception, in the form of the key exception information.</returns>
    public override string ToString() =>
        $"{Message}\n" + 
        $"Failed Event Properties:\n{DebugUtils.GetFailedInstancePropertiesStatus(InvalidEvent)}\n\n" +
        $"Reason: {_exception.Message}\n"+
        $"Stack Trace:\n{StackTrace}";
    
}