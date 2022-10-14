using System.Diagnostics;
using System.Diagnostics.Tracing;
using Microsoft.Diagnostics.NETCore.Client;
using ServiceProfiler.EventPipe.UserApp30;

namespace ManualProfiler;

public class ProfilerService : BackgroundService
{
    private readonly ILogger _logger;
    private const string _outputFileName = "output.nettrace";
    public ProfilerService(ILogger<ProfilerService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int pid = Process.GetCurrentProcess().Id;
        List<EventPipeProvider> providers = new List<EventPipeProvider>{
            
            // Custom EventPipe Providers
            new EventPipeProvider(ProfilerTestEventSource.DisplayName, System.Diagnostics.Tracing.EventLevel.Verbose, 0x1, null),
            
            // Counters
            new EventPipeProvider("System.Runtime", EventLevel.Verbose, 0xFFFFFFFFFFFF, new Dictionary<string,string>(){ ["EventCounterIntervalSec"] = "1"}),
            new EventPipeProvider("System.Net.Sockets", EventLevel.Informational, 0x0, new Dictionary<string,string>(){ ["EventCounterIntervalSec"] = "1"}),
        };

        DiagnosticsClient client = new DiagnosticsClient(pid);
        using EventPipeSession session = client.StartEventPipeSession(providers, true, 256);
        stoppingToken.Register(() =>
        {
            if (session is not null)
            {
                _logger.LogInformation("Stopping Profiler");
                session.Stop();
                _logger.LogInformation("Profiler Stopped. Trace file: {outputFile}", _outputFileName);
            }
        });

        _logger.LogInformation("Start profiling. Writing to: {outputFile}", _outputFileName);
        using (Stream writeTo = new FileStream(_outputFileName, FileMode.Create))
        {
            await session.EventStream.CopyToAsync(writeTo);
        }
    }
}