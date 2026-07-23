using BepInEx.Configuration;

namespace PeakAMap.Core;
public class UserConfig
{
    public static ConfigEntry<bool> LoadMapsOnStart;

    public static ConfigEntry<bool> ShowBiomesInHUD;

    public static ConfigEntry<bool> ShowBiomeDetails;

    public static void Initialize(ConfigFile config)
    {
        LoadMapsOnStart = config.Bind("General", "Pre-Load Missing Maps", true,
            """
            If enabled, a popup will show at the game's start that allows you 
            to retrieve map biome information when the data is missing.
            """);

        ShowBiomesInHUD = config.Bind("General", "Show Biomes in HUD", true,
            """
            If enabled, all biomes of the map you're playing in will appear 
            on screen during the run.
            """);

        ShowBiomeDetails = config.Bind("General", "Show Biome Details in UI", true,
            """
            If enabled, biome variant and open tomb information will be
            displayed in the map select UI.
            """);
    }
}
