using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using SplashKitSDK;

namespace BeyondtheBigTwo
{
    public class Program
    {
        static string postcodeInput = "unknown";
        static string distanceInput = "0";

        static bool requiresOrganic = false;
        static bool requiresRegenerative = false;
        static bool onlyDonation = false;

        public static UserFilter BuildFilter()
        {
            return new UserFilter
            {
                Postcode = postcodeInput,
                MaxDistance = double.TryParse(distanceInput, out var km) ? km : 50,
                RequiresOrganic = requiresOrganic,
                RequiresRegenerative = requiresRegenerative,
                OnlyDonationLocations = onlyDonation,
                Location = new PickupPoint(-38.17, 144.56, postcodeInput, "User", "", false)
            };
        }

        static void HandleToggles()
        {
            if (SplashKit.KeyTyped(KeyCode.OKey)) requiresOrganic = !requiresOrganic;
            if (SplashKit.KeyTyped(KeyCode.RKey)) requiresRegenerative = !requiresRegenerative;
            if (SplashKit.KeyTyped(KeyCode.DKey)) onlyDonation = !onlyDonation;
        }

        public static void Main()
        {
            FoodMap map = new FoodMap();
            MapSeeder seedData = new MapSeeder(map);
            MapServer server = null;
            Thread serverThread = null;
            UserFilter filter = new UserFilter();

            Window window = new Window("Beyond the Big Two Map Generator", 800, 600);
            SplashKit.LoadFont("input", "arial.ttf");

            Rectangle postcodeRect = new Rectangle { X = 200, Y = 50, Width = 300, Height = 30 };
            Rectangle distanceRect = new Rectangle { X = 200, Y = 120, Width = 300, Height = 30 };

            // Request postcode first
            SplashKit.StartReadingText(postcodeRect);

            string currentStage = "postcode";

            List<string> messages = new();

            Button serverButton = new Button { X = 20, Y = 300, Caption = "Start Server" };
            serverButton.OnClick += (btn) =>
            {
                if (server == null)
                {
                    server = new MapServer();
                    serverThread = new Thread(() => server.Run());
                    serverThread.IsBackground = true;
                    serverThread.Start();

                    messages.Clear();
                    messages.Add("Server started: http://localhost:8080");
                }
                else
                {
                    messages.Add("Server already running.");
                }
            };

            Button mapButton = new Button { X = 140, Y = 300, Caption = "Generate Map" };
            mapButton.OnClick += (btn) =>
            {
                filter = BuildFilter();
                map.CurrentFilter = filter;
                var results = map.SortResults((a, b) => a.GetDistanceTo(filter.Location).CompareTo(b.GetDistanceTo(filter.Location)));
                // map.DisplayResults(results); // Optional terminal window output

                string json = JsonSerializer.Serialize(results, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new LocationConverter() }
                });

                Directory.CreateDirectory("Resources/server");
                File.WriteAllText("Resources/server/data.json", json);
                messages.Clear();
                messages.Add("Map data exported to data.json");

                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "http://localhost:8080/map.html",
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    messages.Add("Could not open browser: " + ex.Message);
                }
            };

            Button refreshButton = new Button { X = 260, Y = 300, Caption = "Refresh Map" };
            refreshButton.OnClick += (btn) =>
            {
                filter = BuildFilter();
                map.CurrentFilter = filter;
                var results = map.SortResults((a, b) => a.GetDistanceTo(filter.Location).CompareTo(b.GetDistanceTo(filter.Location)));

                string json = JsonSerializer.Serialize(results, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new LocationConverter() }
                });

                File.WriteAllText("Resources/server/data.json", json);
                messages.Clear();
                messages.Add("App data refreshed. Please refresh browser.");
                map.DisplayResults(results);
            };


            Button quitButton = new Button { X = 440, Y = 300, Caption = "Quit" };
            quitButton.OnClick += (btn) =>
            {
                server?.StopServer();
                Environment.Exit(0);
            };

            while (!window.CloseRequested)
            {
                SplashKit.ProcessEvents();
                window.Clear(Color.White);

                // First get user inputs for postcode and distance
                if (SplashKit.ReadingText())
                {
                    SplashKit.DrawCollectedText(Color.Black, SplashKit.FontNamed("input"), 18, SplashKit.OptionDefaults());
                }
                else
                {
                    if (!SplashKit.TextEntryCancelled())
                    {
                        if (currentStage == "postcode")
                        {
                            postcodeInput = SplashKit.TextInput().Trim();
                            if (postcodeInput.Length == 4 && int.TryParse(postcodeInput, out _))
                            {
                                currentStage = "maximum distance";
                                SplashKit.StartReadingText(distanceRect);
                            }
                            else
                            {
                                messages.Clear();
                                // messages.Add("Invalid postcode. Please enter a 4-digit number.");
                                SplashKit.StartReadingText(postcodeRect);
                            }
                        }
                        else if (currentStage == "maximum distance")
                        {
                            distanceInput = SplashKit.TextInput().Trim();
                            if (double.TryParse(distanceInput, out _))
                            {
                                currentStage = "done";
                            }
                            else
                            {
                                messages.Clear();
                                // messages.Add("Invalid distance. Please enter a numeric value.");
                                SplashKit.StartReadingText(distanceRect);
                            }
                        }
                    }
                }

                // Once inputs are complete, show UI
                if (currentStage == "done")
                {
                    HandleToggles();

                    window.DrawText("Postcode: " + postcodeInput, Color.Black, 20, 20);
                    window.DrawText("Distance (km): " + distanceInput, Color.Black, 20, 50);

                    window.DrawText("Press O/R/D to toggle filters...", Color.Gray, 20, 90);
                    window.DrawText("[O] Organic Only: " + requiresOrganic, Color.Black, 20, 110);
                    window.DrawText("[R] Regenerative Only: " + requiresRegenerative, Color.Black, 20, 130);
                    window.DrawText("[D] Donation Pickup Points Only: " + onlyDonation, Color.Black, 20, 150);

                    serverButton.Draw();
                    mapButton.Draw();
                    refreshButton.Draw();
                    quitButton.Draw();

                    serverButton.HandleInput();
                    mapButton.HandleInput();
                    refreshButton.HandleInput();
                    quitButton.HandleInput();

                    int msgY = 400;
                    foreach (var msg in messages)
                    {
                        window.DrawText(msg, Color.DarkBlue, 20, msgY);
                        msgY += 20;
                    }
                }
                else
                {
                    SplashKit.DrawRectangle(Color.Black, currentStage == "postcode" ? postcodeRect : distanceRect);
                    SplashKit.DrawText("Enter your " + currentStage + " and press Enter", Color.Red, SplashKit.FontNamed("input"), 16, 200, 10);
                }

                window.Refresh(60);
            }
            server?.StopServer();
        }
    }
}
