namespace BookingSystem.Storage;

public static class Database
{
    public static string BasePath =
        Path.Combine(Directory.GetCurrentDirectory(), "Data");

    public static string FlightsPath =
        Path.Combine(BasePath, "flights.csv");

    public static string BookingsPath =
        Path.Combine(BasePath, "bookings.csv");

    public static string PassengersPath =
        Path.Combine(BasePath, "passengers.csv");
}