using System.Reflection;

namespace DomainDevKit;

public abstract record EnumEntity<T> where T : EnumEntity<T>
{
    private static readonly Lazy<T[]> _values = new(FindValues, true);
    private static readonly Lazy<Dictionary<int, T>> _byId = new(() => Values.ToDictionary(v => v.Id, v => v), true);
    private static readonly Lazy<Dictionary<string, T>> _byName = new(() => Values.ToDictionary(v => v.Name, v => v, StringComparer.OrdinalIgnoreCase), true);
    protected EnumEntity(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; protected init; }
    public string Name { get; protected init; }

    public static T[] Values => _values.Value;

    public static T ById(int id)
    {
        if (!_byId.Value.TryGetValue(id, out var value))
            throw new InvalidOperationException($"Value with id '{id}' not found.");

        return value;
    }

    public static T ByName(string name)
    {
        if (!_byName.Value.TryGetValue(name, out var value))
            throw new InvalidOperationException($"Value with name '{name}' not found.");

        return value;
    }

    private static T[] FindValues()
    {
        var type = typeof(T);
        var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly;

        return type
            .GetFields(flags)
            .Where(t => t.IsInitOnly && t.FieldType.Equals(typeof(T)))
            .Select(t => (T)t.GetValue(null)!)
            .ToArray();
    }
}