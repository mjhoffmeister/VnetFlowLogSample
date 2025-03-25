using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VnetFlowLogSample;

public class VnetFlowLog
{
    [JsonPropertyName("records")]
    public List<FlowLogRecord> Records { get; set; } = new();

    public IEnumerable<FlowTuple> GetFlowTuples()
    {
        return Records
            .Select(r => r.FlowRecords)
            .SelectMany(fr => fr.Flows)
            .SelectMany(f => f.FlowGroups)
            .SelectMany(fg => fg.FlowTuples)
            .Select(t => FlowTuple.TryCreate(t, out var flowTuple) ? flowTuple : null)
            .Where(flowTuple => flowTuple != null)!;
    }
}

public class FlowLogRecord
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("flowLogVersion")]
    public int FlowLogVersion { get; set; }

    [JsonPropertyName("flowLogGUID")]
    public string FlowLogGUID { get; set; } = string.Empty;

    [JsonPropertyName("macAddress")]
    public string MacAddress { get; set; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;

    [JsonPropertyName("flowLogResourceID")]
    public string FlowLogResourceID { get; set; } = string.Empty;

    [JsonPropertyName("targetResourceID")]
    public string TargetResourceID { get; set; } = string.Empty;

    [JsonPropertyName("operationName")]
    public string OperationName { get; set; } = string.Empty;

    [JsonPropertyName("flowRecords")]
    public FlowRecords FlowRecords { get; set; } = new();
}

public class FlowRecords
{
    [JsonPropertyName("flows")]
    public List<Flow> Flows { get; set; } = new();
}

public class Flow
{
    [JsonPropertyName("aclID")]
    public string AclId { get; set; } = string.Empty;

    [JsonPropertyName("flowGroups")]
    public List<FlowGroup> FlowGroups { get; set; } = new();
}

public class FlowGroup
{
    [JsonPropertyName("rule")]
    public string Rule { get; set; } = string.Empty;

    [JsonPropertyName("flowTuples")]
    public List<string> FlowTuples { get; set; } = new();
}

public class FlowTuple
{
    public long UnixTimestamp { get; }
    public string SourceIpAddress { get; }
    public string DestinationIpAddress { get; }
    public int SourcePort { get; }
    public int DestinationPort { get; }
    public int IanaProtocolNumber { get; }
    public string TrafficFlow { get; }
    public string FlowState { get; }
    public string FlowEncryptionStatus { get; }
    public long SourceToDestinationPacketCount { get; }
    public long SourceToDestinationBytes { get; }
    public long DestinationToSourcePacketCount { get; }
    public long DestinationToSourceBytes { get; }

    // Private constructor to force usage of TryCreate
    private FlowTuple(
        long unixTimestamp,
        string sourceIpAddress,
        string destinationIpAddress,
        int sourcePort,
        int destinationPort,
        int ianaProtocolNumber,
        string trafficFlow,
        string flowState,
        string flowEncryptionStatus,
        long sourceToDestinationPacketCount,
        long sourceToDestinationBytes,
        long destinationToSourcePacketCount,
        long destinationToSourceBytes)
    {
        UnixTimestamp = unixTimestamp;
        SourceIpAddress = sourceIpAddress;
        DestinationIpAddress = destinationIpAddress;
        SourcePort = sourcePort;
        DestinationPort = destinationPort;
        IanaProtocolNumber = ianaProtocolNumber;
        TrafficFlow = trafficFlow;
        FlowState = flowState;
        FlowEncryptionStatus = flowEncryptionStatus;
        SourceToDestinationPacketCount = sourceToDestinationPacketCount;
        SourceToDestinationBytes = sourceToDestinationBytes;
        DestinationToSourcePacketCount = destinationToSourcePacketCount;
        DestinationToSourceBytes = destinationToSourceBytes;
    }

    public static bool TryCreate(
        string flowTupleString, out FlowTuple? flowTuple)
    {
        flowTuple = null;

        if (string.IsNullOrEmpty(flowTupleString))
        {
            return false;
        }

        string[] parts = flowTupleString.Split(',');
        if (parts.Length != 13)
        {
            return false;
        }

        try
        {
            if (!long.TryParse(parts[0], out long unixTimestamp) ||
                !int.TryParse(parts[3], out int sourcePort) ||
                !int.TryParse(parts[4], out int destinationPort) ||
                !int.TryParse(parts[5], out int ianaProtocolNumber) ||
                !long.TryParse(parts[9], out long sourceToDestinationPacketCount) ||
                !long.TryParse(parts[10], out long sourceToDestinationBytes) ||
                !long.TryParse(parts[11], out long destinationToSourcePacketCount) ||
                !long.TryParse(parts[12], out long destinationToSourceBytes))
            {
                return false;
            }

            flowTuple = new FlowTuple(
                unixTimestamp,
                parts[1],
                parts[2],
                sourcePort,
                destinationPort,
                ianaProtocolNumber,
                parts[6],
                parts[7],
                parts[8],
                sourceToDestinationPacketCount,
                sourceToDestinationBytes,
                destinationToSourcePacketCount,
                destinationToSourceBytes);

            return true;
        }
        catch
        {
            return false;
        }
    }
}