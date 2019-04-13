using System;
using Newtonsoft.Json;

// This class implements logic to format a GPS location in GeoJSON format,
// such as the following.  See http://geojson.org
//
//   "location": {
//     "type": "Point",
//     "coordinates": [
//       -86.8138056,
//       39.6335556
//     ]
//   }
//
// Chris Joakim, Microsoft, 2019/04/12

namespace Hackathon {

    public class Location {
        public string type { get; set; }
        public double[] coordinates { get; set; }

        // Default constructor
        public Location(double lat, double lng)
        {
            this.type = "Point";
            this.coordinates =  new double[2];
            this.coordinates[0] = lng;
            this.coordinates[1] = lat;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
