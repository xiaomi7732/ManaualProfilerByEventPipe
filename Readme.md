# Manual Profiling using EventPipe

## Profiling
It is very easy to start a profiling session by providing custom providers, see [ProfilerService.cs](./ProfilerService.cs).

## Custom EventSource

Custom event source has some requirements needs to pay attention to.

1. The keyword, when not specified, would be 0xF00000000000 - and would not be covered by 0xffffff, for example.
   1. That might lead to events being filtered out unintentionally.
2. The keywords has to be defined in a subclass of the eventsource before using.
3. There is no need to create a manual EventSource to enable the target EventSource. When turning on profiler, the target event source shall be enabled.

## Notes 

To myself: because I didn't set the Keyword in the custom event source at the very beginning and used `0xfffff` as keyword parameter in the provider, no event's captured at the beginning.
It worked after either use `0xfffffffffffffff` as the keyword or specify a custom keyword for the event in the EventSource.

