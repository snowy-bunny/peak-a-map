using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PeakAMap.Utilities;
using static PeakAMap.UI.MapsBoardUI;

namespace PeakAMap.UI;
internal sealed class ConfigEntryTogglePrefab
{
    private static readonly ConfigEntryTogglePrefab _instance = new();

    private ConfigEntryTogglePrefab() { Initialize(); }

    static ConfigEntryTogglePrefab() { }

    public static ConfigEntryTogglePrefab Instance => _instance;

    public GameObject gameObject { get; private set; }

    private void Initialize()
    {
        gameObject = SetConfigEntryToggle();
        DontDestroy.Add(gameObject);
    }

    private GameObject SetConfigEntryToggle()
    {
        GameObject prefab = BoardingPassUI.CustomOptionItemTogglePrefab.gameObject;
        GameObject entry = Object.Instantiate(prefab);
        entry.name = "ConfigEntryToggle";

        CustomOptionItemToggle toggleRef = entry.GetComponent<CustomOptionItemToggle>();
        Object.DestroyImmediate(toggleRef);

        RectTransform rect = entry.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);

        TextMeshProUGUI tmp = entry.GetComponentInChildren<TextMeshProUGUI>();
        Image? box = rect.gameObject.QueryChildren("Toggle/Background")?.GetImage();
        Image? check = box?.transform.GetChild(0).GetImage();
        Image? bg = box?.transform.parent.QueryChildren("Image")?.GetImage();

        if (box == null || check == null || bg == null)
        {
            return entry;
        }

        tmp.color = MainFontColor;
        box.color = MainFontColor;
        check.color = AccentColor;
        bg.color = Cell1Color;

        RectTransform bgRect = bg.gameObject.GetRectTransform();
        bgRect.sizeDelta = new Vector2(-40, bgRect.sizeDelta.y);
        bgRect.anchoredPosition = new Vector2(0, 0);

        RectTransform titleRect = tmp.gameObject.GetRectTransform();
        titleRect.anchoredPosition = new Vector2(-20, 0);

        // LocalizedText removed until able to support more languages
        Object.DestroyImmediate(tmp.gameObject.GetComponent<LocalizedText>());

        return entry;
    }
}
