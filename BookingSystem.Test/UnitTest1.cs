using BookingSystem.Services;

namespace BookingSystem.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var flightService = new FlightService(); //Arrange
        var flight = flightService.GetById(5);//Act//Execute
        Assert.Equal("UAE",flight?.DepartureCountry);//Assert

    }

    [Fact]
    public void Test2()
    {
    }
}