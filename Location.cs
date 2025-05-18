using System;

namespace BeyondtheBigTwo
{
    public abstract class Location
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public string Postcode { get; }

        public string Name { get; set;  }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string GoogleMapsUrl => $"https://www.google.com/maps/search/?api=1&query={Latitude},{Longitude}";

        // Abstract class so protected constructor called by child classes
        protected Location(double latitude, double longitude, string postcode)
        {
            Latitude = latitude;
            Longitude = longitude;
            Postcode = postcode;
        }

        public double GetDistanceTo(Location other)
        {
            // double dx = this.Latitude - other.Latitude;
            // double dy = this.Longitude - other.Longitude;
            // return Math.Sqrt(dx * dx + dy * dy);

            // Using something called the Haversine formula to get real km

            double R = 6371; // Earth's radius in km
            double lat1 = this.Latitude * (Math.PI / 180);
            double lat2 = other.Latitude * (Math.PI / 180);
            double dLat = (other.Latitude - this.Latitude) * (Math.PI / 180);
            double dLon = (other.Longitude - this.Longitude) * (Math.PI / 180);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // distance in km
        }

        public abstract bool MatchesFilter(UserFilter filter);  // allows polymorphism if not using IFilterable
    }
}