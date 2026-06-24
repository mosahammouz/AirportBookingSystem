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
        
        //************************ ImportFromCsv ****************
        public void ImportFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("❌ File not found!");
                return;
            }

            var lines = File.ReadAllLines(filePath);

            List<Flight> importedFlights = new();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length != 9)
                {
                    Console.WriteLine($"❌ Invalid row skipped: {line}");
                    continue;
                }

                try
                {
                    var flight = new Flight
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

                    importedFlights.Add(flight);
                }
                catch
                {
                    Console.WriteLine($"❌ Error parsing row: {line}");
                }
            }

            // Add to system
            _flights.AddRange(importedFlights);

            // Save to system file
            Save();

            Console.WriteLine($"✅ Successfully imported {importedFlights.Count} flights!");
        }
        
        // ******************************  validation *****************
        public void ShowFlightValidationRules()
        {
            Console.Clear();
            Console.WriteLine("=== Flight Validation Rules ===");
            Console.WriteLine();

            Console.WriteLine("1. Departure Country:");
            Console.WriteLine("   - Type: Free Text");
            Console.WriteLine("   - Constraint: Required (cannot be empty)");
            Console.WriteLine();

            Console.WriteLine("2. Destination Country:");
            Console.WriteLine("   - Type: Free Text");
            Console.WriteLine("   - Constraint: Required (cannot be empty)");
            Console.WriteLine();

            Console.WriteLine("3. Departure Airport:");
            Console.WriteLine("   - Type: String");
            Console.WriteLine("   - Constraint: Required");
            Console.WriteLine();

            Console.WriteLine("4. Arrival Airport:");
            Console.WriteLine("   - Type: String");
            Console.WriteLine("   - Constraint: Required");
            Console.WriteLine();

            Console.WriteLine("5. Departure Date:");
            Console.WriteLine("   - Type: DateTime");
            Console.WriteLine("   - Constraint: Required");
            Console.WriteLine("   - Rule: Must be Today or in the Future");
            Console.WriteLine();

            Console.WriteLine("6. Economy Price:");
            Console.WriteLine("   - Type: Decimal");
            Console.WriteLine("   - Constraint: Must be > 0");
            Console.WriteLine();

            Console.WriteLine("7. Business Price:");
            Console.WriteLine("   - Type: Decimal");
            Console.WriteLine("   - Constraint: Must be > 0 and > Economy Price");
            Console.WriteLine();

            Console.WriteLine("8. First Class Price:");
            Console.WriteLine("   - Type: Decimal");
            Console.WriteLine("   - Constraint: Must be > Business Price");
            Console.WriteLine();

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
        public List<ImportError> ImportWithValidation(string filePath)
        {
            List<ImportError> errors = new();
            List<Flight> validFlights = new();

            var lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');

                try
                {
                    var flight = new Flight
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

                    // 🔥 VALIDATION RULES

                    if (string.IsNullOrWhiteSpace(flight.DepartureCountry))
                        throw new Exception("Departure country is required");

                    if (flight.DepartureDate < DateTime.Today)
                        throw new Exception("Date must be today or future");

                    if (flight.EconomyPrice <= 0)
                        throw new Exception("Prices must be > 0");

                    validFlights.Add(flight);
                }
                catch (Exception ex)
                {
                    errors.Add(new ImportError
                    {
                        Row = i + 1,
                        Line = lines[i],
                        Error = ex.Message
                    });
                }
            }

            _flights.AddRange(validFlights);
            Save();

            return errors;
        }
}