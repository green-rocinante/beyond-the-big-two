# 🌱 Beyond the Big Two

**Beyond the Big Two** is a prototype tool for helping users discover local producers and community food pickup points across Victoria, Australia, outside the major supermarket duopoly. 

This version of the system is focused on a data-driven map interface that shows food producers, local pickup points, and community food events. Users can search or filter by region, food type, and transport radius. Producers can be registered by community members or themselves.
The system design focuses on supporting extensibility - eventual plans include real-time supply updates, user-contributed reviews, and regenerative farming indicators - but the current design covers the basics of spatially mapping and classifying sources of food.

It was developed with the two-birds-one-stone principle in mind: to design a tool that would help address the current food system crisis in Victoria, and to contribute to assessment tasks for SIT771 (Object-Oriented Development) at Deakin University.

---

## What the Program Does

- Filters and displays **local producers** and **pickup points** based on user-defined criteria (postcode, radius, organic/regenerative practices, etc.) in a console window
- Visualises filtered results directly in a terminal window and generates a **JSON dataset**
- Hosts an interactive **map interface** to display the results using Leaflet.js
- Offers two modes:
  - **Dynamic map generation via a local C# server**
  - **Static map prototype viewable on GitHub Pages**  
    ➡[View Demo](https://green-rocinante.github.io/beyond-the-big-two/map.html)

---

## Object-Oriented Design Overview

The system was designed using clear **OOP principles**:

### 1. **Encapsulation**
- Each class (e.g. `Producer`, `PickupPoint`, `UserFilter`, `FoodMap`) manages its own data and behaviour
- User filters are encapsulated in the `UserFilter` class and passed to the `FoodMap` for filtering

### 2. **Abstraction**
- The abstract base class `Location` defines shared attributes (`Latitude`, `Longitude`, `Postcode`, etc.)
- Subclasses such as `PickupPoint`, `Producer`, and `LocationConversion` override specific behaviours (e.g. how they match filter criteria)

### 3. **Inheritance**
- `Producer` and `PickupPoint` both inherit from the `Location` class
- This allows shared methods (e.g. `GetDistanceTo()`) and simplifies filtering/sorting

### 4. **Polymorphism**
- Filtering logic uses the `IFilterable` interface so that any type (Producer or PickupPoint) can be matched using the same `MatchesFilter()` method
- A custom `LocationConverter` enables polymorphic JSON serialization of different location types

---

## Map Output Options

### Option 1: Static Map (Preview Only)

You can view a **read-only demo** of generated test data hosted on GitHub Pages:

[View Static Map Demo](https://green-rocinante.github.io/beyond-the-big-two/map.html)

> This map is generated from pre-generated JSON data and does not support dynamic filtering.

---

### Option 2: Dynamic Map via Local C# Server

To use the dynamic filtering system and generate live data for the map:

1. Clone the repo and run the C# program using SplashKit
2. Click the **Start Server** button in the app window
3. The app will generate a `data.json` file and open `map.html` in your browser
4. The HTML/JS frontend will fetch the generated data and render pins dynamically
5. Click the **Refresh map** button to refresh data after new filters have been applied. Refresh the browser. 

> This version supports real-time filtering based on postcode, radius, and food system flags (e.g. organic-only, donation-friendly).

---

## Tech Stack

- C# + SplashKit SDK
- JSON serialization
- Leaflet.js (for maps)
- GitHub Pages (static hosting)

---

## Author

Developed by Ivy Craw
For Deakin University – SIT771 Object-Oriented Development (T2 2025)

