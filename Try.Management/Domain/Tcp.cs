using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Try.Management.Domain;
public partial class Tcp
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = default!;

    public uint SequenceNumber { get; set; }
    public uint Acknowledgment { get; set; }
    public byte DataOffset { get; set; }
    public ushort ControlFlags { get; set; }
    public ushort WindowSize {  get; set; }
    public string Checksum { get; set; } = default!;
    public ushort UrgentPoint { get; set; }
    public string Options {  get; set; } = default!;

    public string Payload { get; set; } = default!;

    public ushort SourcePort { get; set; } = default!;
    public ushort DestinationPort { get; set; } = default!;
}
