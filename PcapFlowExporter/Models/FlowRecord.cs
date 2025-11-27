using System;

namespace PcapFlowExporter.Models;

public class FlowRecord
{
    public int FlowId { get; set; }

    public string SrcIp { get; set; } = "";
    public string DstIp { get; set; } = "";
    public int SrcPort { get; set; }
    public int DstPort { get; set; }
    public string Protocol { get; set; } = "";

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double DurationSeconds => (EndTime - StartTime).TotalSeconds;

    public int PacketCount { get; set; }
    public long TotalBytes { get; set; }

    public double BytesPerSecond =>
        DurationSeconds > 0 ? TotalBytes / DurationSeconds : TotalBytes;

    public double PacketsPerSecond =>
        DurationSeconds > 0 ? PacketCount / DurationSeconds : PacketCount;
}
