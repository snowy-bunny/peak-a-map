using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PeakAMap.Utilities;
[HarmonyPatch]
public static class boardingPassUI
{
    private static bool s_initialized;

    public static BoardingPass boardingPass { get; private set; }

    public static Image Panel { get; private set; }

    public static GameObject InnerboardingPass { get; private set; }

    public static Button StartGameButton { get; private set; }

    public static Button IncrementAscentButton { get; private set; }

    public static GameObject Plane { get; private set; }

    public static TMP_Text PlayerName { get; private set; }

    public static Vector2 Pivot { get; private set; }

    public static GameObject BlueTop { get; private set; }

    private static void Initialize()
    {
        boardingPass = GUIManager.instance.boardingPass;

        Panel = boardingPass.playerName.transform.parent.GetImage();

        InnerboardingPass = Panel.transform.parent.gameObject;

        StartGameButton = boardingPass.startGameButton;

        IncrementAscentButton = boardingPass.incrementAscentButton;

        Plane = Panel.gameObject.QueryChildren("Plane")
            ?? new GameObject("PlaneFallback", typeof(RectTransform), typeof(Image));

        PlayerName = boardingPass.playerName;

        Pivot = InnerboardingPass.GetComponent<RectTransform>().pivot;

        BlueTop = Panel.gameObject.QueryChildren("BlueTop")
            ?? new GameObject("BlueTopFallback", typeof(Image));
    }

    [HarmonyPatch(typeof(AirportCheckInKiosk), nameof(AirportCheckInKiosk.Start))]
    [HarmonyPostfix]
    private static void InitializeBoardingPassUI()
    {
        if (!s_initialized)
        {
            Initialize();
            s_initialized = true;
        }
    }
}
