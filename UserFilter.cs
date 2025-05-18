using System;

namespace BeyondtheBigTwo
{
    public class UserFilter
    {
        // Location criteria
        public string Postcode { get; set; }    // Optional filter
        public Location Location { get; set; }  // Optional anchor point to filter by distance
        public double MaxDistance { get; set; } = double.MaxValue;

        // Producer specific
        public string ProduceType { get; set; } // Optional filter
        public bool RequiresOrganic { get; set; }
        public bool RequiresRegenerative { get; set; }

        // Pickup point specific
        public bool OnlyDonationLocations { get; set; }

        // Thinking about adding hours open filter later on

        // Constructor to initialise with maxdistance and no filters
        public UserFilter()
        {
            MaxDistance = double.MaxValue;  // Setting a default
            RequiresOrganic = false;
            RequiresRegenerative = false;
            OnlyDonationLocations = false;
        }
    }
}