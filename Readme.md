# Manual Profiling using EventPipe

## Profiling
It is very easy to start a profiling session by providing custom providers, see [ProfilerService.cs](./ProfilerService.cs).

## Custom EventSource

Custom event source has some requirements needs to pay attention to.

1. The keyword, when not specified, would be 0xF00000000000 - and would not be covered by 0xffffff, for example.
   1. That might lead to events being filtered out unintentionally.
2. The keywords has to be defined in a subclass of the eventsource before using.
3. There is no need to create a manual EventSource to enable the target EventSource. When turning on profiler, the target event source shall be enabled.

## Counters

### How to enable .NET Counters by EventPipe provider

```csharp
List<EventPipeProvider> providers = new List<EventPipeProvider>{
   ...
   new EventPipeProvider("System.Runtime", EventLevel.Verbose, 0xFFFFFFFFFFFF, new Dictionary<string,string>(){ ["EventCounterIntervalSec"] = "1"}),
   ...
};
```
Quick notes:

* The argument is required to enable counters. No counter otherwise.

### well known counter providers:

| Name                                | EventLevel       | Keywords   | Remark                                                              |
| ----------------------------------- | ---------------- | ---------- | ------------------------------------------------------------------- |
| System.Runtime                      | Verbose(5)       | 0xffffffff | A default set of performance counters provided by the .NET runtime. |
| Microsoft.AspNetCore.Hosting        | Informational(4) | 0x0        | "A set of performance counters provided by ASP.NET Core.            |
| Microsoft-AspNetCore-Server-Kestrel | Informational(4) | 0x0        | A set of performance counters provided by Kestrel.                  |
| System.Net.Http                     | Informational(4) | 0x0        | A set of performance counters for System.Net.Http                   |
| System.Net.NameResolution           | Informational(4) | 0x0        | A set of performance counters for DNS lookups                       |
| System.Net.Security                 | Informational(4) | 0x0        | A set of performance counters for TLS                               |
| System.Net.Sockets                  | Informational(4) | 0x0        | A set of performance counters for System.Net.Sockets                |

Not all the providers / counters are supported on all runtime. Refer to [KnownData.cs](https://github.com/dotnet/diagnostics/blob/main/src/Tools/dotnet-counters/KnownData.cs) for more details.

## Notes 

To myself: because I didn't set the Keyword in the custom event source at the very beginning and used `0xfffff` as keyword parameter in the provider, no event's captured at the beginning.
It worked after either use `0xfffffffffffffff` as the keyword or specify a custom keyword for the event in the EventSource.

