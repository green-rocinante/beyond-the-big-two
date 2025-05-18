using System;
using System.Collections.Generic;

namespace BeyondtheBigTwo
{
    public class Producer : Location, IFilterable   // inheriting from Location, filter via Interface
    {
        public List<string> ProduceTypes { get; }
        public bool IsOrganic { get; }
        public bool IsRegenerative { get; }
        public Dictionary<string, string> AgroPractices { get; }

        public Producer(double latitude, double longitude, string postcode, List<string> produceTypes,
                        bool isOrganic, bool isRegenerative, Dictionary<string, string> agroPractices)
            : base(latitude, longitude, postcode)
        {
            ProduceTypes = produceTypes;
            IsOrganic = isOrganic;
            IsRegenerative = isRegenerative;
            AgroPractices = agroPractices;
        }

        public override bool MatchesFilter(UserFilter filter)
        {
            if (filter == null) return true;

            // if (filter.Postcode != null && this.Postcode != filter.Postcode)
            //     return false;

            if (filter.ProduceType != null &&
                !ProduceTypes.Any(p => p.Equals(filter.ProduceType, StringComparison.OrdinalIgnoreCase)))
                return false;

            if (filter.RequiresOrganic && !IsOrganic)
                return false;

            if (filter.RequiresRegenerative && !IsRegenerative)
                return false;

            if (filter.Location != null && GetDistanceTo(filter.Location) > filter.MaxDistance)
                return false;

            if (filter.OnlyDonationLocations)
                return false;

            return true;
        }
    }
}