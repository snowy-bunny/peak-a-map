# PeakAMap

**Find out details such as the biomes, biome variants, and whether the Tomb is open for the current map rotation. Then select a map to play with an in-game UI.** 

PeakAMap searches for details of each map and lets players pick one to play. **There shouldn't be, but there may be inaccuracies.** For performance reasons, the loading and searching is done beforehand and saved in a file included with PeakAMap. If this file is missing or a new maps are patched in, then PeakAMap will automatically create this file when the game starts. _For slower PCs, this process may take 2-4 minutes._

## Features

- **Use a UI to Select a Map to Play**

   <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/boarding-pass-map-00-button.png?raw=true" width=800px>
  
   <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-custom-detailed-english.png?raw=true" width=800px>

	- Biomes of each map are displayed.
	- Choose to play a custom map or to play the daily map.
	- If you're playing with friends, only the host needs PeakAMap for everyone to play a selected map.
      > NOTE: The host must be the one to start the game for the selected map to be played.

- **Biome Variant and Open Tomb Info**
  - Extra information, i.e., biome variant and whether the Tomb is open for current maps, are displayed with the map select UI.

	> TIP: This feature is optional.
   If you would like to keep this information hidden, this feature can be disabled with the in-game toggle.
   
      <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/toggle-more-details.png?raw=true" width=500px>

- **Biomes are Displayed in the HUD During Your Run**
  
  <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-english.png?raw=true" width=800px>

	> TIP: This feature is optional.
   If you don't like having extra text on your screen, this feature can be disabled with the in-game toggle.
  
   <img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/toggle-display-biomes-in-run.png?raw=true" width=500px>

- **Map Rotation Info is Preloaded for the Suppported PEAK Version**
  - Currently supporting map rotation from PEAK 1.60.d
  
- **Support for Future Map Patches** 
  - PeakAMap *SHOULD* still display correct information when PEAK patches in new maps.
  - If PeakAMap doesn't have data for the current map rotation or there are issues with the file containing this data, the map rotation info will be loaded, searched, and saved at the game's start
      > TIP: This feature is optional.
      If you're only interested in selecting a map without the biome info or if preloading takes too long. This feature can be disabled in your `.cfg` file. Instructions on how to do so is further below in the "Configuration" section.

- **Partial Language Support** 
  - Some phrases/words that were already supported in game will be supported in PeakAMap.

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
   - Go to [PeakAMap Thunderstore page](https://thunderstore.io/c/peak/p/snowybunny/PeakAMap/) > "Manual Download".
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
- Enable or disable map rotation info from loading at the game's start when data files are missing.
- Enable or disable biomes of the map you're playing in from showing in the HUD.
- Enable or disable biome variant and open tomb information from showing in the map select UI.

### Instructions: 

- Run PEAK with PeakAMap once and quit game.
- Edit the `.cfg` file one of two ways:
  1. If you installed **PeakAMap** with a **mod manager**, you can edit the config file with your mod manager app.
  2. If you installed **PeakAMap** **manually**:
     - Open Steam > "Library" > "PEAK" > gears/settings icon > "Manage" > "Browse local files" > `BepInEx` > `config`
     - Open `com.github.snowybunny.PeakAMap.cfg`.
     - Set the values to "true" or "false" according to your preference.
- If your game is still running. **Quit** and **relaunch** to apply settings.

## Extra Screenshots:

#### Boarding pass and map select UIs with daily map selected and "More Details" disabled:

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/boarding-pass-daily-map-button.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-daily-simple-english.png?raw=true">

#### Loading screen when data is missing:

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/loading-maps-screen.png?raw=true">

#### Biomes in HUD during a Custom Run:

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-english-customrun.png?raw=true">

#### Examples of language support:

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-custom-ukrainian.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-daily-japanese.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/map-select-ui-daily-turkish.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-chinese-simplified.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-italian.png?raw=true">

<img src="https://github.com/snowy-bunny/peak-a-mod/blob/main/assets/biomes-hud-spanish-latam.png?raw=true">
