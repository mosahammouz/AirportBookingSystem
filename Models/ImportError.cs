namespace BookingSystem.Models;

public class ImportError
{
    public int Row { get; set; }
    public string Line { get; set; }
    public string Error { get; set; }
}