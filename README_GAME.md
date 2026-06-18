# Unity prototype: Open-World Driving Game (Starter)

This repository contains a small Unity script scaffold to prototype a driving / drifting / crash mechanic, simple traffic AI, and a mission manager.

What I added
- Assets/Scripts/VehicleController.cs — player vehicle controller with drifting and basic crash detection.
- Assets/Scripts/MissionManager.cs — simple mission target system and UI hook.
- Assets/Scripts/TrafficSpawner.cs — spawns traffic prefabs at spawn points (see usage below).
- Assets/Scripts/SimpleWaypointAI.cs — lightweight AI that follows placed Waypoint objects.
- Assets/Scripts/Waypoint.cs — marker component for placing waypoints in the scene.

How to use (Unity 2020.3 LTS / 2021.x / 2022.x)
1. Open Unity Hub, create a new 3D project (LTS recommended) and open this repository folder.
2. Import standard packages as needed (Text UI, etc.).
3. Create a simple scene:
   - Terrain or a large Plane for driving surface.
   - Create an empty GameObject tagged "Player" and attach a car model to it.
   - Add a Rigidbody to the car root, then add WheelCollider components to four empty child GameObjects for wheels.
   - Attach the VehicleController script to the car root. Assign wheel colliders and visual wheel meshes in the inspector.
   - Add a ParticleSystem for skids and an AudioSource for crash SFX and wire them to the VehicleController.

4. Missions:
   - Create empty GameObjects as mission targets (name them Mission_01, Mission_02...).
   - Create a Canvas with a Text element and assign it to MissionManager.missionText.
   - Add the MissionManager component to an empty GameObject and populate the missionTargets list.

5. Traffic & Waypoints:
   - Create Waypoint objects (empty GameObjects with Waypoint component) forming a loop.
   - Create a traffic car prefab (simple model + Rigidbody). Add the SimpleWaypointAI component when spawning via TrafficSpawner.
   - Place spawn points as empty GameObjects and assign them to TrafficSpawner.spawnPoints.

6. Play the scene and use WASD / arrow keys to drive, Space for handbrake (drift).

Notes & next steps
- This is prototype-level code; for realistic handling consider integrating a vehicle physics asset (Edy's Vehicle Physics, Unity Vehicle Physics, or a custom tire model using PhysX).
- Add sound blending for engine pitch, skid/impact layering, tire marks (decals), camera shake, and damage visuals for polish.
- If you want, I can scaffold a Unity scene (.unity) and simple prefabs — that requires binary assets and is larger, I can add them if you confirm.

Would you like me to:
- Add a ready-to-open Unity scene + sample prefabs to this repo? (binary assets will increase repo size)
- Instead prepare an exported Unity package (.unitypackage) you can import locally?
- Or switch to Unreal (Blueprints) instead?
