using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PeakAMap.Utilities;
[HarmonyPatch]
public static class BoardingPassUI
{
    private static bool s_initialized;

    public static BoardingPass boardingPass { get; private set; }

    public static Image Panel { get; private set; }

    public static GameObject InnerBoardingPass { get; private set; }

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

        InnerBoardingPass = Panel.transform.parent.gameObject;

        StartGameButton = boardingPass.startGameButton;

        IncrementAscentButton = boardingPass.incrementAscentButton;

        Title = Panel.gameObject.QueryChildren("BOARDING PASS");

        Plane = Panel.gameObject.QueryChildren("Plane");

        PlayerName = boardingPass.playerName;

        Pivot = InnerBoardingPass.GetComponent<RectTransform>().pivot;

        BlueTop = Panel.gameObject.QueryChildren("BlueTop");

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
