namespace ServiceProfiler.EventPipe.UserApp30
{
    public class HeartBeatService : BackgroundService
    {
        private readonly ILogger<HeartBeatService> _logger;

        public HeartBeatService(ILogger<HeartBeatService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Sending heart beat.");
                // Signal heart beat over the test event source.
                ProfilerTestEventSource.Log.HeartBeat("HeartBeat");
                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }
    }
}
