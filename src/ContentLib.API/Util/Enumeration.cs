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

    public TValue Value
    {
        get { return _value; }
    }

    public string DisplayName
    {
        get { return _displayName; }
    }

    public override string ToString()
    {
        return DisplayName;
    }

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

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = EqualityComparer<TValue>.Default.Equals(_value, otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TValue>.Default.GetHashCode(_value);
    }

    public static int AbsoluteDifference(Enumeration<TValue> firstValue, Enumeration<TValue> secondValue)
    {
        if (typeof(TValue) != typeof(int))
        {
            throw new InvalidOperationException("AbsoluteDifference is only supported for int-based Enumeration.");
        }

        var firstInt = Convert.ToInt32(firstValue.Value);
        var secondInt = Convert.ToInt32(secondValue.Value);

        return Math.Abs(firstInt - secondInt);
    }

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
            var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
            throw new ApplicationException(message);
        }

        return matchingItem;
    }

    public int CompareTo(object other)
    {
        return Value.CompareTo(((Enumeration<TValue>)other).Value);
    }
}
