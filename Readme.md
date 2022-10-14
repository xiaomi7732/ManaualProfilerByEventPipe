# Manual Profiling using EventPipe

## Profiling
It is very easy to start a profiling session by providing custom providers, see [ProfilerService.cs](./ProfilerService.cs).

## Custom EventSource

Custom event source has some requirements needs to pay attention to.

1. The keyword, when not specified, would be 0 - and would not be covered by 0xffffff, for example.
   1. Interestingly, it looks like: 0xfffffffffffffff would cover it. Why?
2. The keywords has to be defined in a subclass of the eventsource before using.
3. There is no need to create a manual EventSource to enable the target EventSource. When turning on profiler, the target event source shall be enabled.

