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
- Menu navigation is handled with Events


### Game Patterns
- Singletone
- State Machine

---

##  Controls

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
- Itch.io: https://leon-05.itch.io/jump-pepe

---

## Technical Highlights

- **Additive scene managment**
- **Centralized input and scene flow**
- **Fully animated**
- **Dynamic camera system**

---

##  Screenshots
![](https://img.itch.zone/aW1hZ2UvMzcwNDI1Ni8yMjE2MDM5OS5wbmc=/original/i%2Fft%2Ba.png)
![](https://img.itch.zone/aW1hZ2UvMzcwNDI1Ni8yMjE2MDQwMC5wbmc=/original/UMsqDp.png)
![](https://img.itch.zone/aW1hZ2UvMzcwNDI1Ni8yMjE2MDQwMS5wbmc=/original/npm2O2.png)
![](https://img.itch.zone/aW1hZ2UvMzcwNDI1Ni8yMjE2MDQwMi5wbmc=/original/aYs%2Fto.png)

---

##  Credits

Developed by **Nicolas Leon**    
Professor: **Juan Pablo Varela**
Game Assets:
- Keys by RGS_Dev: https://rgsdev.itch.io/free-3d-modular-low-poly-assets-for-prototyping-by-rgsdev
- Buildings by Kenney: https://kenney.nl/assets/city-kit-commercial
- Platforms by Kay Lousberg: https://kaylousberg.itch.io/kaykit-platformer
- Character model and animations by Mixamo: https://www.mixamo.com
- Skybox by Avionx: https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633

Sound Effects:
- https://assetstore.unity.com/packages/audio/sound-fx/voices/effort-sounds-male-npc-player-audio-pack-285382
- https://assetstore.unity.com/packages/audio/sound-fx/free-ui-click-sound-pack-244644
- https://assetstore.unity.com/packages/audio/sound-fx/foley/footsteps-essentials-189879
- https://assetstore.unity.com/packages/audio/sound-fx/free-casual-game-sfx-pack-54116
