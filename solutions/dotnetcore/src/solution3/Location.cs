using Newtonsoft.Json;
using System;

namespace solution3
{

    public class Location
    {
        public string type { get; set; }
        public double[] coordinates { get; set; }

        // Default constructor
        public Location(double lat, double lng)
        {
            this.type = "Point";
            this.coordinates = new double[2];
            this.coordinates[0] = lng;
            this.coordinates[1] = lat;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}