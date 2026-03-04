using HarmonyLib;
using TMPro;

namespace PeakAMap.Utilities;
[HarmonyPatch]
public static class PassportUI
{
    private static bool s_initialized;

    public static PassportManager passportManager { get; private set; }

    public static TextMeshProUGUI PassportText { get; private set; }

    private static void Initialize()
    {
        passportManager = PassportManager.instance;

        PassportText = passportManager.uiObject
            .QueryChildren("Canvas/Panel/Panel/BG/Portrait")?
            .GetComponentInChildren<TextMeshProUGUI>()
            ?? new TextMeshProUGUI();
    }

    [HarmonyPatch(typeof(AirportCheckInKiosk), nameof(AirportCheckInKiosk.Start))]
    [HarmonyPostfix]
    private static void InitializePassportUI()
    {
        if (!s_initialized)
        {
            Initialize();
            s_initialized = true;
        }
    }
}
