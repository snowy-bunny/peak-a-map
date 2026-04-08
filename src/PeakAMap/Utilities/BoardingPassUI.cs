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

    public static GameObject Title { get; private set; }

    public static GameObject Plane { get; private set; }

    public static TMP_Text PlayerName { get; private set; }

    public static Vector2 Pivot { get; private set; }

    public static GameObject BlueTop { get; private set; }

    public static CustomOptionsWindow customOptionsWindow { get; private set; }

    public static Button CloseButtonCustom { get; private set; }

    public static GameObject CustomWindowPanel { get; private set; }

    public static GameObject CustomOptions { get; private set; }

    public static GameObject Background { get; private set; }

    public static GameObject Border { get; private set; }

    public static Button MiniRunButton { get; private set; }

    public static Button CustomOptionsButton { get; private set; }

    public static CustomOptionItemToggle CustomOptionItemTogglePrefab { get; private set; }

    public static TextMeshProUGUI AscentDescription { get; private set; }

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

        customOptionsWindow = boardingPass.customOptionsWindow;

        CloseButtonCustom = customOptionsWindow.closeButtonCustom;

        CustomWindowPanel = CloseButtonCustom.transform.parent.gameObject;

        CustomOptions = CustomWindowPanel.transform.parent.gameObject;

        Background = customOptionsWindow.gameObject.QueryChildren("BG");

        Border = CustomOptions.QueryChildren("Border");

        MiniRunButton = customOptionsWindow.miniRunButton;

        CustomOptionsButton = boardingPass.customOptionsButton;

        CustomOptionItemTogglePrefab = customOptionsWindow.customOptionItemTogglePrefab;

        AscentDescription = CustomOptionsButton.transform.parent.QueryChildren("Description").GetTMPro();
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
