namespace BookingSystem.Storage;

public static class Database
{
    public static string BasePath
    {
        get
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory != null && directory.Name != "BookingSystem")
            {
                directory = directory.Parent;
            }

            return Path.Combine(directory!.FullName, "Data");
        }
    }

    public static string FlightsPath =
        Path.Combine(BasePath, "flights.csv");

    public static string BookingsPath =
        Path.Combine(BasePath, "bookings.csv");

    public static string PassengersPath =
        Path.Combine(BasePath, "passengers.csv");
}