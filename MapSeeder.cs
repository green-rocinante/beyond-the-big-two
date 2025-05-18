using System;
using SplashKitSDK;

namespace BeyondtheBigTwo
{
    public class MapSeeder
    {
        public MapSeeder(FoodMap map)
        {
            var p1 = new Producer(
                latitude: -38.2778737,
                longitude: 145.0772523,
                postcode: "3915",
                produceTypes: new List<string> { "Grains", "Flour" },
                isOrganic: true,
                isRegenerative: true,
                agroPractices: new Dictionary<string, string>
                {
                    {"Soil testing", "Used to guide compost and minerals"},
                    {"Cover cropping", "Improves soil health"}
                }
            );
            p1.Name = "Tuerong Farm";
            p1.Website = "https://www.tuerongfarm.com.au";
            p1.Facebook = "https://www.facebook.com/tuerongfarm";
            map.AddProducer(p1);

            var p2 = new Producer(
                latitude: -37.5500,
                longitude: 149.7500,
                postcode: "3892",
                produceTypes: new List<string> { "Native Grains" },
                isOrganic: true,
                isRegenerative: true,
                agroPractices: new Dictionary<string, string>
                {
                    {"Cultural land care", "Led by Indigenous communities"}
                }
            );
            p2.Name = "Black Duck Foods";
            p2.Website = "https://www.blackduckfoods.com.au";
            p2.Facebook = "https://www.facebook.com/blackduckfoods";
            map.AddProducer(p2);

            var p3 = new Producer(
                latitude: -30.9833,
                longitude: 150.2500,
                postcode: "2380",
                produceTypes: new List<string> { "Flour" },
                isOrganic: true,
                isRegenerative: true,
                agroPractices: new Dictionary<string, string>
                {
                    {"Stone milling", "Preserves nutrition"}
                }
            );
            p3.Name = "Wholegrain Milling Co.";
            p3.Website = "https://www.wholegrain.com.au";
            p3.Facebook = "https://www.facebook.com/WholegrainMillingCo";
            map.AddProducer(p3);

            var p4 = new Producer(
                latitude: -37.7749,
                longitude: 144.9634,
                postcode: "3057",
                produceTypes: new List<string> { "Vegetables", "Fruits" },
                isOrganic: true,
                isRegenerative: false,
                agroPractices: new Dictionary<string, string>()
            );
            p4.Name = "CERES Fair Food";
            p4.Website = "https://www.ceresfairfood.org.au";
            p4.Facebook = "https://www.facebook.com/ceresfairfood";
            map.AddProducer(p4);

            var p5 = new Producer(
                latitude: -38.3333,
                longitude: 144.3167,
                postcode: "3228",
                produceTypes: new List<string> { "Bread", "Bakery" },
                isOrganic: true,
                isRegenerative: true,
                agroPractices: new Dictionary<string, string>
                {
                    {"Natural fermentation", "Maintains microbial health"}
                }
            );
            p5.Name = "Zeally Bay Sourdough";
            p5.Website = "https://zeallybaysourdough.com.au";
            p5.Facebook = "https://www.facebook.com/zeallybaysourdough";
            map.AddProducer(p5);

            var pp1 = new PickupPoint(
                latitude: -38.2672,
                longitude: 144.5265,
                postcode: "3226",
                label: "Feed Me Bellarine",
                openingHours: "Mon-Fri 9am-5pm",
                acceptsDonations: true
            );
            pp1.Name = "Feed Me Bellarine";
            pp1.Website = "https://www.feedme.org.au";
            pp1.Facebook = "https://www.facebook.com/feedmebellarine";
            map.AddPickupPoint(pp1);

            var pp2 = new PickupPoint(
                latitude: -38.1744,
                longitude: 144.5700,
                postcode: "3222",
                label: "Bellarine Wholefoods",
                openingHours: "Mon-Sat 9am-5pm",
                acceptsDonations: false
            );
            pp2.Name = "Bellarine Wholefoods";
            pp2.Website = "https://www.bellarinewholefoods.com.au";
            pp2.Facebook = "https://www.facebook.com/bellarinewholefoods";
            map.AddPickupPoint(pp2);

            var pp3 = new PickupPoint(
                latitude: -38.1710,
                longitude: 144.5700,
                postcode: "3222",
                label: "Farm My School",
                openingHours: "Mon-Fri 8am-4pm",
                acceptsDonations: false
            );
            pp3.Name = "Farm My School";
            pp3.Website = "https://www.farmmyschool.com";
            pp3.Facebook = "https://www.facebook.com/farmmyschool";
            map.AddPickupPoint(pp3);

            var pp4 = new PickupPoint(
                latitude: -38.2675,
                longitude: 144.5265,
                postcode: "3226",
                label: "My Bellarine Kitchen",
                openingHours: "Wed-Sun 10am-4pm",
                acceptsDonations: false
            );
            pp4.Name = "My Bellarine Kitchen";
            pp4.Website = "https://www.mybellarinekitchen.com.au";
            pp4.Facebook = "https://www.facebook.com/mybellarinekitchen";
            map.AddPickupPoint(pp4);
        }
    }
}
