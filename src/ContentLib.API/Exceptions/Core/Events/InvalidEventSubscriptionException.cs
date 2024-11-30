using System;

namespace ContentLib.API.Exceptions.Core.Events;
/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class InvalidEventSubscriptionException<T> : Exception
{
    public override string Message => "Invalid event subscription for: ";
}