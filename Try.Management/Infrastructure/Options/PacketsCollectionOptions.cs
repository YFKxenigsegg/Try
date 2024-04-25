namespace Try.Management.Infrastructure.Options;
public class PacketsCollectionOptions : MongoCollectionOptions { }

public interface IPacketsCollectionFactory : IMongoCollectionFactory { }

public sealed class PacketsCollectionFactory(IOptions<PacketsCollectionOptions> options, IManagementDbFactory dbFactory)
    : MongoCollectionFactory<PacketsCollectionOptions, IManagementDbFactory>(options, dbFactory), IPacketsCollectionFactory // check 2nd T
{ }
