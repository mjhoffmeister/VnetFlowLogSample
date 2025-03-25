using System;
using System.Text.Json;
using System.Linq;
using Xunit;

namespace VnetFlowLogSample.UnitTests;

public class VnetFlowLogTests
{
    private readonly string _sampleJson = @"{
        ""records"": [
            {
                ""time"": ""2022-09-14T09:00:52.5625085Z"",
                ""flowLogVersion"": 4,
                ""flowLogGUID"": ""66aa66aa-bb77-cc88-dd99-00ee00ee00ee"",
                ""macAddress"": ""112233445566"",
                ""category"": ""FlowLogFlowEvent"",
                ""flowLogResourceID"": ""/SUBSCRIPTIONS/aaaa0a0a-bb1b-cc2c-dd3d-eeeeee4e4e4e/RESOURCEGROUPS/NETWORKWATCHERRG/PROVIDERS/MICROSOFT.NETWORK/NETWORKWATCHERS/NETWORKWATCHER_EASTUS2EUAP/FLOWLOGS/VNETFLOWLOG"",
                ""targetResourceID"": ""/subscriptions/aaaa0a0a-bb1b-cc2c-dd3d-eeeeee4e4e4e/resourceGroups/myResourceGroup/providers/Microsoft.Network/virtualNetworks/myVNet"",
                ""operationName"": ""FlowLogFlowEvent"",
                ""flowRecords"": {
                    ""flows"": [
                        {
                            ""aclID"": ""00aa00aa-bb11-cc22-dd33-44ee44ee44ee"",
                            ""flowGroups"": [
                                {
                                    ""rule"": ""DefaultRule_AllowInternetOutBound"",
                                    ""flowTuples"": [
                                        ""1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B,NX,0,0,0,0"",
                                        ""1663146003606,10.0.0.6,192.0.2.180,23956,443,6,O,E,NX,3,767,2,1580""
                                    ]
                                }
                            ]
                        },
                        {
                            ""aclID"": ""00aa00aa-bb11-cc22-dd33-44ee44ee44ee"",
                            ""flowGroups"": [
                                {
                                    ""rule"": ""BlockHighRiskTCPPortsFromInternet"",
                                    ""flowTuples"": [
                                        ""1663145998065,101.33.218.153,10.0.0.6,55188,22,6,I,D,NX,0,0,0,0""
                                    ]
                                },
                                {
                                    ""rule"": ""Internet"",
                                    ""flowTuples"": [
                                        ""1663145989563,192.0.2.10,10.0.0.6,50557,44357,6,I,D,NX,0,0,0,0"",
                                        ""1663145989679,203.0.113.81,10.0.0.6,62797,35945,6,I,D,NX,0,0,0,0""
                                    ]
                                }
                            ]
                        }
                    ]
                }
            }
        ]
    }";

    private VnetFlowLog DeserializeVnetFlowLog()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<VnetFlowLog>(_sampleJson, options)!;
    }

    [Fact]
    public void Deserialize_VnetFlowLog_ShouldNotBeNull()
    {
        // Act
        var vnetFlowLog = DeserializeVnetFlowLog();

        // Assert
        Assert.NotNull(vnetFlowLog);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_ShouldHaveSingleRecord()
    {
        // Act
        var vnetFlowLog = DeserializeVnetFlowLog();

        // Assert
        Assert.Single(vnetFlowLog.Records);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_RecordTimeShouldMatchExpected()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.Equal(new DateTime(2022, 9, 14, 9, 0, 52, 562, DateTimeKind.Utc).AddTicks(5085), record.Time);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_RecordVersionShouldBeFour()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.Equal(4, record.FlowLogVersion);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_RecordGuidShouldMatchExpected()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.Equal("66aa66aa-bb77-cc88-dd99-00ee00ee00ee", record.FlowLogGUID);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_MacAddressShouldMatchExpected()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.Equal("112233445566", record.MacAddress);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_CategoryShouldBeFlowLogFlowEvent()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.Equal("FlowLogFlowEvent", record.Category);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_ResourceIdShouldStartWithExpectedPrefix()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.StartsWith("/SUBSCRIPTIONS/aaaa0a0a-bb1b-cc2c-dd3d-eeeeee4e4e4e", record.FlowLogResourceID);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_TargetResourceIdShouldStartWithExpectedPrefix()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.StartsWith("/subscriptions/aaaa0a0a-bb1b-cc2c-dd3d-eeeeee4e4e4e", record.TargetResourceID);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_OperationNameShouldBeFlowLogFlowEvent()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.Equal("FlowLogFlowEvent", record.OperationName);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_FlowRecordsShouldNotBeNull()
    {
        // Act
        var record = DeserializeVnetFlowLog().Records[0];

        // Assert
        Assert.NotNull(record.FlowRecords);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_FlowRecordsShouldHaveTwoFlows()
    {
        // Act
        var flowRecords = DeserializeVnetFlowLog().Records[0].FlowRecords;

        // Assert
        Assert.Equal(2, flowRecords.Flows.Count);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_FirstFlowShouldHaveExpectedAclId()
    {
        // Act
        var flow = DeserializeVnetFlowLog().Records[0].FlowRecords.Flows[0];

        // Assert
        Assert.Equal("00aa00aa-bb11-cc22-dd33-44ee44ee44ee", flow.AclId);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_FirstFlowShouldHaveSingleFlowGroup()
    {
        // Act
        var flow = DeserializeVnetFlowLog().Records[0].FlowRecords.Flows[0];

        // Assert
        Assert.Single(flow.FlowGroups);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_FirstFlowGroupShouldHaveExpectedRule()
    {
        // Act
        var flowGroup = DeserializeVnetFlowLog().Records[0].FlowRecords.Flows[0].FlowGroups[0];

        // Assert
        Assert.Equal("DefaultRule_AllowInternetOutBound", flowGroup.Rule);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_FirstFlowGroupShouldHaveTwoFlowTuples()
    {
        // Act
        var flowGroup = DeserializeVnetFlowLog().Records[0].FlowRecords.Flows[0].FlowGroups[0];

        // Assert
        Assert.Equal(2, flowGroup.FlowTuples.Count);
    }

    [Fact]
    public void Deserialize_VnetFlowLog_SecondFlowShouldHaveTwoFlowGroups()
    {
        // Act
        var flow = DeserializeVnetFlowLog().Records[0].FlowRecords.Flows[1];

        // Assert
        Assert.Equal(2, flow.FlowGroups.Count);
    }

    [Fact]
    public void FlowTuple_TryCreate_ReturnsTrue_ForValidInput()
    {
        // Arrange
        string tupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B,NX,0,0,0,0";

        // Act
        bool result = FlowTuple.TryCreate(tupleString, out _);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void FlowTuple_TryCreate_CreatesNonNullInstance_ForValidInput()
    {
        // Arrange
        string tupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B,NX,0,0,0,0";

        // Act
        FlowTuple.TryCreate(tupleString, out var flowTuple);

        // Assert
        Assert.NotNull(flowTuple);
    }

    [Fact]
    public void FlowTuple_TryCreate_ParsesUnixTimestamp_Correctly()
    {
        // Arrange
        string tupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B,NX,0,0,0,0";

        // Act
        FlowTuple.TryCreate(tupleString, out var flowTuple);

        // Assert
        Assert.Equal(1663146003599, flowTuple!.UnixTimestamp);
    }

    [Fact]
    public void FlowTuple_TryCreate_ParsesSourceIpAddress_Correctly()
    {
        // Arrange
        string tupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B,NX,0,0,0,0";

        // Act
        FlowTuple.TryCreate(tupleString, out var flowTuple);

        // Assert
        Assert.Equal("10.0.0.6", flowTuple!.SourceIpAddress);
    }

    [Fact]
    public void FlowTuple_TryCreate_ParsesDestinationIpAddress_Correctly()
    {
        // Arrange
        string tupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B,NX,0,0,0,0";

        // Act
        FlowTuple.TryCreate(tupleString, out var flowTuple);

        // Assert
        Assert.Equal("192.0.2.180", flowTuple!.DestinationIpAddress);
    }

    [Fact]
    public void FlowTuple_TryCreate_ParsesSourcePort_Correctly()
    {
        // Arrange
        string tupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B,NX,0,0,0,0";

        // Act
        FlowTuple.TryCreate(tupleString, out var flowTuple);

        // Assert
        Assert.Equal(23956, flowTuple!.SourcePort);
    }

    [Fact]
    public void FlowTuple_TryCreate_ParsesDestinationPort_Correctly()
    {
        // Arrange
        string tupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B,NX,0,0,0,0";

        // Act
        FlowTuple.TryCreate(tupleString, out var flowTuple);

        // Assert
        Assert.Equal(443, flowTuple!.DestinationPort);
    }

    [Fact]
    public void FlowTuple_TryCreate_ReturnsFalse_ForInvalidFormat()
    {
        // Arrange
        string invalidTupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B";

        // Act
        bool result = FlowTuple.TryCreate(invalidTupleString, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void FlowTuple_TryCreate_ReturnsNullInstance_ForInvalidFormat()
    {
        // Arrange
        string invalidTupleString = "1663146003599,10.0.0.6,192.0.2.180,23956,443,6,O,B";

        // Act
        FlowTuple.TryCreate(invalidTupleString, out var flowTuple);

        // Assert
        Assert.Null(flowTuple);
    }

    [Fact]
    public void FlowTuple_TryCreate_ReturnsFalse_ForEmptyString()
    {
        // Arrange
        string emptyString = "";

        // Act
        bool result = FlowTuple.TryCreate(emptyString, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void FlowTuple_TryCreate_ReturnsFalse_ForNullString()
    {
        // Arrange
        string? nullString = null;

        // Act
        bool result = FlowTuple.TryCreate(nullString!, out _);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetFlowTuples_HasFlowTuples_ReturnsAllValidFlowTuples()
    {
        // Arrange
        var vnetFlowLog = DeserializeVnetFlowLog();

        // Expected count calculation:
        // From the sample JSON:
        // - First flow has 1 flow group with 2 flow tuples
        // - Second flow has 2 flow groups with 1 and 2 flow tuples respectively
        // Total expected: 2 + 1 + 2 = 5 flow tuples
        int expectedTupleCount = 5;

        // Act
        var flowTuples = vnetFlowLog.GetFlowTuples().ToList();

        // Assert
        Assert.Equal(expectedTupleCount, flowTuples.Count);
    }
}
