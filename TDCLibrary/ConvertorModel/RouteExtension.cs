using System.ComponentModel;
using System.Text.RegularExpressions;
using TDCLibrary.GtfsModel;
using TDCLibrary.GtsfModel;
using TDCLibrary.JdfModel.Enums;

namespace TDCLibrary.ConvertorModel;

public class RouteExtension
{
    /// <summary>
    /// Extract route id from strings like ZCCCZC or ZCCCCCCZC.
    /// If a 6-digit sequence exists returns that (e.g. "123456").
    /// If only a 3-digit sequence exists returns "010" + those 3 digits (e.g. "010123").
    /// Returns null when no 3- or 6-digit sequence is found.
    /// </summary>
    public static int ToRouteId(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.Error.Write("Route id string is null or empty.");
            return -1;
        }

        return int.Parse(input.Replace("_", ""));
        // // prefer 6-digit sequence
        // var m6 = Regex.Match(input, @"\d{6}");
        // if (m6.Success)
        // {
        //     return int.Parse(m6.Value);
        // }
        //
        // // fallback to 3-digit sequence
        // var m3 = Regex.Match(input, @"\d{3}");
        // if (m3.Success)
        // {
        //     return int.Parse("010" + m3.Value);
        // }

        Console.Error.Write($"Route id string '{input}' does not contain a valid 3- or 6-digit sequence.");
        return -1;
    }
    
    public static int FindRouteIdFromTripId(List<Trip> trips,string tripId)
    {
        var trip = trips.FirstOrDefault(t => t.TripId == tripId);
        
        if (trip == null)
        {
            Console.Error.Write($"Trip with id '{tripId}' not found.");
            return -1;
        }
        
        return ToRouteId(trip.RouteId);
    }

    public static Trip? FindRouteFromTripId(List<Trip> trips, string tripId)
    {
        var trip = trips.FirstOrDefault(t => t.TripId == tripId);
        
        if (trip == null)
        {
            Console.Error.Write($"Trip with id '{tripId}' not found.");
            return null;
        }
        
        return trip;
    }
    
}