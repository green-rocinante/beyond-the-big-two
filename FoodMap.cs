using System;
using System.Collections.Generic;
using System.Linq;

namespace BeyondtheBigTwo
{
    public class FoodMap
    {
        // Setting our collections of producers and pickup points
        private List<Producer> _producers = new();
        private List<PickupPoint> _pickupPoints = new();

        // Current filter state
        public UserFilter CurrentFilter { get; set; }

        // Constructor to initialise map
        public FoodMap()
        {
            _producers = new List<Producer>();
            _pickupPoints = new List<PickupPoint>();
            CurrentFilter = null;
        }

        // Add new producer
        public void AddProducer(Producer producer)
        {
            _producers.Add(producer);
        }

        // Add new pickup point
        public void AddPickupPoint(PickupPoint point)
        {
            _pickupPoints.Add(point);
        }

        // Filtering results using UserFilter CurrentFilter parameter
        // Passes filter to interface method to get yes or no for match
        public List<Location> FilterResults()
        {
            List<Location> results = new List<Location>();

            List<IFilterable> allItems = new();
            allItems.AddRange(_producers);
            allItems.AddRange(_pickupPoints);

            foreach (IFilterable item in allItems)
            {
                // Uses interface bool to find match
                if (item.MatchesFilter(CurrentFilter))
                {
                    results.Add((Location)item);  // Casting polymorphically as Location can be PUP or Producer
                }
                // else
                // {
                //     Console.WriteLine("Filtered out: " + item.GetType().Name);
                // }
            }
            return results; // returns a list of locations
        }

        // Example of using system default delegate Comparison with T wildcard
        public List<Location> SortResults(Comparison<Location> comparison)
        {
            List<Location> filtered = FilterResults();  // Reuse existing method

            // Sort list of locations 
            filtered.Sort(comparison);

            return filtered;
        }

        // Display results
        public void DisplayResults(List<Location> locations)
        {
            if (locations.Count == 0)
            {
                Console.WriteLine("No results found for current filter.");
                return;
            }

            foreach (var loc in locations)
            {
                double distance = loc.GetDistanceTo(CurrentFilter.Location);

                if (loc is Producer prod)
                {
                    Console.WriteLine($"[Producer] Postcode: {prod.Postcode}, Organic: {prod.IsOrganic}, Regenerative: {prod.IsRegenerative}, Agro-ecology practices used: {string.Join(", ", prod.AgroPractices.Keys)}, Produce: {string.Join(", ", prod.ProduceTypes)}");
                    // Added a print check for distance
                    Console.WriteLine($"\tDistance to {prod.Postcode} from your postcode: {distance:F2} km\n");

                }
                else if (loc is PickupPoint point)
                {
                    Console.WriteLine($"[Pickup Point] Name: {point.Label}, Postcode: {point.Postcode}, Accepts Donations: {point.AcceptsDonations}");
                    // Checking distance
                    Console.WriteLine($"\tDistance to {point.Postcode} from your postcode: {distance:F2} km\n");
                }
            }
        }

        // Clear filters
        public void ClearFilter()
        {
            CurrentFilter = null;
        }
    }
}