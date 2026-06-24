using BookingSystem.Models;
using BookingSystem.Storage;

namespace BookingSystem.Services;

public class FlightService
{
    private List<Flight> _flights;
    public FlightService(){_flights = load();}
    public List<Flight> GetAll(){return _flights;}

    public Flight? GetById(int id){return _flights.FirstOrDefault(f => f.Id == id);} // the power of LINQ
    

    // *******************LOAD***************************
    private List<Flight> load()
    {
        List<Flight> flights = new();
        if (!File.Exists(Database.FlightsPath)) return flights;
        string[] lines = File.ReadAllLines(Database.FlightsPath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            Flight flight = new Flight
            {
                Id = int.Parse(parts[0]),
                DepartureCountry = parts[1],
                DestinationCountry = parts[2],
                DepartureAirport = parts[3],
                ArrivalAirport = parts[4],
                DepartureDate = DateTime.Parse(parts[5]),
                EconomyPrice = decimal.Parse(parts[6]),
                BusinessPrice = decimal.Parse(parts[7]),
                FirstClassPrice = decimal.Parse(parts[8])
            };
            flights.Add(flight);
        }

        return flights;
    }
    
    
    //*******************SAVE***************************
    public void Save()
    {
        List<string> lines = new();

        foreach (var flight in _flights)
        {
            lines.Add(
                $"{flight.Id}," +
                $"{flight.DepartureCountry}," +
                $"{flight.DestinationCountry}," +
                $"{flight.DepartureAirport}," +
                $"{flight.ArrivalAirport}," +
                $"{flight.DepartureDate:yyyy-MM-dd}," +
                $"{flight.EconomyPrice}," +
                $"{flight.BusinessPrice}," +
                $"{flight.FirstClassPrice}");
        }

        File.WriteAllLines(Database.FlightsPath, lines);
    }
    
    //*******************************SEARCH*************************88
        public List<Flight> Search(
        string? departureCountry = null,
        string? destinationCountry = null,
        DateTime? departureDate= null,
        string? departureAirport= null,
        string? arrivalAirport= null,
        FlightClass? flightClass= null,
        decimal? maxPrice= null)
    {
        IEnumerable<Flight> query = _flights;

        if (!string.IsNullOrWhiteSpace(departureCountry))
        {
            query = query.Where(f =>
                f.DepartureCountry.Equals(
                    departureCountry,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(destinationCountry))
        {
            query = query.Where(f =>
                f.DestinationCountry.Equals(
                    destinationCountry,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (departureDate.HasValue)
        {
            query = query.Where(f =>
                f.DepartureDate.Date ==
                departureDate.Value.Date);
        }

        if (!string.IsNullOrWhiteSpace(departureAirport))
        {
            query = query.Where(f =>
                f.DepartureAirport.Equals(
                    departureAirport,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(arrivalAirport))
        {
            query = query.Where(f =>
                f.ArrivalAirport.Equals(
                    arrivalAirport,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (flightClass.HasValue && maxPrice.HasValue)
        {
            query = query.Where(f =>
            {
                decimal price = flightClass.Value switch
                {
                    FlightClass.Economy => f.EconomyPrice,
                    FlightClass.Business => f.BusinessPrice,
                    FlightClass.FirstClass => f.FirstClassPrice,
                    _ => 0
                };

                return price <= maxPrice.Value;
            });
        }

        return query.ToList();
    }
        //***********************ADD RANGE********************
        public void AddRange(List<Flight> flights)
        {
            _flights.AddRange(flights);
            Save();
        }
}