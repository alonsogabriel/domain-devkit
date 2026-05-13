using System.Collections;
using System.Runtime.CompilerServices;

namespace DomainDevKit;

public interface ILocaleList<T> : IReadOnlyList<T> where T : ILocaleData
{
    T? Get(string locale);
    bool ContainsLocale(string locale);
}

public class LocaleList<T> : IList<T>, ILocaleList<T> where T : ILocaleData
{
    private readonly List<T> _items = [];
    private readonly Lazy<Dictionary<string, T>> _locales;

    public LocaleList()
    {
        _locales = new(() =>
        {
            return _items.ToDictionary(i => i.Locale.Value, i => i, StringComparer.OrdinalIgnoreCase);
        });
    }

    public int Count => _items.Count;

    public bool IsReadOnly => false;

    public T this[int index]
    {
        get => _items[index];
        set
        {
            var current = _items[index];

            if (value.Locale.Equals(current.Locale))
            {
                _items[index] = value;
                return;
            }

            CheckLocaleToAdd(value.Locale.Value, nameof(value));

            _locales.Value.Remove(_items[index].Locale.Value);
            _items[index] = value;
            _locales.Value.Add(value.Locale.Value, value);
        }
    }

    public T? Get(string locale)
    {
        if (_locales.Value.TryGetValue(locale, out var data))
            return data;

        return default;
    }

    public int IndexOf(T item)
    {
        return _items.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        if (index < 0 || index > _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        CheckLocaleToAdd(item.Locale.Value, nameof(item));
        _items.Insert(index, item);
        _locales.Value.Add(item.Locale.Value, item);
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        _locales.Value.Remove(_items[index].Locale.Value);
        _items.RemoveAt(index);
    }

    public void Add(T item)
    {
        CheckLocaleToAdd(item.Locale.Value, nameof(item));
        _items.Add(item);
        _locales.Value.Add(item.Locale.Value, item);
    }

    public bool ContainsLocale(string locale)
    {
        return _locales.Value.ContainsKey(locale);
    }

    private void CheckLocaleToAdd(string locale, [CallerArgumentExpression(nameof(locale))] string? paramName = null)
    {
        if (ContainsLocale(locale))
            throw new ArgumentException($"There is already an item for locale '{locale}'.", paramName);
    }

    public void Clear()
    {
        _locales.Value.Clear();
        _items.Clear();
    }

    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        if (_items.Remove(item))
        {
            _locales.Value.Remove(item.Locale.Value);
            return true;
        }

        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}