using BepInEx.Configuration;

namespace PeakAMap.Core;
public class UserConfig
{
    public static ConfigEntry<bool> LoadMapsOnStart;

    public static ConfigEntry<bool> ShowBiomesInHUD;

    public static void Initialize(ConfigFile config)
    {
        LoadMapsOnStart = config.Bind("General", "Pre-Load Missing Maps", true,
            """
            If enabled, any missing information on map biomes will be
            retrieved at the game's start.
            """);

        ShowBiomesInHUD = config.Bind("General", "Show Biomes In HUD", true,
            """
            If enabled, all biomes of the map you're playing in will appear 
            on screen during the run.
            """);
    }
}
