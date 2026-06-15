namespace TelemetryPublisher.Models;

public record SensorReading(
    string Id,
    DateTimeOffset Timestamp,
    double Value,
    string SensorType
);