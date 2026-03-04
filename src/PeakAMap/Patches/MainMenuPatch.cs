using HarmonyLib;
using PeakAMap.Core;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(MainMenu))]
internal class MainMenuPatch
{
    [HarmonyPatch(nameof(MainMenu.Start))]
    [HarmonyPostfix]
    private static void SearchForAllMapsPatch()
    {
        if (MapRotationHandler.Instance.NeedToLoad)
        {
            Plugin.Log.LogInfo("Missing biomes data. Loading maps to gather missing biomes info.");
            MapRotationHandler.Instance.LoadAllMaps();
        }
    }
}
