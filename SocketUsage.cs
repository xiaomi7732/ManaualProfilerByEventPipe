namespace ManualProfiler;

public class SocketUsage : BackgroundService
{
    private readonly ILogger<SocketUsage> _logger;

    public SocketUsage(ILogger<SocketUsage> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Not best practice of using HttpClient. Use HttpClientFactory is recommended. Or make a singleton of HttpClient.
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("https://www.google.com");
                _logger.LogInformation("Http result content length: {count}", result.Length);

                try
                {

                    await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == stoppingToken)
                {
                    // Canceled by the user.
                }
            }
        }
    }
}