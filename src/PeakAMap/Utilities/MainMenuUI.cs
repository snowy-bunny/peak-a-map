using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PeakAMap.Utilities;
[HarmonyPatch]
public static class MainMenuUI
{
    private static bool s_initialized = false;

    public static GameObject Dropdown { get; private set; }

    public static GameObject SettingsBackButton { get; private set; }

    public static TextMeshProUGUI VersionTMPro { get; private set; }

    private static void Initialize(MainMenu instance)
    {
        GameObject mainMenu = instance.credits.transform.parent.parent.gameObject;

        GameObject innerMainMenu = instance.credits.transform.parent.gameObject;

        GameObject MainMenuReference = new GameObject("MainMenuReference");

        Dropdown = Object.Instantiate(mainMenu
            .QueryChildren("FirstTimeSetupPage/SettingParent/ENUM DROPDOWN/Dropdown"));
        Dropdown.transform.SetParent(MainMenuReference.transform, worldPositionStays: false);

        SettingsBackButton = Object.Instantiate(mainMenu
            .QueryChildren("SettingsPage/SettingsPageShared/UI_MainMenuButton_LeaveGame (2)"));
        SettingsBackButton.transform.SetParent(MainMenuReference.transform, worldPositionStays: false);

        VersionTMPro = Object.Instantiate(innerMainMenu.QueryChildren("Version").GetTMPro());
        VersionTMPro.transform.SetParent(MainMenuReference.transform, worldPositionStays: false);

        UI.DontDestroy.Add(MainMenuReference);
    }

    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    [HarmonyPostfix]
    private static void InitializeMainMenu(MainMenu __instance)
    {
        if (!s_initialized)
        {
            Initialize(__instance);
            s_initialized = true;
        }
    }
}
