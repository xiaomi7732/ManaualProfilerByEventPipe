using System.Diagnostics;
using System.Diagnostics.Tracing;
using Microsoft.Diagnostics.NETCore.Client;
using ServiceProfiler.EventPipe.UserApp30;

namespace ManualProfiler;

public class ProfilerService : BackgroundService
{
    EventPipeSession? _activeSession = null;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int pid = Process.GetCurrentProcess().Id;
        List<EventPipeProvider> providers = new List<EventPipeProvider>{
            new EventPipeProvider(ProfilerTestEventSource.DisplayName, System.Diagnostics.Tracing.EventLevel.Verbose, 0x1, null),
        };

        stoppingToken.Register(() =>
                {
                    if (_activeSession is not null)
                    {
                        Console.WriteLine("Stopping Profiler");
                        _activeSession.Stop();

                        Console.WriteLine("Profiler Stopped.");
                    }
                });


        DiagnosticsClient client = new DiagnosticsClient(pid);

        _activeSession = client.StartEventPipeSession(providers, true, 256);

        using (Stream writeTo = new FileStream("output.nettrace", FileMode.Create))
        {
            await _activeSession.EventStream.CopyToAsync(writeTo);
            await _activeSession.EventStream.FlushAsync();
            _activeSession.Dispose();
        }



        Console.WriteLine("Start profiling");
    }
}