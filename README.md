# Korlátko Castle – Interactive Visualisation (Unity)

Interactive 3D visualisation of **Korlátko Castle** in the Little Carpathians, developed in **Unity** as part of the **MTMP course – Multimedia and  section** at FEI STU.

The project combines historical context, terrain realism, and interactive exploration in a low-poly 3D environment.

---

## Project Overview

This project represents an **interactive showroom / visualisation** of Korlátko Castle, focusing on:

- Accurate spatial placement of the castle ruins
- Realistic surrounding terrain (Little Carpathians, Záhorie region)
- Combination of **current ruins** and a **historical reconstruction (circa 1650)**

The application allows users to freely explore the environment, access contextual information, and interact with individual parts of the castle.

---

## Key Features

- **3D low-poly visualisation** of the current castle ruins
- **Historical reconstruction (1650)** based on sketches by P. Gutek and M. Šimkovič
- Detailed **terrain and environment** of the surrounding landscape
- Interactive **information markers** for castle parts
- Historical descriptions and legends based on official municipal sources
- **Fly-Camera mode** for free movement
- **Day / night cycle** with real-time lighting changes

---

## Controls & Usage

An in-application help menu is available via the **question mark (?) button**.

### Camera Controls
- **WASD** – movement
- **Mouse** – rotation
- **Space** – vertical movement
- **Left Mouse Button** – interaction with markers

### UI Options
- Show / hide information markers
- Switch between historical and current visualisation
- Toggle shadows
- Adjust day/night time

---

## Project Structure
```
├── Models/ # Custom 3D models, textures, materials
│ └── korlatka.fbx # Castle ruins model (created in SketchUp)
├── StreamingAssets/ # Runtime-loaded assets
│ └── info_en.json # Castle information (loaded at runtime)
├── Scripts/
│ ├── CameraController.cs # Fly-camera movement
│ ├── UiManager.cs # UI logic
│ ├── ShowroomManager.cs # JSON loading, UI logic
│ ├── LightningPreset.cs # ScriptableObject for lighting setup
│ ├── WorldLights.cs # Day/night cycle logic
│ └── WaypointMarker.cs # Interactive UI markers
├── Scenes/
│ └── main.unity # Main visualisation scene
└── ...
```

Other folders contain free to use third-party assets imported from the Unity Asset Store.

---

## Implementation Notes

- **Lighting system** uses a `ScriptableObject` (`LightningPreset`) to smoothly interpolate:
  - Light color
  - Fog color
  - Light intensity during day/night transitions
- **Information system** loads structured data from `info_en.json` at runtime
- **Waypoint markers** dynamically hide when outside the camera view
- Marker interactions display contextual information for specific castle parts

---

## Assets & Resources

The project uses several **free assets** from the Unity Asset Store, including:

- Medieval Castle – Modular
- Medieval Stone Keep
- House Pack
- SIMPLE FANTASY GUI
- Waypoint Marker System (UI only)
- Medieval Music Pack Vol. 2
- Vegetation and terrain assets

Post-processing effects are used via Unity Registry packages.

---

## Academic Context

- **Course:** MTMP – Multimedia and Telematics for Mobile Platforms  
- **Institution:** Faculty of Electrical Engineering and Informatics  
- **University:** Slovak University of Technology in Bratislava (FEI STU)

This project was submitted as a **practical exam assignment**.

---

## Author

**Bc. Juraj Hušek**  
Faculty of Electrical Engineering and Informatics  
Slovak University of Technology in Bratislava (FEI STU)

---

## License

This project is publicly available for educational and demonstrational purposes.  
Asset licenses remain subject to their original authors.

