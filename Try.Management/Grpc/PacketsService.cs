using Try.Management.Protos;

namespace Try.Management.Grpc;
public class PacketsService(IPacketsCollectionFactory packetsCollectionFactory, IMapper mapper) : Packets.PacketsBase
{
    private readonly IPacketsCollectionFactory _packetsCollectionFactory = packetsCollectionFactory;
    private readonly IMapper _mapper = mapper;

    public override async Task<GetAllResponse> GetAll(GetAllRequest request, ServerCallContext context)
    {
        var packets = await _packetsCollectionFactory.GetAsync<Protos.Packet>(context.CancellationToken);
        return _mapper.Map<GetAllResponse>(packets);
    }
}
