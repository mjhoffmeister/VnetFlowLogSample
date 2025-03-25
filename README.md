# Azure VNet Flow Log Parser Sample

A simple demonstration of parsing Azure Virtual Network flow logs using .NET 8.

## Overview

This sample project shows how to deserialize and work with Virtual Network (VNet) flow logs in a .NET application. It demonstrates:

- Creating a C# object model for VNet flow log JSON
- Using System.Text.Json for efficient deserialization
- Parsing comma-delimited flow tuple strings
- Implementing unit tests

## Project Structure

- **VnetFlowLogSample**: The core object model
  - `VnetFlowLog.cs`: Class definitions for the flow log structure
- **VnetFlowLogSample.UnitTests**: Tests validating the model works correctly

## Flow Log Structure

The sample handles the following flow log structure:
- Records: Collection of flow log entries
  - Flow records: Groups of network flows
    - Flows: Collections of flow groups with the same ACL ID
      - Flow groups: Collections of flows for a specific rule
        - Flow tuples: Individual network flows (connection details)

