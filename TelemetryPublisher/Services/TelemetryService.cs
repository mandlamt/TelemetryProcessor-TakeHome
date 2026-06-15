using System.Collections.Concurrent;
using TelemetryPublisher.Models;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;

public class TelemetryService : BackgroundService
{
    private readonly ConcurrentQueue<SensorReading> _queue = new();
    private readonly IConfiguration _config;
    private long _totalGenerated = 0;
    private long _totalSpilled = 0;

    public TelemetryService(IConfiguration config)
    {
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            var reading = GenerateReading();
            _queue.Enqueue(reading);
            Interlocked.Increment(ref _totalGenerated);

            // Spill logic
            if (_queue.Count > 10)
            {
                await SpillOldestToDb();
            }
            // Age check would be on dequeue
        }
    }

    private SensorReading GenerateReading()
    {
        var types = new[] { "Temperature", "Pressure", "Humidity", "Vibration" };
        return new SensorReading(
            "sr_" + Guid.NewGuid().ToString("N").Substring(0, 8),
            DateTimeOffset.UtcNow,
            Random.Shared.NextDouble() * 100,
            types[Random.Shared.Next(types.Length)]
        );
    }

    private async Task SpillOldestToDb()
    {
        if (_queue.TryDequeue(out var reading))
        {
            // Call sp_SavePendingReading using SqlConnection
            // Implementation omitted for brevity - use Dapper or ADO.NET
            Interlocked.Increment(ref _totalSpilled);
        }
    }

    public SensorReading? GetNextReading()
    {
        // Prioritize memory, then DB
        if (_queue.TryDequeue(out var reading)) return reading;
        // Query DB sp_GetOldestPending
        return null;
    }
}