using SplashKitSDK;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;
using System.IO;

namespace BeyondtheBigTwo
{
    public class MapServer
    {
        private bool _running = true;
        private WebServer _server;

        public MapServer()
        {
            _server = new WebServer(8080);
        }

        public void StopServer()
        {
            _running = false;
        }

        public void Run()
        {
            Console.WriteLine("Current working dir: " + Environment.CurrentDirectory);
            while (_running)
            {
                if (SplashKit.HasIncomingRequests(_server))
                {
                    HttpRequest request = _server.NextWebRequest;
                    Console.WriteLine("Serving: " + request.URI);
                    HandleRequest(request);
                }
            }
        }

        private void HandleRequest(HttpRequest r)
        {
            try
            {
                string basePath = Path.Combine(Environment.CurrentDirectory, "Resources", "server");

                if (r.IsGetRequestFor("/") || r.IsGetRequestFor("/map.html"))
                {
                    string path = Path.Combine(basePath, "map.html");
                    Console.WriteLine("→ Sending file: " + path);
                    string html = File.ReadAllText(path);
                    r.SendResponse(HttpStatusCode.HttpStatusOk, html, "text/html");
                }
                else if (r.IsGetRequestFor("/index.js"))
                {
                    string path = Path.Combine(basePath, "index.js");
                    Console.WriteLine("→ Sending file: " + path);
                    string js = File.ReadAllText(path);
                    r.SendResponse(HttpStatusCode.HttpStatusOk, js, "application/javascript");
                }
                else if (r.IsGetRequestFor("/data.json"))
                {
                    string path = Path.Combine(basePath, "data.json");
                    Console.WriteLine("→ Sending file: " + path);
                    string json = File.ReadAllText(path);
                    r.SendResponse(HttpStatusCode.HttpStatusOk, json, "application/json");
                }
                else if (r.IsGetRequestFor("/stop"))
                {
                    r.SendResponse("Server stopped");
                    StopServer();
                }
                else
                {
                    r.SendResponse(HttpStatusCode.HttpStatusNotFound);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Server Error] " + ex.Message);
                r.SendResponse(HttpStatusCode.HttpStatusInternalServerError);
            }
        }
    }

    public class LocationConverter : JsonConverter<Location>
    {
        public override Location Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            throw new System.NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Location value, JsonSerializerOptions options)
        {
            if (value is Producer prod)
            {
                writer.WriteStartObject();
                writer.WriteString("Type", "Producer");
                writer.WriteString("Name", prod.Name);
                writer.WriteString("Postcode", prod.Postcode);
                writer.WriteNumber("Latitude", prod.Latitude);
                writer.WriteNumber("Longitude", prod.Longitude);
                writer.WriteBoolean("IsOrganic", prod.IsOrganic);
                writer.WriteBoolean("IsRegenerative", prod.IsRegenerative);
                writer.WritePropertyName("ProduceTypes");
                JsonSerializer.Serialize(writer, prod.ProduceTypes);
                writer.WritePropertyName("AgroPractices");
                JsonSerializer.Serialize(writer, prod.AgroPractices);

                writer.WriteString("Website", value.Website);
                writer.WriteString("Facebook", value.Facebook);
                writer.WriteString("GoogleMapsUrl", value.GoogleMapsUrl);
                writer.WriteEndObject();
            }
            else if (value is PickupPoint pp)
            {
                writer.WriteStartObject();
                writer.WriteString("Type", "PickupPoint");
                writer.WriteString("Postcode", pp.Postcode);
                writer.WriteNumber("Latitude", pp.Latitude);
                writer.WriteNumber("Longitude", pp.Longitude);
                writer.WriteString("Label", pp.Label);
                writer.WriteString("OpeningHours", pp.OpeningHours);
                writer.WriteBoolean("AcceptsDonations", pp.AcceptsDonations);

                writer.WriteString("Website", value.Website);
                writer.WriteString("Facebook", value.Facebook);
                writer.WriteString("GoogleMapsUrl", value.GoogleMapsUrl);
                writer.WriteEndObject();
            }
        }
    }
}
