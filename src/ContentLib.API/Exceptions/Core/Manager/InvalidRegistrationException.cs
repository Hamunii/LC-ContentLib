using System;
using ContentLib.API.Model.Entity;
using ContentLib.API.Model.Item;

namespace ContentLib.API.Exceptions.Core.Manager;
/// <summary>
/// Exception thrown when a registration of an instance to one of the API's managers is invalid. The ToString method
/// of this Exception produces the initial exception message, along with the properties of the invalid object (including
/// invalid properties), followed by the stacktrace. 
/// </summary>
/// <typeparam name="T">Type Parameter of the instance that failed to register.</typeparam>
public abstract class InvalidRegistrationException<T> : Exception
{
    /// <summary>
    /// The instance that failed to register.
    /// </summary>
    private T InvalidInstance { get; }

    /// <summary>
    /// Constructor that initialises the instance that failed to register.
    /// </summary>
    /// <param name="invalidInstance">The instance that failed to register.</param>
    protected InvalidRegistrationException(T invalidInstance)
    {
        InvalidInstance = invalidInstance;
    }
    
    /// <summary>
    /// Gets the current status of the invalid instance, specifically its properties.
    /// </summary>
    /// <returns>string containing the properties of the invalid instance.</returns>
    public string GetFailedInstancePropertiesStatus()
    {
        var properties = typeof(T).GetProperties();
        var result = "";

        foreach (var property in properties)
        {
            try
            {
                var value = property.GetValue(InvalidInstance);
                result += $"{property.Name}: {value}, ";
            }
            catch
            {
                result += $"{property.Name}: THREW EXCEPTION, ";
            }
        }

        return result.TrimEnd(' ', ',');
    }

    /// <inheritdoc />
    public override string Message => $"{base.Message}\nRegistration of type {InvalidInstance.GetType().Name} failed!";

    /// <inheritdoc />
    public override string ToString() =>
        $"{Message}\n" + 
        $"Failed Object Properties:\n{GetFailedInstancePropertiesStatus()}\n\n" +
        $"Stack Trace:\n{StackTrace}";
}

/// <summary>
/// Exception for an invalid IGameEntity registration.
/// </summary>
/// <param name="invalidEntity">The Entity that failed to register.</param>
public class InvalidEntityRegistrationException(IGameEntity invalidEntity)
    : InvalidRegistrationException<IGameEntity>(invalidEntity);

/// <summary>
/// Exception for an invalid IGameItem registration.
/// </summary>
/// <param name="invalidItem">The Item that failed to register.</param>
public class InvalidItemRegistrationException(IGameItem invalidItem)
    : InvalidRegistrationException<IGameItem>(invalidItem);

    
