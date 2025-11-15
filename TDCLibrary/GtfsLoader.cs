using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TDCLibrary.GtsfModel;
using TDCLibrary.GtsfModel.Enums;

namespace TDCLibrary;

/// <summary>
/// Načítavač GTFS dát zo súborov.
/// Podporuje všetky štandardné GTFS súbory podľa špecifikácie https://gtfs.org/documentation/schedule/reference/
/// </summary>
public class GtfsLoader
{
    private readonly string _gtfsDirectoryPath;
    
    /// <summary>
    /// Inicializuje nový načítavač GTFS dát.
    /// </summary>
    /// <param name="gtfsDirectoryPath">Cesta k adresáru obsahujúcemu GTFS súbory</param>
    public GtfsLoader(string gtfsDirectoryPath)
    {
        _gtfsDirectoryPath = gtfsDirectoryPath ?? throw new ArgumentNullException(nameof(gtfsDirectoryPath));
        
        if (!Directory.Exists(gtfsDirectoryPath))
        {
            throw new DirectoryNotFoundException($"GTFS adresár nebol najdený: {gtfsDirectoryPath}");
        }
    }
    
    /// <summary>
    /// Načíta všetky dostupné GTFS dáta z adresára.
    /// Načíta len súbory, ktoré existujú - voliteľné súbory nemusia byť prítomné.
    /// </summary>
    /// <returns>Objekt obsahujúci všetky načítané GTFS dáta</returns>
    public GtfsData LoadAll()
    {
        var data = new GtfsData();
        
        // Povinné súbory
        data.Agencies = LoadAgencies();
        data.Stops = LoadStops();
        data.Routes = LoadRoutes();
        data.Trips = LoadTrips();
        data.StopTimes = LoadStopTimes();
        
        // Podmienečne povinné súbory (aspoň jeden musí existovať)
        data.Calendars = LoadCalendars();
        data.CalendarDates = LoadCalendarDates();
        
        // Voliteľné súbory
        data.FareAttributes = LoadFareAttributes();
        data.FareRules = LoadFareRules();
        data.Shapes = LoadShapes();
        data.Frequencies = LoadFrequencies();
        data.Transfers = LoadTransfers();
        data.Pathways = LoadPathways();
        data.Levels = LoadLevels();
        data.FeedInfo = LoadFeedInfo();
        data.Translations = LoadTranslations();
        data.Attributions = LoadAttributions();
        
        return data;
    }
    
    /// <summary>
    /// Načíta agentúry zo súboru agency.txt
    /// </summary>
    public List<Agency> LoadAgencies()
    {
        return LoadCsvFile("agency.txt", ParseAgency);
    }
    
    /// <summary>
    /// Načíta zastávky zo súboru stops.txt
    /// </summary>
    public List<Stop> LoadStops()
    {
        return LoadCsvFile("stops.txt", ParseStop);
    }
    
    /// <summary>
    /// Načíta trasy zo súboru routes.txt
    /// </summary>
    public List<Route> LoadRoutes()
    {
        return LoadCsvFile("routes.txt", ParseRoute);
    }
    
    /// <summary>
    /// Načíta jazdy zo súboru trips.txt
    /// </summary>
    public List<Trip> LoadTrips()
    {
        return LoadCsvFile("trips.txt", ParseTrip);
    }
    
    /// <summary>
    /// Načíta časy zastávok zo súboru stop_times.txt
    /// </summary>
    public List<StopTime> LoadStopTimes()
    {
        return LoadCsvFile("stop_times.txt", ParseStopTime);
    }
    
    /// <summary>
    /// Načíta kalendáre zo súboru calendar.txt
    /// </summary>
    public List<GtsfModel.Calendar> LoadCalendars()
    {
        return LoadCsvFile("calendar.txt", ParseCalendar);
    }
    
    /// <summary>
    /// Načíta výnimky kalendára zo súboru calendar_dates.txt
    /// </summary>
    public List<CalendarDate> LoadCalendarDates()
    {
        return LoadCsvFile("calendar_dates.txt", ParseCalendarDate);
    }
    
    /// <summary>
    /// Načíta cenové atribúty zo súboru fare_attributes.txt
    /// </summary>
    public List<FareAttribute> LoadFareAttributes()
    {
        return LoadCsvFile("fare_attributes.txt", ParseFareAttribute);
    }
    
    /// <summary>
    /// Načíta cenové pravidlá zo súboru fare_rules.txt
    /// </summary>
    public List<FareRule> LoadFareRules()
    {
        return LoadCsvFile("fare_rules.txt", ParseFareRule);
    }
    
    /// <summary>
    /// Načíta tvary trás zo súboru shapes.txt
    /// </summary>
    public List<Shape> LoadShapes()
    {
        return LoadCsvFile("shapes.txt", ParseShape);
    }
    
    /// <summary>
    /// Načíta frekvencie zo súboru frequencies.txt
    /// </summary>
    public List<Frequency> LoadFrequencies()
    {
        return LoadCsvFile("frequencies.txt", ParseFrequency);
    }
    
    /// <summary>
    /// Načíta prestupy zo súboru transfers.txt
    /// </summary>
    public List<Transfer> LoadTransfers()
    {
        return LoadCsvFile("transfers.txt", ParseTransfer);
    }
    
    /// <summary>
    /// Načíta chodníky zo súboru pathways.txt
    /// </summary>
    public List<Pathway> LoadPathways()
    {
        return LoadCsvFile("pathways.txt", ParsePathway);
    }
    
    /// <summary>
    /// Načíta úrovne zo súboru levels.txt
    /// </summary>
    public List<Level> LoadLevels()
    {
        return LoadCsvFile("levels.txt", ParseLevel);
    }
    
    /// <summary>
    /// Načíta informácie o datasete zo súboru feed_info.txt
    /// </summary>
    public FeedInfo? LoadFeedInfo()
    {
        var items = LoadCsvFile("feed_info.txt", ParseFeedInfo);
        return items.FirstOrDefault();
    }
    
    /// <summary>
    /// Načíta preklady zo súboru translations.txt
    /// </summary>
    public List<Translation> LoadTranslations()
    {
        return LoadCsvFile("translations.txt", ParseTranslation);
    }
    
    /// <summary>
    /// Načíta atribúty zo súboru attributions.txt
    /// </summary>
    public List<Attribution> LoadAttributions()
    {
        return LoadCsvFile("attributions.txt", ParseAttribution);
    }
    
    // Pomocné metódy
    
    private List<T> LoadCsvFile<T>(string fileName, Func<Dictionary<string, string>, T> parser)
    {
        var filePath = Path.Combine(_gtfsDirectoryPath, fileName);
        
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }
        
        var results = new List<T>();
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null, // Ignoruj chýbajúce polia
            BadDataFound = null // Ignoruj chybné dáta
        };
        
        using var reader = new StreamReader(filePath, System.Text.Encoding.UTF8);
        using var csv = new CsvReader(reader, config);
        
        // Prečítaj hlavičku
        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord;
        
        if (headers == null || headers.Length == 0)
        {
            return results;
        }
        
        // Prečítaj všetky riadky
        while (csv.Read())
        {
            try
            {
                var row = new Dictionary<string, string>();
                
                foreach (var header in headers)
                {
                    row[header] = csv.GetField(header) ?? string.Empty;
                }
                
                var item = parser(row);
                results.Add(item);
            }
            catch
            {
                // Ignoruj chybné riadky
                Console.Error.WriteLine($"Chyba pri parsovaní riadku v súbore {fileName}");
            }
        }
        
        return results;
    }
    
    private string GetValue(Dictionary<string, string> row, string key)
    {
        return row.TryGetValue(key, out var value) ? value.Trim() : string.Empty;
    }
    
    private int? ParseInt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        return int.TryParse(value, out var result) ? result : null;
    }
    
    private double? ParseDouble(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : null;
    }
    
    private decimal? ParseDecimal(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : null;
    }
    
    private T? ParseEnum<T>(string value) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        if (int.TryParse(value, out var intValue))
        {
            if (Enum.IsDefined(typeof(T), intValue))
            {
                return (T)(object)intValue;
            }
        }
        
        return null;
    }
    
    private T ParseEnumRequired<T>(string value, T defaultValue = default) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;
        
        if (int.TryParse(value, out var intValue))
        {
            if (Enum.IsDefined(typeof(T), intValue))
            {
                return (T)(object)intValue;
            }
        }
        
        return defaultValue;
    }
    
    // Parsery pre jednotlivé entity
    
    private Agency ParseAgency(Dictionary<string, string> row)
    {
        return new Agency
        {
            AgencyId = GetValue(row, "agency_id"),
            AgencyName = GetValue(row, "agency_name"),
            AgencyUrl = GetValue(row, "agency_url"),
            AgencyTimezone = GetValue(row, "agency_timezone"),
            AgencyLang = GetValue(row, "agency_lang"),
            AgencyPhone = GetValue(row, "agency_phone"),
            AgencyFareUrl = GetValue(row, "agency_fare_url"),
            AgencyEmail = GetValue(row, "agency_email"),
            CemvSupport = ParseEnum<CemvSupport>(GetValue(row, "cemv_support"))
        };
    }
    
    private Stop ParseStop(Dictionary<string, string> row)
    {
        return new Stop
        {
            StopId = GetValue(row, "stop_id"),
            StopCode = GetValue(row, "stop_code"),
            StopName = GetValue(row, "stop_name"),
            TtsStopName = GetValue(row, "tts_stop_name"),
            StopDesc = GetValue(row, "stop_desc"),
            StopLat = ParseDouble(GetValue(row, "stop_lat")),
            StopLon = ParseDouble(GetValue(row, "stop_lon")),
            ZoneId = GetValue(row, "zone_id"),
            StopUrl = GetValue(row, "stop_url"),
            LocationType = ParseEnum<LocationType>(GetValue(row, "location_type")),
            ParentStation = GetValue(row, "parent_station"),
            StopTimezone = GetValue(row, "stop_timezone"),
            WheelchairBoarding = ParseEnum<WheelchairAccessibility>(GetValue(row, "wheelchair_boarding")),
            LevelId = GetValue(row, "level_id"),
            PlatformCode = GetValue(row, "platform_code")
        };
    }
    
    private Route ParseRoute(Dictionary<string, string> row)
    {
        return new Route
        {
            RouteId = GetValue(row, "route_id"),
            AgencyId = GetValue(row, "agency_id"),
            NetworkId = GetValue(row, "network_id"),
            RouteShortName = GetValue(row, "route_short_name"),
            RouteLongName = GetValue(row, "route_long_name"),
            RouteDesc = GetValue(row, "route_desc"),
            RouteType = ParseEnumRequired(GetValue(row, "route_type"), RouteType.Bus),
            RouteUrl = GetValue(row, "route_url"),
            RouteColor = GetValue(row, "route_color"),
            RouteTextColor = GetValue(row, "route_text_color"),
            RouteSortOrder = ParseInt(GetValue(row, "route_sort_order")),
            ContinuousPickup = ParseEnum<ContinuousPickupDropOff>(GetValue(row, "continuous_pickup")),
            ContinuousDropOff = ParseEnum<ContinuousPickupDropOff>(GetValue(row, "continuous_drop_off"))
        };
    }
    
    private Trip ParseTrip(Dictionary<string, string> row)
    {
        return new Trip
        {
            RouteId = GetValue(row, "route_id"),
            ServiceId = GetValue(row, "service_id"),
            TripId = GetValue(row, "trip_id"),
            TripHeadsign = GetValue(row, "trip_headsign"),
            TripShortName = GetValue(row, "trip_short_name"),
            DirectionId = ParseEnum<DirectionId>(GetValue(row, "direction_id")),
            BlockId = GetValue(row, "block_id"),
            ShapeId = GetValue(row, "shape_id"),
            WheelchairAccessible = ParseEnum<WheelchairAccessibility>(GetValue(row, "wheelchair_accessible")),
            BikesAllowed = ParseEnum<BikesAllowed>(GetValue(row, "bikes_allowed")),
            CarsAllowed = ParseEnum<CarsAllowed>(GetValue(row, "cars_allowed"))
        };
    }
    
    private StopTime ParseStopTime(Dictionary<string, string> row)
    {
        return new StopTime
        {
            TripId = GetValue(row, "trip_id"),
            ArrivalTime = GetValue(row, "arrival_time"),
            DepartureTime = GetValue(row, "departure_time"),
            StopId = GetValue(row, "stop_id"),
            StopSequence = ParseInt(GetValue(row, "stop_sequence")) ?? 0,
            StopHeadsign = GetValue(row, "stop_headsign"),
            PickupType = ParseEnum<PickupDropOffType>(GetValue(row, "pickup_type")),
            DropOffType = ParseEnum<PickupDropOffType>(GetValue(row, "drop_off_type")),
            ContinuousPickup = ParseEnum<ContinuousPickupDropOff>(GetValue(row, "continuous_pickup")),
            ContinuousDropOff = ParseEnum<ContinuousPickupDropOff>(GetValue(row, "continuous_drop_off")),
            ShapeDistTraveled = ParseDouble(GetValue(row, "shape_dist_traveled")),
            Timepoint = ParseEnum<TimepointType>(GetValue(row, "timepoint"))
        };
    }
    
    private GtsfModel.Calendar ParseCalendar(Dictionary<string, string> row)
    {
        return new GtsfModel.Calendar
        {
            ServiceId = GetValue(row, "service_id"),
            Monday = ParseInt(GetValue(row, "monday")) ?? 0,
            Tuesday = ParseInt(GetValue(row, "tuesday")) ?? 0,
            Wednesday = ParseInt(GetValue(row, "wednesday")) ?? 0,
            Thursday = ParseInt(GetValue(row, "thursday")) ?? 0,
            Friday = ParseInt(GetValue(row, "friday")) ?? 0,
            Saturday = ParseInt(GetValue(row, "saturday")) ?? 0,
            Sunday = ParseInt(GetValue(row, "sunday")) ?? 0,
            StartDate = GetValue(row, "start_date"),
            EndDate = GetValue(row, "end_date")
        };
    }
    
    private CalendarDate ParseCalendarDate(Dictionary<string, string> row)
    {
        return new CalendarDate
        {
            ServiceId = GetValue(row, "service_id"),
            Date = GetValue(row, "date"),
            ExceptionType = ParseEnumRequired(GetValue(row, "exception_type"), ExceptionType.ServiceAdded)
        };
    }
    
    private FareAttribute ParseFareAttribute(Dictionary<string, string> row)
    {
        return new FareAttribute
        {
            FareId = GetValue(row, "fare_id"),
            Price = ParseDecimal(GetValue(row, "price")) ?? 0,
            CurrencyType = GetValue(row, "currency_type"),
            PaymentMethod = ParseEnumRequired(GetValue(row, "payment_method"), PaymentMethod.OnBoard),
            Transfers = ParseEnum<TransfersAllowed>(GetValue(row, "transfers")),
            AgencyId = GetValue(row, "agency_id"),
            TransferDuration = ParseInt(GetValue(row, "transfer_duration"))
        };
    }
    
    private FareRule ParseFareRule(Dictionary<string, string> row)
    {
        return new FareRule
        {
            FareId = GetValue(row, "fare_id"),
            RouteId = GetValue(row, "route_id"),
            OriginId = GetValue(row, "origin_id"),
            DestinationId = GetValue(row, "destination_id"),
            ContainsId = GetValue(row, "contains_id")
        };
    }
    
    private Shape ParseShape(Dictionary<string, string> row)
    {
        return new Shape
        {
            ShapeId = GetValue(row, "shape_id"),
            ShapePtLat = ParseDouble(GetValue(row, "shape_pt_lat")) ?? 0,
            ShapePtLon = ParseDouble(GetValue(row, "shape_pt_lon")) ?? 0,
            ShapePtSequence = ParseInt(GetValue(row, "shape_pt_sequence")) ?? 0,
            ShapeDistTraveled = ParseDouble(GetValue(row, "shape_dist_traveled"))
        };
    }
    
    private Frequency ParseFrequency(Dictionary<string, string> row)
    {
        return new Frequency
        {
            TripId = GetValue(row, "trip_id"),
            StartTime = GetValue(row, "start_time"),
            EndTime = GetValue(row, "end_time"),
            HeadwaySecs = ParseInt(GetValue(row, "headway_secs")) ?? 0,
            ExactTimes = ParseEnum<FrequencyExactTimes>(GetValue(row, "exact_times"))
        };
    }
    
    private Transfer ParseTransfer(Dictionary<string, string> row)
    {
        return new Transfer
        {
            FromStopId = GetValue(row, "from_stop_id"),
            ToStopId = GetValue(row, "to_stop_id"),
            TransferType = ParseEnumRequired(GetValue(row, "transfer_type"), TransferType.Recommended),
            MinTransferTime = ParseInt(GetValue(row, "min_transfer_time"))
        };
    }
    
    private Pathway ParsePathway(Dictionary<string, string> row)
    {
        return new Pathway
        {
            PathwayId = GetValue(row, "pathway_id"),
            FromStopId = GetValue(row, "from_stop_id"),
            ToStopId = GetValue(row, "to_stop_id"),
            PathwayMode = ParseEnumRequired(GetValue(row, "pathway_mode"), PathwayMode.Walkway),
            IsBidirectional = ParseEnumRequired(GetValue(row, "is_bidirectional"), PathwayDirection.Unidirectional),
            Length = ParseDouble(GetValue(row, "length")),
            TraversalTime = ParseInt(GetValue(row, "traversal_time")),
            StairCount = ParseInt(GetValue(row, "stair_count")),
            MaxSlope = ParseDouble(GetValue(row, "max_slope")),
            MinWidth = ParseDouble(GetValue(row, "min_width")),
            SignpostedAs = GetValue(row, "signposted_as"),
            ReversedSignpostedAs = GetValue(row, "reversed_signposted_as")
        };
    }
    
    private Level ParseLevel(Dictionary<string, string> row)
    {
        return new Level
        {
            LevelId = GetValue(row, "level_id"),
            LevelIndex = ParseDouble(GetValue(row, "level_index")) ?? 0,
            LevelName = GetValue(row, "level_name")
        };
    }
    
    private FeedInfo ParseFeedInfo(Dictionary<string, string> row)
    {
        return new FeedInfo
        {
            FeedPublisherName = GetValue(row, "feed_publisher_name"),
            FeedPublisherUrl = GetValue(row, "feed_publisher_url"),
            FeedLang = GetValue(row, "feed_lang"),
            DefaultLang = GetValue(row, "default_lang"),
            FeedStartDate = GetValue(row, "feed_start_date"),
            FeedEndDate = GetValue(row, "feed_end_date"),
            FeedVersion = GetValue(row, "feed_version"),
            FeedContactEmail = GetValue(row, "feed_contact_email"),
            FeedContactUrl = GetValue(row, "feed_contact_url")
        };
    }
    
    private Translation ParseTranslation(Dictionary<string, string> row)
    {
        return new Translation
        {
            TableName = GetValue(row, "table_name"),
            FieldName = GetValue(row, "field_name"),
            Language = GetValue(row, "language"),
            TranslationText = GetValue(row, "translation"),
            RecordId = GetValue(row, "record_id"),
            RecordSubId = GetValue(row, "record_sub_id"),
            FieldValue = GetValue(row, "field_value")
        };
    }
    
    private Attribution ParseAttribution(Dictionary<string, string> row)
    {
        return new Attribution
        {
            AttributionId = GetValue(row, "attribution_id"),
            AgencyId = GetValue(row, "agency_id"),
            RouteId = GetValue(row, "route_id"),
            TripId = GetValue(row, "trip_id"),
            OrganizationName = GetValue(row, "organization_name"),
            IsProducer = ParseInt(GetValue(row, "is_producer")),
            IsOperator = ParseInt(GetValue(row, "is_operator")),
            IsAuthority = ParseInt(GetValue(row, "is_authority")),
            AttributionUrl = GetValue(row, "attribution_url"),
            AttributionEmail = GetValue(row, "attribution_email"),
            AttributionPhone = GetValue(row, "attribution_phone")
        };
    }
}
