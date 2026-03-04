using HarmonyLib;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(AirportCheckInKiosk))]
internal class AirportCheckInKioskPatch
{
    [HarmonyPatch(nameof(AirportCheckInKiosk.Start))]
    [HarmonyPostfix]
    private static void InstantiateMapsBoard()
    {
        Core.MapsBoardHandler.Instantiate();
    }
}
