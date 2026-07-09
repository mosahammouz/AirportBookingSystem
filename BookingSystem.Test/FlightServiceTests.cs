using BookingSystem.Models;
using BookingSystem.Services;

namespace BookingSystem.Test;

public class FlightServiceTest
{
    [Fact]
    public void Check_DepartureCountry_for_Id()
    {
        var flightService = new FlightService(); //Arrange
        var flight = flightService.GetById(5); //Act//Execute
        Assert.Equal("UAE", flight?.DepartureCountry); //Assert

    }

    [Fact]
    public void Check_GetAll_Flights()
    {
        var flightService = new FlightService(); //Arrange
        List<Flight> flights = flightService.GetAll(); //Act
        Assert.IsType<List<Flight>>(flights);

    }

    [Fact]
    public void Search_ByArrivalAirport_ReturnsMatchingFlights()
    {
        var service = new FlightService(); // Arrange
        var result = service.Search(
            arrivalAirport: "FCO" //Act
        );
        Console.WriteLine("the count here :");
        Console.WriteLine(result.Count);
        //Assert.Single(result);
        Assert.Equal("France", result[1].DepartureCountry);
    }

    [Fact]
    public void Search_ByFlightClass_ReturnMatchingFlights()
    {
        var service = new FlightService(); // Arrange
        var result = service.Search(
            flightClass: FlightClass.Business //Act
        );
        Assert.Equal(200, result[0].BusinessPrice); //Assert
    }
}