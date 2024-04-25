namespace Try.Management.Infrastructure;
public interface IMongoCollectionFactory
{
    ValueTask<IMongoCollection<T>> GetAsync<T>(CancellationToken ct) where T : class, new();
}

public abstract class MongoCollectionFactory<TOptions, TFactory>(IOptions<TOptions> options, TFactory dbFactory) : IMongoCollectionFactory
    where TOptions : MongoCollectionOptions, new()
    where TFactory : IMongoDbFactory
{
    private readonly ConcurrentDictionary<string, object> _cache = new();
    private readonly TOptions _options = options.Value;
    private readonly TFactory _dbFactory = dbFactory;

    public async ValueTask<IMongoCollection<T>> GetAsync<T>(CancellationToken ct) where T : class, new()
    {
        var name = _options.Name.Trim().ToLowerInvariant();
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Collection not configured.");
        IMongoDatabase db = await _dbFactory.GetAsync(ct).ConfigureAwait(false); // ?
        if (_cache.ContainsKey(name)) return (IMongoCollection<T>)_cache[name];
        _cache[name] = GetMongoCollection(db, name);
        return (IMongoCollection<T>)_cache[name];

        static IMongoCollection<T> GetMongoCollection(IMongoDatabase db, string name)
        {
            var getCollectionMethod = db.GetType().GetMethod(nameof(IMongoDatabase.GetCollection));
            var definition = getCollectionMethod.GetGenericMethodDefinition();
            var getCollection = getCollectionMethod.MakeGenericMethod(new Type[] { typeof(T) });
            var collection = getCollection.Invoke(db, new object[] { name, new MongoCollectionSettings() });
            return (IMongoCollection<T>)collection!;
        }
    }
}
