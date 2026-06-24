using BookingSystem.Models;
using BookingSystem.Services;

bool running = true;
FlightService flightService = new FlightService();
BookingService bookingService = new BookingService(flightService);
while (running)
{
    Console.WriteLine("=== Airport Booking System ===");
    Console.WriteLine("1. Passenger");
    Console.WriteLine("2. Manager");
    Console.WriteLine("3. Exit");

    string choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.WriteLine("--- Passenger Menu ---");
        Console.WriteLine("1. Search Flights");
        Console.WriteLine("2. Book Flight");
        Console.WriteLine("3. View My Bookings");
        Console.WriteLine("4. Cancel Booking");
        Console.WriteLine("5. Modify a book");
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
        
    }

    if (choice == "3")
    {
        running = false;
    }
}