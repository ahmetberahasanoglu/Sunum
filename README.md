# ⚔️ AI-Assisted FPS Prototype

![Unity](https://img.shields.io/badge/Unity-2023%2B-black?style=flat&logo=unity)
![C#](https://img.shields.io/badge/C%23-Scripting-blue?style=flat&logo=csharp)
![AI](https://img.shields.io/badge/AI-Assisted-orange?style=flat&logo=openai)
![Status](https://img.shields.io/badge/Status-Prototype-green)

> **"A vertical slice FPS project demonstrating the power of AI in rapid game development mechanics."**

![Gameplay Demo](8.png)


## 📖 About The Project

This project is a high-mobility First Person Shooter (FPS) prototype developed to showcase **AI-Assisted Game Development** workflows. 

Inspired by *Attack on Titan* and Dani's *Karlson*, the game features physics-based movement, a grappling hook system, and a giant AI enemy with dynamic hitboxes. The core goal was to utilize Large Language Models (LLMs) to accelerate the scripting of complex mechanics, proving that AI can act as a "Technical Co-Pilot" for developers.

### 🎮 Key Features
* **Physics-Based Movement:** Custom Rigidbody controller featuring sliding, crouching, and momentum preservation.
* **Grappling Gun System:** Raycast and SpringJoint based mechanics allowing dynamic swinging and verticality.
* **Wall Run & Parkour:** Seamless wall running and wall jumping mechanics with camera tilt effects.
* **Titan AI (The Enemy):** * NavMesh-based pathfinding.
    * State Machine (Chase/Attack logic).
    * **Dynamic Hitboxes:** Specific limb damage (Headshots deal 2x damage).
    * **Ragdoll Physics:** Transitions from animated state to physical ragdoll upon death.
* **Combat Feel (Game Juice):**
    * Procedural Weapon Recoil (Position & Rotation).
    * Screen Shake & FOV effects.
    * Speed Lines VFX based on player velocity.
* **Resource Management:** Ammo system with UI integration and pickup mechanics.
* **Time Manipulation:** "Slow-Motion" ability for cinematic combat moments.

## 🤖 The AI Workflow

This project was built to demonstrate how AI tools (ChatGPT/Gemini/Claude) can be integrated into the Unity workflow. 

* **Script Generation:** 80% of the C# scripts (Movement, AI Logic, Gun System) were generated via prompts and refined manually.
* **Debugging:** AI was used to solve vector calculation errors in the grappling mechanic.
* **Visual Assets:** UI elements (Win Panel, Restart Button) were conceptualized using Midjourney prompts.

## 🕹️ Controls

| Key | Action |
| :--- | :--- |
| **W, A, S, D** | Movement |
| **Space** | Jump / Wall Jump |
| **Left Ctrl** | Crouch / Slide (Builds speed on slopes) |
| **Left Click** | Grapple Hook |
| **Right Click** | Shoot |
| **E** | Dash (Directional) |
| **Q** | Slow Motion (Hold) |
| **R** | Reload |

## 🛠️ Built With

* **Engine:** Unity 6 (or your specific version)
* **Language:** C#
* **Assets:** * *Mixamo* (Character Models & Animations)
    * *Midjourney* (UI Concepts)
    * *TextMeshPro* (UI Text)

## 🚀 Installation & How to Run

1.  Clone the repo:
    ```sh
    git clone [https://github.com/ahmetberahasanoglu/Sunum.git](https://github.com/ahmetberahasanoglu/Sunum.git)
    ```
2.  Open **Unity Hub**.
3.  Click **Add** and select the cloned folder.
4.  Open the project and load the `MainScene` from the `Scenes` folder.
5.  Press **Play**!

## 📂 Project Documentation

You can examine the detailed project presentation regarding the AI workflow and game design process below:

[![View Presentation](https://img.shields.io/badge/View_Presentation-PDF-red?style=for-the-badge&logo=adobeacrobatreader)](Docs/how.pdf?raw=true)


---
*Note: This project was created for an educational presentation on "Game Development with Artificial Intelligence".*
