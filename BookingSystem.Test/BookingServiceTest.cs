using BookingSystem.Models;
using BookingSystem.Services;

namespace BookingSystem.Test;

public class BookingServiceTest
{
    [Fact]
    public void Check_GetAll_Bookings()
    {
        FlightService flightService = new FlightService();
        BookingService bookingService = new BookingService(flightService);//Arrange

        List<Booking> bookings = bookingService.GetAll(); //Act

        Assert.IsType<List<Booking>>(bookings); //Assert

    }
    
}