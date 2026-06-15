-- Database schema and stored procedures for Telemetry Processor

CREATE TABLE PendingReadings (
    Id NVARCHAR(50) PRIMARY KEY,
    Timestamp DATETIME2 NOT NULL,
    Value FLOAT NOT NULL,
    SensorType NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE TABLE AnalysisResults (
    Id NVARCHAR(50) PRIMARY KEY,
    SensorReadingId NVARCHAR(50) NOT NULL,
    AnalysisType NVARCHAR(100) NOT NULL,
    Result FLOAT NOT NULL,
    ProcessedAt DATETIME2 DEFAULT GETUTCDATE()
);

-- Stored Procedures
CREATE PROCEDURE sp_SavePendingReading
    @Id NVARCHAR(50),
    @Timestamp DATETIME2,
    @Value FLOAT,
    @SensorType NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO PendingReadings (Id, Timestamp, Value, SensorType)
    VALUES (@Id, @Timestamp, @Value, @SensorType);
END;

CREATE PROCEDURE sp_GetOldestPending
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP 1 * FROM PendingReadings ORDER BY Timestamp ASC;
END;

CREATE PROCEDURE sp_InsertAnalysisResults
    @Id NVARCHAR(50),
    @SensorReadingId NVARCHAR(50),
    @AnalysisType NVARCHAR(100),
    @Result FLOAT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO AnalysisResults (Id, SensorReadingId, AnalysisType, Result)
    VALUES (@Id, @SensorReadingId, @AnalysisType, @Result);
END;
