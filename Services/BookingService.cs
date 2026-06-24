using BookingSystem.Models;
using BookingSystem.Storage;

namespace BookingSystem.Services;

public class BookingService
{
    private List<Booking> _bookings;
    private FlightService _flightService;
    public BookingService(FlightService flightService){_flightService = flightService; _bookings = load();}
    public List<Booking> GetAll(){return _bookings;}
    
    //**************************LOAD*********************
    public List<Booking> load()
    {
        List<Booking> bookings = new();
        if (!File.Exists(Database.BookingsPath)) return bookings;
        string[] lines = File.ReadAllLines(Database.BookingsPath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(",");
            Booking booking = new Booking
            {
                Id = int.Parse(parts[0]),
                PassengerName = parts[1],
                FlightId = int.Parse(parts[2]),
                Class = Enum.Parse<FlightClass>(parts[3]),
                Price = decimal.Parse(parts[4])
            };

            bookings.Add(booking);


        }

        return bookings;
    }
    
    // ******************** SAVE TO CSV ********************
    public void Save()
    {
        List<string> lines = new();

        foreach (var b in _bookings)
        {
            lines.Add(
                $"{b.Id}," +
                $"{b.PassengerName}," +
                $"{b.FlightId}," +
                $"{b.Class}," +
                $"{b.Price}");
        }

        File.WriteAllLines(Database.BookingsPath, lines);
    }
   // ************************** BOOK ****************
   public void Book(string passengerName, int flightId, FlightClass flightClass)
   {
       var flight = _flightService.GetById(flightId);

       if (flight == null)
       {
           Console.WriteLine("Flight not found.");
           return;
       }

       decimal price = flightClass switch
       {
           FlightClass.Economy => flight.EconomyPrice,
           FlightClass.Business => flight.BusinessPrice,
           FlightClass.FirstClass => flight.FirstClassPrice,
           _ => 0
       };

       int newId = _bookings.Count > 0
           ? _bookings.Max(b => b.Id) + 1
           : 1;

       Booking booking = new Booking
       {
           Id = newId,
           PassengerName = passengerName,
           FlightId = flightId,
           Class = flightClass,
           Price = price
       };

       _bookings.Add(booking);
       Save();

       Console.WriteLine("Booking created successfully.");
   }
   
   //******************* cancel a flight booking *******************
   public void Cancel(int bookingId)
   {
       var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);

       if (booking == null)
       {
           Console.WriteLine("Booking not found.");
           return;
       }

       _bookings.Remove(booking);
       Save();

       Console.WriteLine("Booking cancelled.");
   }
 
   // ***********************  PASSENGER BOOKINGS **********
   public List<Booking> GetPassengerBookings(string name)
   {
       return _bookings
           .Where(b => b.PassengerName
               .Equals(name, StringComparison.OrdinalIgnoreCase))
           .ToList();
   }
}