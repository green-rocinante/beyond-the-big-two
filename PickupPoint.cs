

namespace BeyondtheBigTwo
{
    public class PickupPoint : Location, IFilterable   // inheriting from Location
    {
        public string Label { get; }
        public string OpeningHours { get; }
        public bool AcceptsDonations { get; }

        public string Website { get; set; }
        public string Facebook { get; set; }
        public string GoogleMapsUrl { get; set; }

        public PickupPoint(double latitude, double longitude, string postcode, string label,
                           string openingHours, bool acceptsDonations)
            : base(latitude, longitude, postcode)
        {
            Label = label;
            OpeningHours = openingHours;
            AcceptsDonations = acceptsDonations;
        }

        public override bool MatchesFilter(UserFilter filter)
        {
            if (filter == null) return true;

            // if (filter.Postcode != null && this.Postcode != filter.Postcode)
            //     return false;

            if (filter.Location != null && GetDistanceTo(filter.Location) > filter.MaxDistance)
                return false;

            if (filter.OnlyDonationLocations && !AcceptsDonations)
                return false;

            return true;
        }
    }
}