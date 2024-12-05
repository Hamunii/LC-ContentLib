using System;
using ContentLib.API.Exceptions.Util;
using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Item;

namespace ContentLib.API.Exceptions.Core.Manager;
/// <summary>
/// Exception thrown when a registration of an instance to one of the API's managers is invalid. The ToString method
/// of this Exception produces the initial exception message, along with the properties of the invalid object (including
/// invalid properties), followed by the stacktrace. 
/// </summary>
/// <typeparam name="T">Type Parameter of the instance that failed to register.</typeparam>
public abstract class InvalidRegistrationException<T>(T invalidInstance,Exception exception) : Exception
{
    private readonly Exception _exception = exception;

    /// <summary>
    /// The instance that failed to register.
    /// </summary>
    private T InvalidInstance { get; } = invalidInstance;


    /// <summary>
    /// Gets the current status of the invalid instance, specifically its properties.
    /// </summary>
    /// <returns>string containing the properties of the invalid instance.</returns>
  

    /// <inheritdoc />
    public override string Message => $"{base.Message}\nRegistration of type {InvalidInstance.GetType().Name} failed!";

    /// <inheritdoc />
    public override string ToString() =>
        $"{Message}\n" + 
        $"Failed Object Properties:\n{DebugUtils.GetFailedInstancePropertiesStatus(invalidInstance)}\n\n" +
        $"Reason: {_exception.Message}\n"+
        $"Stack Trace:\n{StackTrace}";
}

/// <summary>
/// Exception for an invalid IGameEntity registration.
/// </summary>
/// <param name="invalidEntity">The Entity that failed to register.</param>
public class InvalidEntityRegistrationException(IGameEntity invalidEntity, Exception exception)
    : InvalidRegistrationException<IGameEntity>(invalidEntity, exception);

/// <summary>
/// Exception for an invalid IGameItem registration.
/// </summary>
/// <param name="invalidItem">The Item that failed to register.</param>
public class InvalidItemRegistrationException(IGameItem invalidItem, Exception exception)
    : InvalidRegistrationException<IGameItem>(invalidItem, exception);

    
