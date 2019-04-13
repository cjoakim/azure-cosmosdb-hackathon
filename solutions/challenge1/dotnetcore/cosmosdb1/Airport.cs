using System;
using Newtonsoft.Json;

// Instances of this class represent an Airport in the OpenFlights CSV data, like this:
// AirportId,Name,City,Country,IataCode,IcaoCode,Latitude,Longitude,Altitude,TimezoneNum,Dst,TimezoneCode
// 3682,"Hartsfield Jackson Atlanta Intl","Atlanta","United States","ATL","KATL",33.636719,-84.428067,1026,-5,"A","America/New_York"
//
// Chris Joakim, Microsoft, 2019/04/12

namespace Hackathon {

    public class Airport {
        public string pk { get; set; }
        public int    AirportId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string IataCode { get; set; }
        public string IcaoCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location location { get; set; }
        public double Altitude { get; set; }
        public float  TimezoneNum { get; set; }
        public string Dst { get; set; }
        public string TimezoneCode { get; set; }

        // Default constructor
        public Airport()
        {
            // Default constructor
        }

        public void postParse()
        {
            pk = IataCode;
            this.location = new Location(this.Latitude, this.Longitude);
        }
        public bool IsInCountry(string[] countryList)
        {
            try {
                foreach (var c in countryList) 
                {
                    if (Country.ToLower().Equals(c.ToLower())) {
                        return true;
                    }
                }
            }
            catch (Exception) {
                return false;
            }
            return false;
        }
        public bool IsValid()
        {
            try {
                if (Math.Abs(AirportId) < 1) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}", AirportId);
                    return false;
                }
                if (String.IsNullOrEmpty(Name)) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Name:{1}", AirportId, Name);
                    return false;
                }
                if (String.IsNullOrEmpty(City)) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  City:{1}", AirportId, City);
                    return false;
                }
                if (String.IsNullOrEmpty(Country)) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Country:{1}", AirportId, Country);
                    return false;
                }
                if (String.IsNullOrEmpty(IataCode)) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  IataCode:{1}", AirportId, IataCode);
                    return false;
                }
                if (String.IsNullOrEmpty(TimezoneCode)) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  TimezoneCode:{1}", AirportId, TimezoneCode);
                    return false;
                }
                if (Math.Abs(Latitude) < 0.00001) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Latitude:{1}", AirportId, Latitude);
                    return false;
                }
                if (Math.Abs(Longitude) < 0.00001) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Longitude:{1}", AirportId, Longitude);
                    return false;
                }
                if ((Altitude < -100) || (Altitude > 6000)) {
                    //Console.WriteLine("IsValid Error on AirportId: {0}  Altitude:{1}", AirportId, Altitude);
                    return false;
                }
            }
            catch (Exception e) {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error in IsValid: {0}, Message: {1}", e.Message, baseException.Message);
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            bool valid = IsValid();
            return $"{AirportId}:{IataCode}:{Name}:{City}:{Country}:{Latitude}:{Longitude}:{TimezoneCode}:{TimezoneNum}:{valid}";
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}