# LIV for Gorilla Tag

![image](https://user-images.githubusercontent.com/3955124/166721440-211e687e-5b13-43bc-924f-0262e22074b3.png)


Adds [LIV](https://store.steampowered.com/app/755540/LIV/) support to Gorilla Tag (PCVR only, no Quest Support for now). With it, you can use mixed reality capture (video where you have your real body inserted into the game), avatars (custom 3D avatars with a dynamic camera that can be in third person, while you play the game normally in first person), smoothed first person view, and more.

![Download count](https://img.shields.io/github/downloads/LIV/GorillaTagLIV/total?style=flat-square)

## Easy Installation (Recommended)

- Download the [Monke Mod Manager](https://github.com/DeadlyKitten/MonkeModManager/releases/latest).
- Select "LIV" from the mod list;
- Press "Install / Update". It should install the LIV mod and all its dependencies.

## Manual Installation (Harder)

- Download the [Monke Mod Manager](https://github.com/DeadlyKitten/MonkeModManager/releases/latest).
- Install BepInEx and Utilla using the Monke Mod Manager.
- [Download the latest release of the LIV mod](https://github.com/Raicuparta/GorillaTagLIV/releases/latest).
- Extract it to the Gorilla Tag folder (usually something like `C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag`).

## Usage

You need [LIV](https://store.steampowered.com/app/755540/LIV/) installed and running to use this mod. Run LIV, and then run the capture tool:

![image](https://user-images.githubusercontent.com/3955124/166646386-4aaf8292-cc28-4e34-bdae-d81c8147693e.png)

You'll also have to configure LIV with either a real camera (for mixed reality) or a virtual camera (for 3D avatars). [Check the LIV documentation to learn how to set everything up](https://help.liv.tv/hc/en-us/categories/360002747940-LIV-Setup).

If the LIV capture tool was already running when you installed the Gorilla Tag LIV mod, restart it. Gorilla Tag should now show in the game selection dropdown, in the "Auto" tab:

![image](https://user-images.githubusercontent.com/3955124/165312088-de5c8fb9-5361-4f94-b329-a0ec12876940.png)

Then you can press "Sync & Launch" in the same tab to start the game.

If you can't get this to work, try the "Manual" tab instead. Start Gorilla Tag via Steam, and select the Gorilla Tag exe (while the game is running):

![image](https://user-images.githubusercontent.com/3955124/165311810-d9b8e4ec-7c35-4a75-8d3a-a33c3a579188.png)

## Known problems and reporting bugs

Check the [issues list](https://github.com/Raicuparta/GorillaTagLIV/issues) for known bugs. Create a new issue if you run into problems.

## Development setup

- Download the [Monke Mod Manager](https://github.com/DeadlyKitten/MonkeModManager/releases/latest).
- Install BepInEx and Utilla using the Monke Mod Manager.
- [Install the Unity version used by the game](https://unity3d.com/get-unity/download/archive). `2019.3.15` at the time of writing this, but you should check if the version changed in a game update.
- Open the `GorillaTagLIVUnity` project in the Unity Editor.
- Build the project to the `/Build` directory.
- Open `GorillaTagLIV.sln` in your IDE (Visual Studio, JetBrains Rider, whatever).
- Open `GorillaTagLIV/Directory.Build.props` and edit the `<GamePath>` property to point to your Gorilla Tag installation folder. Or better yet, make a .user file to override the property without modifying the file.
- Try building the project:
  - If you build the Debug configuration, the mod will be placed in Gorilla Tag's BepInEx plugin folder.
  - If you build the Release configuration, the mod will be placed in the `Build` folder at the root of the solution. To make a release, simply zip up the contents of this folder. Check previous releases to make sure you use the same format, it needs to stay compatible with Monke Mod Manager.