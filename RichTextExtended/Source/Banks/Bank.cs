using System.Collections.Generic;

namespace RichTextExtended.Source.Banks;

public abstract class Bank<T>
{
    private readonly Dictionary<string, T> _keyValuePairs = [];


    public bool TryGetValue(string key, out T value)
    {
        return _keyValuePairs.TryGetValue(key, out value);
    }

    public T GetValue(string key)
    {
        return _keyValuePairs[key];
    }

    public bool TryAdd(string key, T value)
    {
        return _keyValuePairs.TryAdd(key, value);
    }

    public void Add(string key, T value)
    {
        _keyValuePairs.Add(key, value);
    }

    public bool Remove(string key)
    {
        return _keyValuePairs.Remove(key);
    }

    public void Replace(string key, T newValue)
    {
        Remove(key);
        Add(key, newValue);
    }
}
