# PeakAMap

**Find out what biomes are currently in rotation and select a map to play with an in-game UI.** 

PeakAMap searches through each map to find out what biomes they have and lets players pick from them. **There shouldn't be, but there may be inaccuracies.** For performance reasons, the loading and searching is done beforehand and saved in a file included with PeakAMap. If this file is missing or a new patch happens, then PeakAMap will automatically create this file when the game starts. _For slower PCs, this process may take 2-4 minutes._

#### **Currently Supports:** PEAK 1.54.c

## Features

- **Use a UI to Select a Map to Play**
	- Biomes of each map are displayed.
	- Choose to play a custom map or to play the daily map.
  
  <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-dropdown-english.png?raw=true" width=800px>

- **Biomes are Displayed in the HUD During Your Run**
  
  <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-english.png?raw=true" width=800px>

	> [!TIP] Tip: This feature is optional.
  If you don't like having extra text on your screen, this feature can be disabled in your `.cfg` file. Instructions on how to do this is [further below](#configuration).

- **Map Rotation Info is Preloaded for the Suppported PEAK Version**
  - Currently supporting PEAK 1.54.c
  
- **Support for Future Map Patches** 
  - PeakAMap *SHOULD* still display correct information when PEAK patches in new maps.
  - If PeakAMap doesn't have data for the current patch or there are issues with the file containing this data, the map rotation info will be loaded, searched, and saved at the game's start
      > [!TIP] Tip: This feature is optional.
      If you're only interested in selecting a map without the biome info or if preloading takes too long. This feature can be disabled in your `.cfg` file. Instructions on how to do this is [further below](#configuration).

- **Partial Language Support** 
  - Some phrases/words that were already supported in game will be supported in PeakAMap.

- **Biome Variant and Open Tomb Info**
  - Extra information on current maps are stored in the `.\BepInEx\plugins\PeakAMap\data\` folder in `map_rotation-<version>.json`. *(Note: The `<version>` refers to the **PEAK** game version, not PeakAMap mod version).*
    > [!NOTE] Note: Only available inside of file. Not shown in game.
    Currently, you can only view the **biome variant** and **open tomb** information in the **PeakAMap's data file**. _There are plans to add a way to display these in game, but it is not supported at the moment._
  
    > [!CAUTION] Caution: DO NOT modify these files.
    Doing so may cause inaccurate biome information to display in the PeakAMap UI or preloading to start due to failure to read the file.

## Installation

### Method 1: Using a Mod Manager

1. Install a **Thunderstore**-compatible mod manager, such as [**r2modman**](https://thunderstore.io/c/peak/p/ebkr/r2modman/) or [**GaleModManager**](https://thunderstore.io/c/peak/p/Kesomannen/GaleModManager/).
2. Search for **BepInExPack_PEAK** in your mod manager app and install OR go to [BepInEx](https://thunderstore.io/c/peak/p/BepInEx/BepInExPack_PEAK/) in Thunderstore and click "Install with Mod Manager" > "Open" > "Install".
3. Search for **PeakAMap** in your mod manager app and install OR go to [PeakAMap](https://thunderstore.io/c/peak/p/snowybunny/PeakAMap/) in Thunderstore link and click "Install with Mod Manager" > "Open" > "Install".
4. Make sure both mods are enabled and run **PEAK** from of your **mod manager** and play.

### Method 2: Manual Install

1. Opening `PEAK` game folder:
   - Open Steam > "Library" > "PEAK" > gears/settings icon > "Manage" > "Browse local files"  
   - This should open the `PEAK` folder. **Keep this folder opened as you continue**.
  
      <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/peak-folder.png?raw=true" width=450px>

1. Installing **BepInEx**:
   - Go to [BepInEx Thunderstore page](https://thunderstore.io/c/peak/p/BepInEx/BepInExPack_PEAK/) > "Manual Download".
   - This downloads a `.zip` file. Navigate to this (likely in your "Downloads" folder).
   - Extract the `.zip` file. (**DO NOT extract inside of the `PEAK` game folder.**)

      <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/extract-bepinex.png?raw=true" width=450px>

   - Open the extracted folder (if it didn't automatically open).
   - Go to `BepInExPack_PEAK` > Select all its contents > Move everything to the `PEAK` game folder you kept open in step 1.

      <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/move-bepinex-to-peak-folder.png?raw=true" width=900px>

   - The final `PEAK` folder should similar to the following: 

      <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/final-peak-folder.png?raw=true" width=450px>

   - Now, in the `PEAK` folder, go to `BepInEx` > `plugins`. **Keep this folder opened as you continue**. 

  
2. Installing **PeakAMap**:
   - Go to [PeakAMap Thunderstore page](https://thunderstore.io/c/peak/p//) > "Manual Download".
   - This downloads a `.zip` file. Navigate to this (likely in your "Downloads" folder).
   - Extract the `.zip` file.

      <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/extract-peakamap.png?raw=true" width=450px>

   - Open the extracted folder (if it didn't automatically open).
   - Go to `plugins` > Move `PeakAMap` folder into the `PEAK/BepInEx/plugins` folder you kept open in step 2.

      <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/move-peakamap-to-peak-folder.png?raw=true" width=900px>

   - The final `PEAK/BepInEx/plugins` folder should similar to the following _(there will be more files/folders if you have more mods installed)_: 

      <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/final-peakamap-path.png?raw=true" width=900px>

3. Run **PEAK** from **Steam** and play.

## Configuration:

With the `.cfg` file, you can:
- Disable or enable biomes from showing in the HUD.
- Disable or enable map rotation info from loading at the game's start when data files are missing.

### Instructions: 

- Run PEAK with PeakAMap once and quit game.
- Edit the `.cfg` file one of two ways:
  1. If you installed **PeakAMap** with a **mod manager**, you can edit the `.cfg` file with your mod manager app.
  2. If you installed **PeakAMap** **manually**:
     - Open Steam > "Library" > "PEAK" > gears/settings icon > "Manage" > "Browse local files" > `BepInEx` > `config`
     - Open `com.github.snowybunny.PeakAMap.cfg`.
     - Set the values to "true" or "false" according to your preference.
- If your game is still running. **Quit** and **relaunch** to apply settings.

## Extra Screenshots:

#### Loading screen when data is missing:

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/loading-maps-screen.png?raw=true">

#### Biomes in HUD with Tenderfoot difficulty:

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-english-tenderfoot.png?raw=true">

#### Examples of language support:

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-custom-ukrainian.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-daily-japanese.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-daily-turkish.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-chinese-simplified.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-italian.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-spanish-latam.png?raw=true">