namespace Hackathon
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using CsvHelper;
    using Newtonsoft.Json;

    // Class FileUtil implements IO operations for local data files.
    // Chris Joakim, Microsoft, 2019/04/12
    public class FileUtil
    {
        public FileUtil()
        {
            // Default constructor
        }
        public string CurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
        public char PathSeparator()
        {
           return Path.DirectorySeparatorChar; 
        }
        public string AbsolutePath(string relativeFilename)
        {
            return CurrentDirectory() + PathSeparator() + relativeFilename;
        }
        public List<Airport> ReadAirportsCsv(string[] filterCountriesList)
        {
            List<Airport> airports = new List<Airport>();
            try {
                string infile = AbsolutePath("data/openflights_airports.csv");
                Console.WriteLine("ReadAirportsCsv: {0}", infile);

                // See https://joshclose.github.io/CsvHelper/
                var config = new CsvHelper.Configuration.Configuration
                {
                    HasHeaderRecord   = true,
                    HeaderValidated   = null,
                    MissingFieldFound = null,
                    IgnoreBlankLines  = true,
                    IncludePrivateMembers = false,
                    IgnoreReferences = true
                };
                using (var reader = new StreamReader(infile))
                using (var csv = new CsvReader(reader, config))
                {    
                    IEnumerable<Airport> rows = csv.GetRecords<Airport>();
                    foreach (var a in rows) 
                    {
                        if ((a.IsValid()) && (a.IsInCountry(filterCountriesList)))
                        {
                            airports.Add(a);
                        }
                    }
                }
            }
            catch (Exception e) {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error in ReadAirportsCsv: {0}, Message: {1}", e.Message, baseException.Message);
            }
            return airports;
        }
    }
}
