namespace DomainDevKit;

public abstract record ObjectId<T>(T Value) : IValueObject<T> where T : notnull;