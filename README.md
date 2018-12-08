# Sub Sinker
This game is a collaboration between imfrendy (Judah) and FGVel0city (KV) for the 2017-2018 SkillsUSA Game Design competition. Combining strategy, stealth, and action, this deceptively difficult game challenges players to balance risk and reward in order to destroy their opponents.

### How to install and run:
Download this repo and open the project in Unity. Then, build it and run the executable it creates.

### How to connect:
Sub Sinker is a LAN Multiplayer game that requires multiple machines connected on a network. Click "Play" on the main menu and on your host machine click "Host". On the same machine, open Command Prommpt and type `ipconfig`. On any other machines, replace the dialog box that reads "localhost" and replace it with the IPv4 address listed in the Command Prompt, then click LAN Client. This should connect the machine to the host.

### Controls:
The controls are simple, but mastering the game is difficult.

WASD: Pilot your sub

Scroll wheel: Change the level your engine is running at (higher levels will reveal more of your surroundings and will increase your speed, while making you more easily spotted)

Left click: Fire a torpedo

Right click: Fire a SONAR ping, illuminating the walls it bounces off

### Gameplay:
Collect health kits (tools) and ammo (torpedos) around the map.

Watch your outer blue ring! Enemies who come within that range can spot your engine.

Jellyfish (currently model-less) can illuminate the game world slightly.

Use the grid in the bottom right to remember the layout of the trenches.

### Debug build only
Press F1 to self-destruct.

Press F2 to provide global illumination.

Press F3 to reveal the whole map.


## To Do/issues:
Refactor cooldowns with IEnumerators instead of the mess it currently is

Add new movement options like a quick dash or boost

Improve visuals and add to environment (i.e. implement jellyfish model)

Replace network UI and allow for custom ports

Improve map texture and allow it to be dynamic/tile, as well as texture the sides of walls

Map generator flips width and height (currently UI fix)

Allow radar to stretch properly with non-square dimensions

Allow players to bounce off walls without screwing up other stuff

Physics seem to affect host and clients differently

Improve ping indicator (lol)