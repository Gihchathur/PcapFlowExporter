using System.Net;

namespace PcapFlowExporter.Models;

public readonly struct FlowKey
{
    public IPAddress SrcIp { get; }
    public IPAddress DstIp { get; }
    public int SrcPort { get; }
    public int DstPort { get; }
    public int Protocol { get; }

    public FlowKey(IPAddress srcIp, IPAddress dstIp, int srcPort, int dstPort, int protocol)
    {
        SrcIp = srcIp;
        DstIp = dstIp;
        SrcPort = srcPort;
        DstPort = dstPort;
        Protocol = protocol;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not FlowKey other) return false;
        return SrcIp.Equals(other.SrcIp)
               && DstIp.Equals(other.DstIp)
               && SrcPort == other.SrcPort
               && DstPort == other.DstPort
               && Protocol == other.Protocol;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + SrcIp.GetHashCode();
            hash = hash * 23 + DstIp.GetHashCode();
            hash = hash * 23 + SrcPort.GetHashCode();
            hash = hash * 23 + DstPort.GetHashCode();
            hash = hash * 23 + Protocol.GetHashCode();
            return hash;
        }
    }
}
