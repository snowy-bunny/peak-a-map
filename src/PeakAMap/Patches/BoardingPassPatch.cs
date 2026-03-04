using HarmonyLib;
using PeakAMap.Core;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(BoardingPass))]
internal class BoardingPassPatch
{
    [HarmonyPatch(nameof(BoardingPass.StartGame))]
    [HarmonyPrefix]
    private static bool StartCustomMapPatch()
    {
        if (CustomMaps.Instance.loadMode == LoadMode.Custom)
        {
            Plugin.Log.LogInfo($"Creating custom login response to load map {CustomMaps.Instance.CustomMapIndex}");
            LoginResponse response = new LoginResponse
            {
                VersionOkay = true,
                HoursUntilLevel = 0,
                MinutesUntilLevel = 0,
                SecondsUntilLevel = 0,
                LevelIndex = CustomMaps.Instance.CustomMapIndex,
                Message = $"Using PeakAMap Message."
            };
            GameHandler.GetService<NextLevelService>().NewData(response);
        }

        return true;
    }
}
