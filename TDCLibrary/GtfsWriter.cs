using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TDCLibrary.GtsfModel;
using TDCLibrary.GtsfModel.Enums;

namespace TDCLibrary;

/// <summary>
/// Zapisuje GTFS dáta späť do CSV súborov v zadanom adresári.
/// Zapisuje len základné súbory (agency, routes, stops, trips, stop_times). Ostatné možno doplniť neskôr.
/// </summary>
public class GtfsWriter
{
    private readonly string _directory;
    public GtfsWriter(string directory)
    {
        _directory = directory ?? throw new ArgumentNullException(nameof(directory));
        if (!Directory.Exists(_directory)) throw new DirectoryNotFoundException(_directory);
    }

    public void WriteAll(GtfsData data)
    {
        WriteAgencies(data.Agencies);
        WriteRoutes(data.Routes);
        WriteStops(data.Stops);
        WriteTrips(data.Trips);
        WriteStopTimes(data.StopTimes);
    }

    private void WriteCsv<T>(string fileName, IEnumerable<T> records, Action<CsvWriter,T> map, string[] header)
    {
        var path = Path.Combine(_directory, fileName);
        using var writer = new StreamWriter(path, false, System.Text.Encoding.UTF8);
        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            ShouldQuote = args => true
        };
        using var csv = new CsvWriter(writer, cfg);
        foreach (var h in header) csv.WriteField(h);
        csv.NextRecord();
        foreach (var r in records)
        {
            map(csv, r);
            csv.NextRecord();
        }
    }

    private void WriteAgencies(List<Agency> agencies)
    {
        WriteCsv("agency.txt", agencies, (csv,a) =>
        {
            csv.WriteField(a.AgencyId);
            csv.WriteField(a.AgencyName);
            csv.WriteField(a.AgencyUrl);
            csv.WriteField(a.AgencyTimezone);
            csv.WriteField(a.AgencyLang);
            csv.WriteField(a.AgencyPhone);
            csv.WriteField(a.AgencyFareUrl);
            csv.WriteField(a.AgencyEmail);
            csv.WriteField(a.CemvSupport.HasValue ? ((int)a.CemvSupport.Value).ToString() : "");
        }, new[]{"agency_id","agency_name","agency_url","agency_timezone","agency_lang","agency_phone","agency_fare_url","agency_email","cemv_support"});
    }

    private void WriteRoutes(List<Route> routes)
    {
        WriteCsv("routes.txt", routes, (csv,r) =>
        {
            csv.WriteField(r.RouteId);
            csv.WriteField(r.AgencyId);
            csv.WriteField(r.NetworkId);
            csv.WriteField(r.RouteShortName);
            csv.WriteField(r.RouteLongName);
            csv.WriteField(r.RouteDesc);
            csv.WriteField(((int)r.RouteType).ToString());
            csv.WriteField(r.RouteUrl);
            csv.WriteField(r.RouteColor);
            csv.WriteField(r.RouteTextColor);
            csv.WriteField(r.RouteSortOrder?.ToString());
            csv.WriteField(r.ContinuousPickup.HasValue? ((int)r.ContinuousPickup.Value).ToString(): "");
            csv.WriteField(r.ContinuousDropOff.HasValue? ((int)r.ContinuousDropOff.Value).ToString(): "");
            csv.WriteField(r.CemvSupport.HasValue ? ((int)r.CemvSupport.Value).ToString() : "");
        }, new[]{"route_id","agency_id","network_id","route_short_name","route_long_name","route_desc","route_type","route_url","route_color","route_text_color","route_sort_order","continuous_pickup","continuous_drop_off","cemv_support"});
    }

    private void WriteStops(List<Stop> stops)
    {
        WriteCsv("stops.txt", stops, (csv,s) =>
        {
            csv.WriteField(s.StopId);
            csv.WriteField(s.StopCode);
            csv.WriteField(s.StopName);
            csv.WriteField(s.TtsStopName);
            csv.WriteField(s.StopDesc);
            csv.WriteField(s.StopLat?.ToString(CultureInfo.InvariantCulture));
            csv.WriteField(s.StopLon?.ToString(CultureInfo.InvariantCulture));
            csv.WriteField(s.ZoneId);
            csv.WriteField(s.StopUrl);
            csv.WriteField(s.LocationType.HasValue ? ((int)s.LocationType.Value).ToString() : "");
            csv.WriteField(s.ParentStation);
            csv.WriteField(s.StopTimezone);
            csv.WriteField(s.WheelchairBoarding.HasValue ? ((int)s.WheelchairBoarding.Value).ToString() : "");
            csv.WriteField(s.LevelId);
            csv.WriteField(s.PlatformCode);
        }, new[]{"stop_id","stop_code","stop_name","tts_stop_name","stop_desc","stop_lat","stop_lon","zone_id","stop_url","location_type","parent_station","stop_timezone","wheelchair_boarding","level_id","platform_code"});
    }

    private void WriteTrips(List<Trip> trips)
    {
        WriteCsv("trips.txt", trips, (csv,t) =>
        {
            csv.WriteField(t.RouteId);
            csv.WriteField(t.ServiceId);
            csv.WriteField(t.TripId);
            csv.WriteField(t.TripHeadsign);
            csv.WriteField(t.TripShortName);
            csv.WriteField(t.DirectionId.HasValue? ((int)t.DirectionId.Value).ToString(): "");
            csv.WriteField(t.BlockId);
            csv.WriteField(t.ShapeId);
            csv.WriteField(t.WheelchairAccessible.HasValue ? ((int)t.WheelchairAccessible.Value).ToString() : "");
            csv.WriteField(t.BikesAllowed.HasValue ? ((int)t.BikesAllowed.Value).ToString() : "");
            csv.WriteField(t.CarsAllowed.HasValue ? ((int)t.CarsAllowed.Value).ToString() : "");
        }, new[]{"route_id","service_id","trip_id","trip_headsign","trip_short_name","direction_id","block_id","shape_id","wheelchair_accessible","bikes_allowed","cars_allowed"});
    }

    private void WriteStopTimes(List<StopTime> stopTimes)
    {
        WriteCsv("stop_times.txt", stopTimes, (csv,st) =>
        {
            csv.WriteField(st.TripId);
            csv.WriteField(st.ArrivalTime);
            csv.WriteField(st.DepartureTime);
            csv.WriteField(st.StopId);
            csv.WriteField(st.StopSequence.ToString());
            csv.WriteField(st.StopHeadsign);
            csv.WriteField(st.PickupType.HasValue? ((int)st.PickupType.Value).ToString(): "");
            csv.WriteField(st.DropOffType.HasValue? ((int)st.DropOffType.Value).ToString(): "");
            csv.WriteField(st.ContinuousPickup.HasValue? ((int)st.ContinuousPickup.Value).ToString(): "");
            csv.WriteField(st.ContinuousDropOff.HasValue? ((int)st.ContinuousDropOff.Value).ToString(): "");
            csv.WriteField(st.ShapeDistTraveled?.ToString(CultureInfo.InvariantCulture));
            csv.WriteField(st.Timepoint.HasValue? ((int)st.Timepoint.Value).ToString(): "");
        }, new[]{"trip_id","arrival_time","departure_time","stop_id","stop_sequence","stop_headsign","pickup_type","drop_off_type","continuous_pickup","continuous_drop_off","shape_dist_traveled","timepoint"});
    }
}