using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PeakAMap.Utilities;
using static PeakAMap.UI.MapsBoardUI;

namespace PeakAMap.UI;

public sealed class MapOptionPrefab
{
    private static readonly MapOptionPrefab _instance = new();

    private MapOptionPrefab()
    {
        GameObject mapOption = CreateMapOption();
        GameObject mapCode = CreateMapCode(mapOption);
        GameObject biomes = CreateBiomes(mapOption);
        gameObject = mapOption;
        Object.DontDestroyOnLoad(gameObject);
    }

    static MapOptionPrefab() { }

    public static MapOptionPrefab Instance => _instance;

    public GameObject gameObject { get; set; }

    public string MapCodePlaceholder { get; set; } = "--";

    public string BiomesTextPlaceholder { get; set; } = "???";

    private GameObject CreateMapOption()
    {
        GameObject mapOption = new GameObject("MapOption", 
            typeof(RectTransform), 
            typeof(Image), 
            typeof(HorizontalLayoutGroup), 
            typeof(Button));

        Image img = mapOption.GetImage();
        img.type = MainImageType;
        img.material = MainMaterial;

        HorizontalLayoutGroup layout = mapOption.GetComponent<HorizontalLayoutGroup>();
        layout.padding.left = MapOptionSpacing;
        layout.padding.right = MapOptionSpacing;
        layout.spacing = MapOptionSpacing;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;
        layout.childControlWidth = false;
        layout.childControlHeight = true;
        layout.childScaleWidth = false;
        layout.childScaleHeight = false;

        return mapOption;
    }

    private GameObject CreateMapCode(GameObject parent)
    {
        GameObject mapCodeObject = new GameObject("MapCode", typeof(RectTransform), typeof(TextMeshProUGUI));

        RectTransform rect = mapCodeObject.GetRectTransform();
        rect.sizeDelta = new Vector2(MapCodeWidth, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 0.5f);

        TextMeshProUGUI txt = mapCodeObject.GetTMPro();
        txt.text = MapCodePlaceholder;
        txt.font = MainFont;
        txt.color = MapCodeColor;
        txt.fontWeight = FontWeight.Black;
        txt.fontSize = 28;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        txt.overflowMode = TextOverflowModes.Overflow;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Center;
        txt.verticalAlignment = VerticalAlignmentOptions.Geometry;

        mapCode.transform.SetParentAndScale(parent.transform, worldPositionStays: false);
    }

    private GameObject CreateBiomes(GameObject parent)
    {
        GameObject biomesObject = new GameObject("MapBiomes", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(LayoutElement));

        RectTransform rect = biomesObject.GetRectTransform();
        rect.sizeDelta = new Vector2(BiomesWidth, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0, 0.5f);

        LayoutElement elem = biomesObject.GetComponent<LayoutElement>();
        elem.flexibleWidth = 1;

        TextMeshProUGUI txt = biomesObject.GetTMPro();
        txt.text = BiomesTextPlaceholder;
        txt.font = MainFont;
        txt.color = MainColor;
        txt.fontWeight = FontWeight.SemiBold;
        txt.fontSize = 20;
        txt.textWrappingMode = TextWrappingModes.Normal;
        txt.overflowMode = TextOverflowModes.Overflow;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Left;
        txt.verticalAlignment = VerticalAlignmentOptions.Geometry;
        txt.lineSpacing = -35;

        biomesObject.transform.SetParent(parent.transform, worldPositionStays: false);
        rect.localScale = new Vector3(1, 1, 1);

        return parent;
    }
}
