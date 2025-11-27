using CsvHelper;
using PacketDotNet;
using PcapFlowExporter.Models;
using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace PcapFlowExporter.Services;

public class PcapFlowService
{
    public List<FlowRecord> ProcessPcapToFlows(string pcapPath)
    {
        var device = new CaptureFileReaderDevice(pcapPath);
        device.Open();

        var flows = new Dictionary<FlowKey, FlowRecord>();

        device.OnPacketArrival += (sender, e) =>
        {
            try
            {
                var rawCapture = e.GetPacket();

                var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);

                var ipPacket = packet.Extract<IPPacket>();
                if (ipPacket == null) return;

                IPAddress srcIp = ipPacket.SourceAddress;
                IPAddress dstIp = ipPacket.DestinationAddress;
                int protocol = (int)ipPacket.Protocol;

                int srcPort = 0;
                int dstPort = 0;
                string protocolName = ipPacket.Protocol.ToString();

                if (ipPacket.PayloadPacket is TcpPacket tcp)
                {
                    srcPort = tcp.SourcePort;
                    dstPort = tcp.DestinationPort;
                    protocolName = "TCP";
                }
                else if (ipPacket.PayloadPacket is UdpPacket udp)
                {
                    srcPort = udp.SourcePort;
                    dstPort = udp.DestinationPort;
                    protocolName = "UDP";
                }

                var key = new FlowKey(srcIp, dstIp, srcPort, dstPort, protocol);

                if (!flows.TryGetValue(key, out var flow))
                {
                    flow = new FlowRecord
                    {
                        SrcIp = srcIp.ToString(),
                        DstIp = dstIp.ToString(),
                        SrcPort = srcPort,
                        DstPort = dstPort,
                        Protocol = protocolName,
                        StartTime = rawCapture.Timeval.Date,
                        EndTime = rawCapture.Timeval.Date,
                        PacketCount = 0,
                        TotalBytes = 0
                    };
                    flows[key] = flow;
                }

                // update stats
                flow.PacketCount += 1;
                flow.TotalBytes += ipPacket.TotalLength;
                if (rawCapture.Timeval.Date > flow.EndTime)
                    flow.EndTime = rawCapture.Timeval.Date;
            }
            catch
            {
                // you can log parse errors if you want
            }
        };

        device.Capture();
        device.Close();

        // assign FlowId
        var list = new List<FlowRecord>(flows.Values);
        for (int i = 0; i < list.Count; i++)
            list[i].FlowId = i + 1;

        return list;
    }

    public void ExportFlowsToCsv(IEnumerable<FlowRecord> flows, string csvPath)
    {
        using var writer = new StreamWriter(csvPath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteHeader<FlowRecord>();
        csv.NextRecord();

        foreach (var flow in flows)
        {
            csv.WriteRecord(flow);
            csv.NextRecord();
        }
    }
}
