using BookingSystem.Services;

namespace BookingSystem.Test;

public class FlightServiceTestsImporting : IDisposable
{
 
    private readonly string _testFilePath;

    public FlightServiceTestsImporting()
    {
        _testFilePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "test_flights.csv"
        );
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
    }

    [Fact]
    public void ImportFromCsv_WithValidFile_AddsFlights()
    {
        File.WriteAllText(
            _testFilePath,
            "10,Palestine,Turkey,TLV,IST,2026-07-10,100,200,300"
        );

        var service = new FlightService();

        service.ImportFromCsv(_testFilePath);

        Assert.Contains(
            service.GetAll(),
            f => f.Id == 10
        );
    }
}