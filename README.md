# Last-Tale-2D

## Project Overview

Last-Tale-2D is a 2D top-down action game created with Unity. In this game, players choose between two animal characters, a Fox and a Wolf, to fight against a challenging boss named Kolobok. The game features a preparation phase where players can gather resources and set up traps before the boss spawns.

### Key Features
- **Two Playable Characters**: Choose between the agile Fox with a stealth ability or the powerful Wolf with a charge attack.
- **Dynamic Boss Fight**: Face the formidable Kolobok, a boss with multiple stages and attack patterns.
- **Strategic Gameplay**: Utilize a preparation phase to gather resources, set traps, and plan your strategy before the fight begins.
- **Resource Management**: Collect branches to build traps and gain an advantage in battle.
- **Engaging Abilities**: Master unique character abilities and a shared trap-boosting skill to overcome challenges.

## Getting Started

To get started with this project, you will need to have Unity Hub and a compatible version of the Unity Editor installed.

### Setup

1.  **Clone the repository:**
    ```
    git clone https://github.com/your-username/Last-Tale-2D.git
    ```
2.  **Open the project in Unity Hub:**
    *   Open Unity Hub.
    *   Click on "Projects" > "Open".
    *   Navigate to the cloned repository's directory and select it.
3.  **Launch the Game**:
    * Once the project is open in the Unity Editor, locate the `MainMenu` scene in `Assets/Scenes`.
    * Press the "Play" button at the top of the editor to start the game.

## Gameplay

The game is divided into two main phases:

1.  **Preparation Phase:** Players have a set amount of time to explore the arena, collect resources (branches), and place traps to prepare for the boss fight.
2.  **Boss Fight:** After the preparation time is over, the boss (Kolobok) spawns, and the fight begins.

### Controls

*   **Move:** `WASD` keys
*   **Aim:** `Mouse`
*   **Attack:** `Left Mouse Button`
*   **Place Trap:** `F` key
*   **Character-Specific Ability:** `Q` key
*   **Trap Boost Ability:** `E` key

## Characters

There are two playable characters, each with a unique ability.

### Fox

The Fox is a nimble character with a stealth ability, perfect for players who prefer a tactical, hit-and-run playstyle.

*   **Ability (Stealth):** Press `Q` to become nearly invisible for a short duration. While in stealth, the boss cannot see the Fox, allowing you to reposition or avoid attacks.

### Wolf

The Wolf is a strong and aggressive character with a powerful charge ability.

*   **Ability (Charge):** Press `Q` to perform a high-speed charge, dealing significant damage to any enemy it hits and stunning them.

## The Boss: Kolobok

Kolobok is the main antagonist of the game, a fearsome boss inspired by a classic fairy tale character. The fight with Kolobok has two stages:

*   **Stage 1 (Rolling):** Kolobok rolls around the arena, attempting to ram the player.
*   **Stage 2 (Jumping):** When Kolobok's health drops below a certain threshold, it enters a more aggressive second stage. In this stage, Kolobok will periodically jump and create a shockwave, dealing area-of-effect damage.

Kolobok can be stunned by traps, providing a window of opportunity for players to deal damage.

## Code Structure

The main source code for the game is located in the `Assets/Scripts` directory. The code is organized into the following subdirectories:

*   `Boss`: Contains the AI and controller logic for the Kolobok boss.
*   `Environment`: Includes scripts for environmental elements like collectible resources and traps.
*   `Managers`: Contains the `GameManager` and other high-level manager scripts that control the game flow.
*   `Player`: Contains scripts related to the player characters, their abilities, and input handling.
*   `UI`: Contains scripts for managing all user interface elements, including the HUD and game over screen.
