using HarmonyLib;
using Peak.Network;
using Photon.Pun;
using Zorro.Core;
using PeakAMap.Core;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(AirportCheckInKiosk))]
internal class AirportCheckInKioskPatch
{
    public static CustomMaps customMaps => CustomMaps.Instance;

    [HarmonyPatch(nameof(AirportCheckInKiosk.Start))]
    [HarmonyPostfix]
    private static void InstantiateMapsBoard()
    {
        MapsBoard.Instantiate();
    }

    [HarmonyPatch(nameof(AirportCheckInKiosk.LoadIslandMaster))]
    [HarmonyPrefix]
    private static bool LoadCustomMapPatch(AirportCheckInKiosk __instance, int ascent, byte[] serializedRunSettings)
    {
        if (NetCode.Session.IsHost && customMaps.loadMode != LoadMode.Daily)
        {
            int mapIndex = customMaps.SelectedMapIndex;
            Plugin.Log.LogInfo($"Loading {customMaps.loadMode.GetName().ToLower()} map {mapIndex}");

            string text = "WilIsland";
            text = SingletonAsset<MapBaker>.Instance.GetLevel(mapIndex + NextLevelService.debugLevelIndexOffset);
            if (string.IsNullOrEmpty(text))
            {
                text = "WilIsland";
            }
            __instance.photonView.RPC("BeginIslandLoadRPC", RpcTarget.All, text, ascent, serializedRunSettings);

            return false;
        }

        return true;
    }
}
