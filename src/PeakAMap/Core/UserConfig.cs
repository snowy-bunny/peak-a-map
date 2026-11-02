using BepInEx.Configuration;

namespace PeakAMap.Core;
public class UserConfig
{
    public static ConfigEntry<bool> ShowBiomesInHUD;

    public static ConfigEntry<bool> ShowBiomeDetails;

    public static void Initialize(ConfigFile config)
    {
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
