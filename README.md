# JUMP PEPE
Jump Pepe is a 3D platformer in the style of Only Up, where you have to complete a set level of platforms to unlock the next one. It currently has 4 levels, with increasing difficulty and different colors, so it's easy to differentiate them.

---

## Main features

- Fully functional 3D platformer with:
  - Dash
  - Double jump
  - Flight (as a cheat)
  - Custom Camera
- Fully animated
- Cheats toggle system (speed up, flight, teleport)
- Multiple levels loaded additively
- Playeable with **keyboard** and **gamepad**

---

### Scene & Navigation Flow

- The project starts with a **Boot** scene.
- Scenes are managed by a persistent `SceneController` using additive and asynchronous loading/unloading.

### Menus

- Main Menu, Pause Menu, and Credits are in the **same scene** (`Menus`).
- Fully navigable using **keyboard, mouse, or controller**.


### Game Patterns
- Singletone
- State Machine

---

## 🎮 Controls

| Action         | Keyboard / Mouse             | Gamepad            |
|----------------|------------------------------|---------------------|
| Move           | WASD / Arrow keys            | Left Stick          |
| Jump           | Space                        | A / Cross           |
| Dash           | Left Shift                   | L3                  |
| Fly Up / Down  | Space / Control (cheat only) | B / Circle          |
| Pause          | Escape                       | Start / Menu        |
| Navigate Menus | WASD / Arrow Keys / Mouse    | Left Stick          |
| Select Button  | Click / Enter                | A / Cross           |

---

## Where to play
- Itch.io: 

---

## Technical Highlights

- **Additive scene managment**
- **Centralized input and scene flow**
- **Fully animated**
- **Dynamic camera system**

---

## 📸 Screenshots

---

## ✨ Credits

Developed by **Nicolas Leon**    
Professor: **Juan Pablo Varela**
Game Assets:
- RGS_Dev: https://rgsdev.itch.io/free-3d-modular-low-poly-assets-for-prototyping-by-rgsdev
- Kenney: https://kenney.nl/assets/city-kit-commercial
- Kay Lousberg: https://kaylousberg.itch.io/kaykit-platformer
- Mixamo: https://www.mixamo.com

Sound Effects:
- https://assetstore.unity.com/packages/audio/sound-fx/voices/effort-sounds-male-npc-player-audio-pack-285382
- https://assetstore.unity.com/packages/audio/sound-fx/free-ui-click-sound-pack-244644
- https://assetstore.unity.com/packages/audio/sound-fx/foley/footsteps-essentials-189879
- https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116
