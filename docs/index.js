window.onload = function () {
  if (typeof L === 'undefined') {
    console.error("Leaflet not loaded.");
    return;
  }

  const map = L.map('map').setView([-38.2, 144.6], 9);

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '&copy; OpenStreetMap contributors'
  }).addTo(map);

  const producerIcon = L.icon({
    iconUrl: 'https://cdn-icons-png.flaticon.com/512/2909/2909765.png',
    iconSize: [32, 32],
    iconAnchor: [16, 32],
    popupAnchor: [0, -32]
  });

  const pickupIcon = L.icon({
    iconUrl: 'https://cdn-icons-png.flaticon.com/512/1903/1903162.png',
    iconSize: [28, 28],
    iconAnchor: [14, 28],
    popupAnchor: [0, -28]
  });

  fetch('data.json')
    .then(res => {
      if (!res.ok) throw new Error("Could not load data.json");
      return res.json();
    })
    .then(data => {
      console.log("✅ Loaded data:", data);

      data.forEach((loc, i) => {
        if (!loc.Latitude || !loc.Longitude) {
          console.warn(`⚠️ Skipping entry ${i}: missing coordinates`, loc);
          return;
        }

        const icon = loc.Type === "Producer" ? producerIcon : pickupIcon;
        const marker = L.marker([loc.Latitude, loc.Longitude], { icon });

        let popup = `<strong>${loc.Name || loc.Type || "Unnamed Location"}</strong><br/>Postcode: ${loc.Postcode || "N/A"}`;

        if (loc.Type === "Producer") {
          popup += `<br/>Produce: ${Array.isArray(loc.ProduceTypes) ? loc.ProduceTypes.join(', ') : "N/A"}`;
          popup += `<br/>Organic: ${loc.IsOrganic ? 'Yes' : 'No'}`;
          popup += `<br/>Regenerative: ${loc.IsRegenerative ? 'Yes' : 'No'}`;
          if (loc.AgroPractices && typeof loc.AgroPractices === 'object') {
            popup += `<br/><em>Agroecological Practices:</em><ul>`;
            for (const [practice, desc] of Object.entries(loc.AgroPractices)) {
              popup += `<li><strong>${practice}:</strong> ${desc}</li>`;
            }
            popup += `</ul>`;
          }
        }

        if (loc.Type === "PickupPoint") {
          popup += `<br/>Label: ${loc.Label || "Unknown"}`;
          popup += `<br/>Opening Hours: ${loc.OpeningHours || "N/A"}`;
          popup += `<br/>Donations: ${loc.AcceptsDonations ? 'Yes' : 'No'}`;
        }

        if (loc.Website) {
          popup += `<br/><a href="${loc.Website}" target="_blank">Website</a>`;
        } else if (loc.Facebook) {
          popup += `<br/><a href="${loc.Facebook}" target="_blank">Facebook</a>`;
        }

        if (loc.GoogleMapsUrl) {
          popup += `<br/><a href="${loc.GoogleMapsUrl}" target="_blank">Google Maps</a>`;
        }

        marker.bindPopup(popup);
        marker.addTo(map);
      });
    })
    .catch(err => {
      console.error("Error fetching or processing data.json:", err);
    });
};
