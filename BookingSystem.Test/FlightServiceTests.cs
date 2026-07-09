using BookingSystem.Models;
using BookingSystem.Services;

namespace BookingSystem.Test;

public class BookingServiceTests
{
    
    [Fact]
    public void GetAll_WhenBookingsExist_ReturnsBookings()
    {
        
        var flightService = new FlightService();  // Arrange
        var bookingService = new BookingService(flightService);

       var bookings = bookingService.GetAll();

       
        //
         Assert.Equal(6, bookings[3].Id);
         Assert.True(
             string.Equals(
                 "Mousa",
                 bookings[0].PassengerName,
                 StringComparison.OrdinalIgnoreCase
             )
         );
        //  Assert.Equal(10, bookings[5].FlightId);
         //Assert.Equal(FlightClass.FirstClass, bookings[3].Class);
         //Assert.Equal(350, bookings[3].Price);
    }


    
}