using Newtonsoft.Json;
using System;

namespace solution3
{
    public class Airport
    {
        public string id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string iata_code { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public Location location { get; set; }
        public double altitude { get; set; }
        public float timezone_num { get; set; }
        public string timezone_code { get; set; }
        // Default constructor
        public Airport()
        {
            // Default constructor
        }

        public void postParse()
        {
            id = iata_code;
            this.location = new Location(this.latitude, this.longitude);
        }

        public bool IsInCountry(string[] countryList)
        {
            try
            {
                foreach (var c in countryList)
                {
                    if (country.ToLower().Equals(c.ToLower()))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public bool IsValid()
        {
            try
            {
                if (String.IsNullOrEmpty(name))
                {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Name:{1}", AirportId, Name);
                    return false;
                }
                if (String.IsNullOrEmpty(city))
                {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  City:{1}", AirportId, City);
                    return false;
                }
                if (String.IsNullOrEmpty(country))
                {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Country:{1}", AirportId, Country);
                    return false;
                }
                if (String.IsNullOrEmpty(iata_code))
                {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  IataCode:{1}", AirportId, IataCode);
                    return false;
                }
                if (Math.Abs(latitude) < 0.00001)
                {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Latitude:{1}", AirportId, Latitude);
                    return false;
                }
                if (Math.Abs(longitude) < 0.00001)
                {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Longitude:{1}", AirportId, Longitude);
                    return false;
                }
                if ((altitude < -100) || (altitude > 6000))
                {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Altitude:{1}", AirportId, Altitude);
                    return false;
                }
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error in IsValid: {0}, Message: {1}", e.Message, baseException.Message);
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            bool valid = IsValid();
            return $"{id}:{iata_code}:{name}:{city}:{country}:{latitude}:{longitude}:{timezone_num}:{valid}";
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}