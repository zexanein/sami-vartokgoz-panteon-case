# Panteon Case Project

![gameplay](https://github.com/user-attachments/assets/b5f35288-709e-453d-8e5f-2d701690bed1)

This is a case project developed for **Panteon Games** as part of the recruitment process.  
It demonstrates a modular, scalable architecture for a tile-based strategy/management game built in **Unity** using **best practices** such as:

- **MVC UI architecture**
- **ScriptableObject-driven data**
- **Grid-based building placement**
- **Selectable soldiers with pathfinding AI**
- **Dynamic UI and object pooling**
- **Modular combat and unit production system**

---

## Project Features

- Grid-based building placement with preview and validation  
- Production system
- Selectable units/buildings with dynamic info panel
- Custom A\* pathfinding system tailored for a tile-based environment
- Attack mechanics via `IAttacker` and `IDamageable` interfaces  
- Object pooling for performance-friendly UI and runtime elements  
- Clean MVC separation for UI elements  
- Easily expandable and editor-friendly design

---

📁 Project Structure

```bash
Fonts/                             # Contains font files used throughout the project.
Prefabs/                           # Contains Prefab files of Game Objects
├── Buildings/                     # Prefabs related to buildings and structures.
├── UI/                            # UI-related prefabs
└── Units/                         # Prefabs for units
Presets/                           # Unity preset files for easy importing
Resources/                         # Assets accessible via Resources.Load at runtime.
├── GameElements/                  # Blueprints of game elements
│   ├── Buildings/                 # Blueprints of buildings
│   └── Units/                     # Blueprints of units
└── UnitProductionDatas/           # Contains Production Datas for UnitSpawnerBuildings
Scenes/                            # Contains all Unity scene files
Scripts/                           # Contains all C# scripts used in the project.
├── Blueprints/                    # Logic for blueprint data system.
├── CombatSystem/                  # Scripts related to combat mechanics and damage handling.
├── Extensions/                    # Helper extensions
├── GameElements/                  # Scripts related to core game entities like buildings and units.
├── Input/                         # Input management scripts
├── Pathfinding/                   # Pathfinding algorithms and navigation logic.
├── PlacementSystem/               # Logic for placing objects in the world
│   └── TilemapLayers/             # Tilemap layers used in the placement system.
├── Pooling/                       # Object pooling system to improve performance.
└── UI/                            # All UI-related scripts.
    ├── Controllers/               # Handle logic and behavior of UI components.
    ├── Decorators/                # Add visual or functional decorators to UI elements.
    └── Views/                     # Visual representations and elements of the UI.
Settings/                          # Project settings files
Sprites/                           # 2D sprite assets
TextMesh Pro/                      # Fonts and assets specific to TextMesh Pro.
TilePalettes/                      # Tile palette files used in the tilemap editor.
Tiles/                             # Individual tile assets used in tilemaps.
```

🛠️ How to Run / Test
- Clone or download the project
- Open in Unity 2021.3+
- Open Scenes/DevelopmentScene
- Press Play
- Left-click to place a building, right-click to cancel
- Select a unit and right-click to move or attack
- Select a building to access production UI (if available)
