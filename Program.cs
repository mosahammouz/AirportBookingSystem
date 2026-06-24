using BookingSystem.Models;
using BookingSystem.Services;

bool running = true;
FlightService flightService = new FlightService();
BookingService bookingService = new BookingService(flightService);
while (running)
{   Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("=== Airport Booking System ===");
    Console.WriteLine("1. Passenger");
    Console.WriteLine("2. Manager");
    Console.WriteLine("3. Exit");

    string choice = Console.ReadLine();

    if (choice == "1")
    {    Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("--- Passenger Menu ---");
        Console.WriteLine("1. Search Flights");
        Console.WriteLine("2. Book Flight");
        Console.WriteLine("3. View My Bookings");
        Console.WriteLine("4. Cancel Booking");
        
        string choicePassenger = Console.ReadLine();
        if (choicePassenger == "1")
{
    string? departureCountry = null;
    string? destinationCountry = null;
    DateTime? departureDate = null;
    string? departureAirport = null;
    string? arrivalAirport = null;
    FlightClass? flightClass = null;
    decimal? maxPrice = null;
    bool exitSearchMenu = false;
    while (!exitSearchMenu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("=== Search Flights ===");
        Console.WriteLine("1. Departure Country");
        Console.WriteLine("2. Destination Country");
        Console.WriteLine("3. Departure Date");
        Console.WriteLine("4. Departure Airport");
        Console.WriteLine("5. Arrival Airport");
        Console.WriteLine("6. Flight Class");
        Console.WriteLine("7. Maximum Price");
        Console.WriteLine("8. Search");
        Console.Write("Choose a filter: ");
        Console.ResetColor();
        string? choiceF = Console.ReadLine();
           
        switch (choiceF)
        {
            case "1":
                Console.Write("Departure Country: ");
                departureCountry = Console.ReadLine();
                break;

            case "2":
                Console.Write("Destination Country: ");
                destinationCountry = Console.ReadLine();
                break;

            case "3":
                Console.Write("Departure Date (yyyy-MM-dd): ");
                string? dateInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(dateInput) &&
                    DateTime.TryParse(dateInput, out DateTime date))
                {
                    departureDate = date;
                }
                break;

            case "4":
                Console.Write("Departure Airport: ");
                departureAirport = Console.ReadLine();
                break;

            case "5":
                Console.Write("Arrival Airport: ");
                arrivalAirport = Console.ReadLine();
                break;

            case "6":
                Console.Write("Flight Class (Economy, Business, FirstClass): ");
                string? classInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(classInput) &&
                    Enum.TryParse(classInput, true, out FlightClass fc))
                {
                    flightClass = fc;
                }
                break;

            case "7":
                Console.Write("Maximum Price: ");
                string? priceInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(priceInput) &&
                    decimal.TryParse(priceInput, out decimal price))
                {
                    maxPrice = price;
                }
                break;

            case "8":
                var results = flightService.Search(
                    departureCountry,
                    destinationCountry,
                    departureDate,
                    departureAirport,
                    arrivalAirport,
                    flightClass,
                    maxPrice);

                Console.Clear();

                if (results.Count == 0)
                {
                    Console.WriteLine("No flights found.");
                }
                else
                {
                    Console.WriteLine($"Found {results.Count} flight(s):\n");

                    foreach (var flight in results)
                    {
                        Console.WriteLine($"Flight ID: {flight.Id}");
                        Console.WriteLine($"From: {flight.DepartureCountry} ({flight.DepartureAirport})");
                        Console.WriteLine($"To: {flight.DestinationCountry} ({flight.ArrivalAirport})");
                        Console.WriteLine($"Date: {flight.DepartureDate:yyyy-MM-dd HH:mm}");
                        Console.WriteLine($"Economy: {flight.EconomyPrice}$");
                        Console.WriteLine($"Business: {flight.BusinessPrice}$");
                        Console.WriteLine($"First Class: {flight.FirstClassPrice}$");
                        Console.WriteLine(new string('-', 40));
                    }
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                exitSearchMenu = true; 
                break;
        }
    }
}
        if (choicePassenger == "2")
        {
            Console.Clear();

            Console.WriteLine("===== Book a Flight =====");
            Console.Write("Enter your name: ");
            string passengerName = Console.ReadLine()!;

            Console.Write("Enter Flight ID: ");
            int flightId = int.Parse(Console.ReadLine()!);

            Console.WriteLine();
            Console.WriteLine("Choose your class:");
            Console.WriteLine("1. Economy");
            Console.WriteLine("2. Business");
            Console.WriteLine("3. First Class");
            Console.Write("Choice: ");

            string choice2 = Console.ReadLine()!;
           
            FlightClass flightClass = choice2 switch
            {
                "1" => FlightClass.Economy,
                "2" => FlightClass.Business,
                "3" => FlightClass.FirstClass,
                _ => throw new Exception("Invalid class selection.")
            };

            bookingService.Book(passengerName, flightId, flightClass);

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        if (choicePassenger == "3")
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine()!;

            List<Booking> bookings =
                bookingService.GetPassengerBookings(name);

            if (bookings.Count == 0)
            {
                Console.WriteLine("No bookings found.");
            }
            else
            {
                Console.WriteLine("\nYour bookings:");

                foreach (var booking in bookings)
                {
                    Console.WriteLine(
                        $"Booking ID: {booking.Id}");
                    Console.WriteLine(
                        $"Flight ID: {booking.FlightId}");
                    Console.WriteLine(
                        $"Class: {booking.Class}");
                    Console.WriteLine(
                        $"Price: {booking.Price}");
                    Console.WriteLine("-------------------");
                }
            }
        }
        if (choicePassenger == "4")
        {   
            Console.WriteLine("please enter your Booking id to cancel it : ");
            string id = Console.ReadLine();
            int idInt = int.Parse(id);
            bookingService.Cancel(idInt);
        }


    }

    if (choice == "2")
    {
        ShowManagerMenu();
    }

    if (choice == "3")
    {
        running = false;
        Console.ResetColor();
    }
}
 void ShowManagerMenu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("=== Manager Menu ===");
        Console.WriteLine("1. Filter Bookings");
        Console.WriteLine("2. Import Flights (CSV)");
        Console.WriteLine("3. Validate Flights Rules");
        Console.WriteLine("4. Back");
        Console.WriteLine();

        Console.Write("Choose: ");
        string? choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.Clear();
            Console.WriteLine("=== Filter Bookings ===");
            Console.WriteLine();
            Console.WriteLine("Filter by:");
            Console.WriteLine("1. Passenger Name");
            Console.WriteLine("2. Flight ID");
            Console.WriteLine("3. Maximum Price");
            Console.WriteLine("4. Show All");
            Console.WriteLine();

            Console.Write("Choose filter type: ");
            string? filterChoice = Console.ReadLine();

            string? passengerName = null;
            int? flightId = null;
            decimal? maxPrice = null;

            if (filterChoice == "1")
            {
                Console.Write("Enter Passenger Name: ");
                passengerName = Console.ReadLine();
            }
            else if (filterChoice == "2")
            {
                Console.Write("Enter Flight ID: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                    flightId = id;
            }
            else if (filterChoice == "3")
            {
                Console.Write("Enter Maximum Price: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal price))
                    maxPrice = price;
            }
            else if (filterChoice == "4")
            {
                Console.WriteLine("Showing all bookings...");
            }
            else
            {
                Console.WriteLine("Invalid choice!");
                Console.ReadKey();
                continue;
            }

            var results = bookingService.FilterBookings(
                passengerName,
                flightId,
                null,
                null,
                null,
                maxPrice
            );

            Console.Clear();

            if (results.Count == 0)
            {
                Console.WriteLine("No bookings found.");
            }
            else
            {
                Console.WriteLine($"Found {results.Count} booking(s):\n");

                foreach (var b in results)
                {
                    Console.WriteLine($"Booking ID: {b.Id}");
                    Console.WriteLine($"Passenger: {b.PassengerName}");
                    Console.WriteLine($"Flight ID: {b.FlightId}");
                    Console.WriteLine($"Class: {b.Class}");
                    Console.WriteLine($"Price: {b.Price}");
                    Console.WriteLine("----------------------------");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        else if (choice == "2")
        {
            Console.Write("Enter file name (example: NewFile.csv): ");
            string fileName = Console.ReadLine();
            string path = Path.Combine("Data", fileName);
            var errors = flightService.ImportWithValidation(path);
            Console.Clear();
            if (errors.Count == 0)
            {
                Console.WriteLine("All flights imported successfully!");
            }
            else
            {
                Console.WriteLine("Import finished with errors:\n");

                foreach (var e in errors)
                {
                    Console.WriteLine($"Row {e.Row}: {e.Error}");
                    Console.WriteLine($"Data: {e.Line}");
                    Console.WriteLine("----------------------");
                }
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
        else if (choice == "3")
        {
            flightService.ShowFlightValidationRules();
            Console.ReadKey();
        }
        else if (choice == "4")
        {
            return; // back to main menu
        }
        else
        {
            Console.WriteLine("Invalid option!");
            Console.ReadKey();
        }
    }
}