using HarmonyLib;
using Zorro.UI.Modal;
using PeakAMap.Core;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(MainMenuPageHandler))]
internal class MainMenuPageHandlerPatch
{    
    [HarmonyPatch(nameof(MainMenuPageHandler.Start))]
    [HarmonyPostfix]
    private static void SearchForAllMapsPatch()
    {
        if (MapRotationHandler.Instance.NeedToLoad && !Modal.IsOpen)
        {
            Modal.OpenYesNoModal(new DefaultHeaderModalOption(
                "[PeakAMap]\nINCOMPLETE MAP ROTATION DATA",
                "Load and search for missing biome info?"),
                LocalizedText.GetText("BACK"),
                LocalizedText.GetText("OK"),
                delegate 
                {
                    MapRotationHandler.Instance._cancelIndicator = true;
                },
                delegate
                {
                    Plugin.Log.LogInfo("Missing biomes data. Loading maps to gather missing biomes info.");
                    MapRotationHandler.Instance.LoadAllMaps();
                }
            );
        }
    }
}
