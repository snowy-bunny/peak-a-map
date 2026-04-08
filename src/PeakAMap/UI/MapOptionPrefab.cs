using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PeakAMap.Core;
using PeakAMap.Utilities;
using static PeakAMap.UI.MapsBoardUI;

namespace PeakAMap.UI;

public sealed class MapOptionPrefab
{
    private static readonly MapOptionPrefab _instance = new();

    private MapOptionPrefab()  { Initialize();  }

    static MapOptionPrefab() { }

    public static MapOptionPrefab Instance => _instance;

    public GameObject gameObject { get; set; }

    public string MapCodePlaceholder { get; set; } = "--";

    public string InfoTextPlaceholder { get; set; } = "???";

    public GameObject Info { get; set; }

    public GameObject Line { get; set; }

    private void Initialize()
    {
        GameObject mapOption = CreateMapOption();
        GameObject mapCode = CreateMapCode(mapOption);
        GameObject biomes = CreateBiomes(mapOption);
        CreateCodeBorder(mapCode);

        gameObject = mapOption;
        Info = CreateBiomeInfo();
        Line = CreateLine();

        for (int i = 0; i < BiomeInfo.NumInfo; i++)
        {            
            GameObject textInfo = CreateTextInfo(Info);
            if (i > 0)
            {
                textInfo.GetTMPro().color = SubtitleColor;
            }
        }

        DontDestroy.Add(gameObject);
        DontDestroy.Add(Info);
    }

    private GameObject CreateMapOption()
    {
        GameObject mapOption = new GameObject("MapOption", 
            typeof(RectTransform), 
            typeof(Image), 
            typeof(HorizontalLayoutGroup), 
            typeof(Button),
            typeof(NicksButtonSFX));

        Image img = mapOption.GetImage();
        img.type = MainImageType;
        img.material = MainMaterial;

        HorizontalLayoutGroup layout = mapOption.GetComponent<HorizontalLayoutGroup>();
        layout.padding.left = MapOptionSpacing;
        layout.padding.right = MapOptionSpacing;
        layout.padding.top = 10;
        layout.padding.bottom = 10;
        layout.spacing = MapOptionSpacing;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;
        layout.childControlWidth = false;
        layout.childControlHeight = true;
        layout.childScaleWidth = false;
        layout.childScaleHeight = false;
        layout.childAlignment = TextAnchor.MiddleLeft;

        NicksButtonSFX sfx = mapOption.GetComponent<NicksButtonSFX>();
        NicksButtonSFX sfxRef = BoardingPassUI.CloseButtonCustom.GetComponent<NicksButtonSFX>();
        sfx.sfxClick = sfxRef.sfxClick;
        sfx.sfxHover = sfxRef.sfxHover;
        sfx.button = mapOption.GetComponent<Button>(); 

        return mapOption;
    }

    private GameObject CreateMapCode(GameObject parent)
    {
        GameObject mapCode = new GameObject("MapCode", typeof(RectTransform), typeof(TextMeshProUGUI));

        RectTransform rect = mapCode.GetRectTransform();
        rect.sizeDelta = new Vector2(MapCodeWidth, 0);
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.pivot = new Vector2(0.5f, 0.5f);

        TextMeshProUGUI txt = mapCode.GetTMPro();
        txt.text = MapCodePlaceholder;
        txt.font = MainFont;
        txt.color = MapCodeColor;
        txt.fontWeight = FontWeight.Black;
        txt.fontSize = 36;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        txt.overflowMode = TextOverflowModes.Overflow;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Center;
        txt.verticalAlignment = VerticalAlignmentOptions.Geometry;

        mapCode.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        return mapCode;
    }

    private GameObject CreateCodeBorder(GameObject parent)
    {
        GameObject border = Object.Instantiate(BoardingPassUI.Border);
        border.StripCloneInName();
        border.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = border.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(65, 55);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);

        Image img = border.GetImage();
        img.pixelsPerUnitMultiplier = 5;
        img.color = AccentColor;

        border.SetActive(false);

        return border;
    }

    private GameObject CreateBiomes(GameObject parent)
    {
        GameObject mapBiomes = new GameObject("MapBiomes", typeof(RectTransform), typeof(LayoutElement));

        LayoutElement elem = mapBiomes.GetComponent<LayoutElement>();
        elem.flexibleWidth = 1;

        RectTransform rect = mapBiomes.GetRectTransform();
        rect.sizeDelta = new Vector2(BiomesWidth, 0);
        rect.pivot = new Vector2(0, 0.5f);
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0, 0.5f);

        HorizontalLayoutGroup layout = mapBiomes.AddComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlHeight = true;
        layout.childControlWidth = true;
        layout.childForceExpandHeight = false;
        layout.childForceExpandWidth = false;

        mapBiomes.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        return mapBiomes;
    }

    private GameObject CreateBiomeInfo()
    {
        GameObject info = new GameObject("BiomeInfo", typeof(RectTransform), typeof(VerticalLayoutGroup));

        RectTransform rect = info.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.localScale = new Vector3(1, 1, 1);

        VerticalLayoutGroup vert = info.GetComponent<VerticalLayoutGroup>();
        vert.childAlignment = TextAnchor.MiddleLeft;
        vert.childControlHeight = false;
        vert.childControlWidth = true;
        vert.childForceExpandHeight = false;
        vert.childForceExpandWidth = true;

        return info;
    }

    private GameObject CreateTextInfo(GameObject parent)
    {
        GameObject text = GameObject.Instantiate(GUIManager.instance.boardingPass.playerName.gameObject);
        text.name = "Info";      

        TextMeshProUGUI tmp = text.GetTMPro();
        tmp.color = MainFontColor;
        tmp.text = InfoTextPlaceholder;
        tmp.fontSize = InfoFontSize;
        tmp.fontSizeMin = InfoFontSizeMin;
        tmp.fontSizeMax = InfoFontSizeMax;
        tmp.alignment = TextAlignmentOptions.Midline;
        tmp.lineSpacing = -45;

        text.transform.SetParentAndScale(parent.transform, worldPositionStays: false);
        tmp.gameObject.GetRectTransform().sizeDelta = new Vector2(0, 19);

        return text;
    }

    private GameObject CreateLine()
    {
        GameObject line = Object.Instantiate(BoardingPassUI.customOptionsWindow.transform.GetChild(2).gameObject);
        line.name = "Line";

        int width = 32;

        LayoutElement lineElem = line.AddComponent<LayoutElement>();
        lineElem.minWidth = width;
        lineElem.preferredWidth = width;
        lineElem.minHeight = LineDetailsHeight;
        lineElem.preferredHeight = LineDetailsHeight;

        RectTransform rect = line.GetRectTransform();
        rect.sizeDelta = new Vector3(width, LineDetailsHeight);

        TextMeshProUGUI tmp = line.GetTMPro();
        tmp.fontSize = 24;
        tmp.lineSpacing = -85;
        tmp.color = SubtitleColor;
        tmp.alignment = TextAlignmentOptions.Midline;

        line.SetActive(false);

        return line;
    }
}
