namespace Try.Management.Infrastructure;
public interface IMongoDbFactory
{
    ValueTask<IMongoDatabase> GetAsync(CancellationToken ct);
}

public abstract class MongoDbFactory<TOptions>(IOptions<TOptions> options, IMongoClientFactory factory) : IMongoDbFactory
    where TOptions : MongoDbOptions, new()
{
    private readonly ConcurrentDictionary<string, IMongoDatabase> _cache = new();
    private readonly TOptions _options = options.Value;
    private readonly IMongoClientFactory _factory = factory;

    public async ValueTask<IMongoDatabase> GetAsync(CancellationToken ct)
    {
        var name = _options.Name.Trim().ToLowerInvariant();
        if (_cache.ContainsKey(name)) return _cache[name];
        var client = _factory.GetOrCreate();
        var names = await (await client.ListDatabaseNamesAsync().ConfigureAwait(false)).ToListAsync(ct).ConfigureAwait(false); //ConfigureAwaiit(false)?
        if (!names.Any(x => x.Equals(name, StringComparison.InvariantCultureIgnoreCase))) throw new ArgumentException($"Database: {name} doesn't exist.");
        _cache[name] = client.GetDatabase(name);
        return _cache[name];
    }
}
