# Bubbles

## Scenes

All scenes are saved in the `Scenes` folder in Assets/

### Menu

`Menu` is the main scene. It is based on `Imports/Game Jam Template Unity` asset from the Asset Store and its tutorial.

- The button objects are in the `Menu UI`.
- Music is stored in the Music folder.
- `Start Options (Script)` attached to `MenuUI` uses a Menu Settings (MenuSettings) ScriptableObject. If you want to adjust the scene transitions, change them there.
- `Play Music (Script)` attached to `MenuUI` also uses the Menu Settings (MenuSettings) ScriptableObject. If you want to change the music settings, change them there.
- If you want to adjust button colors, change them in the `Menu UI -> Menu Panel -> MenuButtons -> StartButton`. You will find a script called `Flexible UI Button (Script)` with a variable called `Flexible UI Data - Game Jam Menu UI Data (FlexibleUIData)`. You can change the colors in that scriptable object.
- How the Menu works is that it uses the `File -> Build Settings`. The Menu is SCENE 0. It will transition to whatever is (Add Open Scenes) to SCENE 1.

###  Game

Both the `Menu` and `Game` scene take from the `Window -> Lighting -> Settings`. In Skybox Material, we are using a material called `CustomSky`. You can change the sky settings in CustomSky by clicking on it.

- Main Camera. The `Main Camera` uses a `Fly Camera (Script)`. The Fly Camera (Script) allows the player to use WASD to move around. The camera will follow the mouse. R and F are used to move vertically (not forward). The `Post Processing Behaviour (Script)` takes advantage of the `PostProcessing Unity Asset` from the Asset Store. It makes things look nice using a PostProcessingProfile found in the Profiles folder.
- Directional Light. All we did with the Directional Light is change the rotation just a little bit to make the shadows on the Scene not too invasive. We baked the lighting from the Lighting tab. The baked lighting created a Game folder in the Scenes folder.
- Network Manager. Network Manager uses the regular `Network Manager HUD`, so that we can choose between a Host or a Client. The `Network Manager (Script)` has only 3 main changes. There's a `Camera Player (Prefab)` in the Player Prefab option. The Camera Player attaches Fly Camera (script) and is created when a player joins. It allows each player to move their camera. `2 Spawnable Prefabs` are `Bubble` when the Camera shoots a Bubble at players. And a `SpritePlayer`, which is the player seen on the map.
- Floor. Floor takes advantage of some models in the `Imports/Kenney Assets` folder. The `Floor -> DeadZones` use Box Colliders with a tag called `DeathZone`. The DeathZone will kill a player if the SpritePlayer touches the collider.
- Announcement. We don't really use this GameObject yet. We were planning on putting text countdowns in the `Announcement -> MainMessage` object. MainMessage takes advantage of a useful Unity Asset called `Imports/TextMesh Pro`. A Text Mesh Pro(Script) text has better looking text than the default Unity text. This MainMessage GameObject also uses a script called `Billboard (Script)`, which makes the words always face the Camera.
- Managers. Managers uses a script called `Logic Manager (Script)`. This script is a placeholder to store information about the `SpritePlayer (Script)` when the SpritePlayer gets spawned. For example, I join the game. There are 2 SpritePlayer Prefabs because there are 2 players on the map. I want to store a quick value to determine which player is me, so we store it in the `Logic Manager (Script)`.

### Other Scenes

Make new scenes to test.

- `MobileGameTest` is a scene that we're testing for ARCore.
- `TestScene` is our original scene that we were testing for the game.

## Scripts

All of our saved objects are stored in the PreFabs folder. PreFabs are just saved objects that we can create into the Scene. For example, we want to save a `Prefab called SpritePlayer` because we always create new Players whenever they join the game. Use Prefabs for that sort of stuff.

- `Billboard` makes the attached GameObject always face the camera.
- BubbleController. A player launched a `Bubble (Prefab)` whenever the player hits the Space Bar. The attached script to the Bubble (Prefab) is called `BubbleController`. All we do with BubbleController is increment the number of bubbles we have launched. When the Bubble is destroyed (lasts for 8 seconds), we decrement the number of Bubbles that we have launched by 1. BubbleController uses `OnDestroy` Unity function and decrements the number of bubbles.
- BubbledController. Whenever a player gets hit by a Bubble, we created a `Bubbled (Prefab)`. The Bubbled (Prefab) is very similar to the Bubble (Prefab). They look almost exactly alike except `BubbledController` is attached to it. We just make sure that it ignores other Bubbled effects because the Bubbled (Prefab) has a Tag attached to it underneath its name called `BubbledTag`. Example. Player 1 gets Bubbled. Then, Player 2 gets Bubbled next to Player 1. We want these bubbles to not collide with each other.
- VisualBubbleController. `VisualBubbleController` is attached to the `VisualBubble child of Bubbled (Prefab)`. It contains the visual effect of a bubble. We attach this script to the VisualBubble child of the Bubbled (Prefab) because whenever a SpritePlayer (Prefab) is hit by a bubble, we create a Bubbled (Prefab), and we want it to hover over the arena until it crashes into the DeadZone in the Floor (GameObject) in the Scene, so the script handles the logic for the visual aspect of the floating.
- FlyCamera. `FlyCamera` is attached to the `Main Camera` in the `Game` Scene. It just adds the WASD controls to move around the map and adds the mouse follow.
- GameManager. We're not using this script at the moment. We were thinking of using it for CountDown and a Lobby system or something. Ready up system in the future.
- LocalNetworkDiscovery. We're not using this script at the moment. We were thinking of using it to determine a Host or Client without using the Network Manager HUD.
- LogicManager. `LogicManager` is attached to the `Managers GameObject` in the `Game` Scene. It is used to store values like which SpritePlayer(Prefab) am I or which CameraPlayer am I? We were planning on using this to create a different camera depending on if you were a PC or Android or iPhone user, but we haven't got that far yet.
- NetworkedPlayerState. `NetworkedPlayerState` is attached to the `CameraPlayer (Prefab)`. We are using this as a way to send information to all the Clients or Server. UNET requires the GameObject to have Local Authority, which in our case is CameraPlayer, so we put all of our NetworkingStuff in here. For example, the SpritePlayer (Prefabs) are moving? Update the movements with the NetworkedPlayerState (Script)!
- PlayerController. `PlayerController` is attached to the `CameraPlayer`. It adds the logic for the max number of bubbles we can shoot and using the KeyCode.Space button to shoot new Bubbles.
- SoundManager. We plan on using SoundManager for sound effects. We haven't got to it yet.
- SpriteController. `SpriteController` is attached to our `SpritePlayer (Prefab)`. We use this script to control the movement of our SpritePlayer (Prefab) and check if it has hit either the `Bubble(Clone)` or DeathZone. If you hit a Bubble, turn off gravity is get trapped in a bubble. Hit any key a lot of times to free yourself and restore gravity. If you hit a `DeathZone`, just destroy the SpritePlayer (Prefab) with `Destroy(gameObject)`, which represents itself.

## Miscellaneous

- Some other bubble and zap effects are stored in `Imports/FX Mega Pack` called HuyBubbleBomb, HuySplash, and HuyZap.
