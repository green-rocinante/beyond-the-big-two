window.onload = function () {
  if (typeof L === 'undefined') {
    console.error("Leaflet not found. Check loading order.");
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

  const markerCluster = L.markerClusterGroup();

  fetch('/data.json')
    .then(res => {
      if (!res.ok) throw new Error("Failed to fetch data.json");
      return res.json();
    })
    .then(data => {
      data.forEach(loc => {
        const icon = loc.Type === "Producer" ? producerIcon : pickupIcon;
        const marker = L.marker([loc.Latitude, loc.Longitude], { icon });

        let popup = `<strong>${loc.Name || loc.Type}</strong><br/>Postcode: ${loc.Postcode}`;

        if (loc.Type === "Producer") {
          popup += `<br/>Produce: ${loc.ProduceTypes?.join(', ') || ''}`;
          popup += `<br/>Organic: ${loc.IsOrganic ? 'Yes' : 'No'}`;
          popup += `<br/>Regenerative: ${loc.IsRegenerative ? 'Yes' : 'No'}`;

          if (loc.AgroPractices && Object.keys(loc.AgroPractices).length > 0) {
            popup += `<br/><em>Agroecological Practices:</em><ul>`;
            for (const [practice, desc] of Object.entries(loc.AgroPractices)) {
              popup += `<li><strong>${practice}:</strong> ${desc}</li>`;
            }
            popup += `</ul>`;
          }
        }

        if (loc.Type === "PickupPoint") {
          popup += `<br/>Label: ${loc.Label}`;
          popup += `<br/>Opening Hours: ${loc.OpeningHours}`;
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

        if (loc.DistanceFromUser !== undefined) {
          popup += `<br/>Distance: ${loc.DistanceFromUser.toFixed(1)} km`;
        }

        marker.bindPopup(popup);
        markerCluster.addLayer(marker);
      });

      map.addLayer(markerCluster);

      // Optional filter summary overlay
      const summary = `
        <div style="background:white;padding:6px 8px;border-radius:4px;line-height:1.4;font-size:13px;">
          <strong>Filter Summary:</strong><br/>
          Max Distance: (based on user input)<br/>
          Organic / Regenerative filters applied<br/>
          Donation pickup points included
        </div>
      `;

      L.control({ position: 'topright' }).onAdd = function () {
        const div = L.DomUtil.create('div');
        div.innerHTML = summary;
        return div;
      }.addTo(map);
    })
    .catch(err => console.error("Error loading data:", err));
};
