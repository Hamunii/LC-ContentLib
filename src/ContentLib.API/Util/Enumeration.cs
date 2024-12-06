using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ContentLib.API.Util;

public abstract class Enumeration<TValue> : IComparable where TValue : IComparable
{
    private readonly TValue _value;
    private readonly string _displayName;

    protected Enumeration()
    {
    }

    protected Enumeration(TValue value, string displayName)
    {
        _value = value;
        _displayName = displayName;
    }

    public TValue Value => Value;

    public string DisplayName => _displayName;

    public override string ToString() => DisplayName;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration<TValue>, new()
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            var instance = new T();
            var locatedValue = info.GetValue(instance) as T;

            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    public override bool Equals(object obj)
    {
        var otherValue = obj as Enumeration<TValue>;

        if (otherValue == null)
        {
            return false;
        }

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = EqualityComparer<TValue>.Default.Equals(_value, otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode() => EqualityComparer<TValue>.Default.GetHashCode(_value);
    public static T FromValue<T>(TValue value) where T : Enumeration<TValue>, new()
    {
        var matchingItem = parse<T, TValue>(value, "value", item => EqualityComparer<TValue>.Default.Equals(item.Value, value));
        return matchingItem;
    }

    public static T FromDisplayName<T>(string displayName) where T : Enumeration<TValue>, new()
    {
        var matchingItem = parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
        return matchingItem;
    }

    private static T parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration<TValue>, new()
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
        {
            var message = $"'{value}' is not a valid {description} in {typeof(T)}";
            throw new ApplicationException(message);
        }

        return matchingItem;
    }

    public int CompareTo(object other) => Value.CompareTo(((Enumeration<TValue>)other).Value);
}
