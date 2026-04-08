using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using PeakAMap.Core;
using PeakAMap.UI;
using PeakAMap.Utilities;
using TMPro;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(BoardingPass))]
internal class BoardingPassPatch
{
    public static BoardingPass boardingPass => GUIManager.instance.boardingPass;

    public static CustomMaps customMaps => CustomMaps.Instance;

    public static MapsBoard mapsBoard => MapsBoard.Instance;

    public static TextMeshProUGUI OpenButtonTMPro { get; private set; }

    public static GameObject Title { get; private set; }

    [HarmonyPatch(nameof(BoardingPass.StartGame))]
    [HarmonyPrefix]
    private static bool StartCustomMapPatch()
    {
        if (customMaps.loadMode == LoadMode.Custom)
        {
            Plugin.Log.LogInfo($"Creating custom login response to load map {customMaps.CustomMapIndex}");
            LoginResponse response = new LoginResponse
            {
                VersionOkay = true,
                HoursUntilLevel = 0,
                MinutesUntilLevel = 0,
                SecondsUntilLevel = 0,
                LevelIndex = customMaps.CustomMapIndex,
                Message = $"Using PeakAMap Message."
            };
            GameHandler.GetService<NextLevelService>().NewData(response);
        }

        return true;
    }

    [HarmonyPatch(nameof(BoardingPass.Initialize))]
    [HarmonyPostfix]
    private static void AddOpenMapsBoardButton()
    {
        GameObject open = CreateOpenButton(boardingPass.closeButton.transform.parent);
        open.SetActive(true);
        open.GetComponent<Button>().onClick.AddListener(OpenMapsBoard);
        OpenButtonTMPro = open.GetComponentInChildren<TextMeshProUGUI>();

        Title = boardingPass.playerName.transform.parent.gameObject.QueryChildren("BOARDING PASS");
        ResizeBoardingPassTitle();
    }

    [HarmonyPatch(nameof(BoardingPass.OnOpen))]
    [HarmonyPostfix]
    private static void UpdateOpenMapsBoardText()
    {
        RefreshOpenButtonText();
    }

    private static GameObject CreateOpenButton(Transform parent)
    {
        Button button = Object.Instantiate(boardingPass.customOptionsButton);
        button.onClick.RemoveAllListeners();

        GameObject open = button.gameObject;
        open.name = "OpenMapsBoard";
        open.transform.SetParentAndScale(parent);

        RectTransform rect = open.GetRectTransform();
        rect.anchoredPosition = new Vector2(-465, MapsBoardUI.OpenButtonYPosition);
        rect.sizeDelta = new Vector2(185, 60);
        rect.pivot = new Vector2(1, 0.5f);
        rect.anchorMin = new Vector2(1, 0.5f);
        rect.anchorMax = new Vector2(1, 0.5f);

        open.GetImage().pixelsPerUnitMultiplier = 5;

        GameObject child = open.QueryChildren("Text");

        TextMeshProUGUI tmp = child.GetTMPro();
        tmp.fontSize = 28;
        tmp.fontSizeMax = 28;

        // LocalizedText removed until able to support more languages
        Object.Destroy(child.GetComponent<LocalizedText>());

        return open;
    }

    public static void ResizeBoardingPassTitle()
    {
        GameObject gameObject = Title;
        GameObject parent = gameObject.transform.parent.gameObject;

        RectTransform rectParent = parent.GetComponent<RectTransform>();
        RectTransform rect = gameObject.GetRectTransform();

        Vector3[] vParent = new Vector3[4];
        Vector3[] v = new Vector3[4];

        rectParent.GetWorldCorners(vParent);
        rect.GetWorldCorners(v);

        float xParent = vParent[0].x;
        float x = v[0].x;

        float newX = (x - xParent) / rect.lossyScale.x;

        rect.pivot = new Vector2(0, 0.5f);
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0, 0.5f);
        rect.sizeDelta = new Vector2(450, rect.sizeDelta.y);
        rect.anchoredPosition = new Vector2(newX, rect.anchoredPosition.y);
    }

    public static void OpenMapsBoard()
    {
        boardingPass.Close();
        mapsBoard.Open();
    }

    private static void RefreshOpenButtonText()
    {
        if (customMaps.loadMode == LoadMode.Custom)
        {
            string mapCode = customMaps.SelectedMapIndex.ToString(MapOption.MapCodeFormat);
            OpenButtonTMPro.text = $"MAP {mapCode}";
        }
        else
        {
            OpenButtonTMPro.text = "DAILY MAP";
        }
    }
}
