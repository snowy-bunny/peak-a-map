using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PeakAMap.Utilities;
using static PeakAMap.UI.MapsBoardUI;
using PeakAMap.UI;
using BepInEx;

namespace PeakAMap.Core;

public class MapOption : MonoBehaviour
{
    public enum InfoType
    {
        BiomeNameInfo,
        OpenTombInfo,
        VariantInfo
    }

    public static string MapCodeFormat = "00";

    public const int DefaultNumBiomes = 5;

    public const int DefaultNumTextIDs = 1;

    public GameObject MapsList { get; set; }

    public GameObject MapBiomes { get; set; }

    public TextMeshProUGUI MapCode { get; set; }

    public Image BGImage { get; set; }

    public Button button { get; set; }

    public TextMeshProUGUI[] TextInfo { get; set; }

    public LayoutElement[] Lines { get; set; }

    public GameObject OpenTombInfo { get; set; }

    public GameObject VariantInfo { get; set; }

    public GameObject MapCodeBorder { get; set; }

    public bool Selected => false;

    public int MapIndex
    {
        get
        {
            int.TryParse(MapCode.text, out int index);
            return index;
        }
    }

    public static void Instantiate(int index, Transform parent)
    {
        GameObject gameObject = Instantiate(MapOptionPrefab.Instance.gameObject);
        gameObject.StripCloneInName();
        gameObject.transform.SetParentAndScale(parent, worldPositionStays: false);

        GameObject mapCode = gameObject.QueryChildren("MapCode");
        mapCode.GetTMPro().text = index.ToString(MapCodeFormat);

        FillMapBiomes(gameObject.QueryChildren("MapBiomes"), index);

        MapOption mapOption = gameObject.AddComponent<MapOption>();
    }

    private static void FillMapBiomes(GameObject mapBiomes, int mapIndex)
    {
        List<BiomeInfo>? biomesInfo;
        try
        {
            biomesInfo = MapRotationHandler.Instance.CurrMapRotation[mapIndex];
        }
        catch
        {
            biomesInfo = null;
        }

        int count = (biomesInfo == null) ? DefaultNumBiomes : biomesInfo.Count;

        for (int i = 0; i < count; i++)
        {
            int numText = (biomesInfo == null) ? DefaultNumTextIDs : biomesInfo[i].TextId.Length;
            for (int j = 0; j < numText; j++)
            {
                if (i > 0 || j > 0)
                {
                    GameObject line = Object.Instantiate(MapOptionPrefab.Instance.Line);
                    line.StripCloneInName();
                    line.transform.SetParentAndScale(mapBiomes.transform, worldPositionStays: false);
                    line.SetActive(true);
                }

                InstantiateInfo(mapBiomes, biomesInfo, i, j);
            }
        }
    }

    private static void InstantiateInfo(GameObject parent, List<BiomeInfo>? biomesInfo, int biomeIndex, int idIndex)
    {
        GameObject info = Object.Instantiate(MapOptionPrefab.Instance.Info);
        info.StripCloneInName();
        info.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        if (biomesInfo == null)
        {
            info.transform
                .GetChild((int)InfoType.BiomeNameInfo).gameObject
                .SetActive(true);

            info.transform
                .GetChild((int)InfoType.OpenTombInfo).gameObject
                .SetActive(false);

            info.transform
                .GetChild((int)InfoType.VariantInfo).gameObject
                .SetActive(false);

            return;
        }

        LocalizedText biomeLoc = info.transform
            .GetChild((int)InfoType.BiomeNameInfo).gameObject
            .GetOrAddComponent<LocalizedText>();
        biomeLoc.index = biomesInfo[biomeIndex].TextId[idIndex];

        LocalizedText tombLoc = info.transform
            .GetChild((int)InfoType.OpenTombInfo).gameObject
            .AddComponent<LocalizedText>();
        TextMeshProUGUI tombTmp = tombLoc.gameObject
            .GetComponent<TextMeshProUGUI>();
        tombLoc.index = biomesInfo[biomeIndex].OpenTomb ? "TOMB" : "";

        TextMeshProUGUI variantTmp = info.transform
            .GetChild((int)InfoType.VariantInfo).gameObject
            .GetTMPro();
        string variantText = BiomeInfo.CleanVariant(biomesInfo[biomeIndex].Variant, biomesInfo[biomeIndex].biomeType);
        variantTmp.text = variantText;
    }

    private void Awake()
    {
        BGImage = GetComponent<Image>();
        button = GetComponent<Button>();

        foreach (Transform child in transform)
        {
            if (child.name.Equals("MapCode"))
            {
                MapCode = child.GetTMPro();
                MapCodeBorder = child.QueryChildren("Border").gameObject;
                continue;
            }
            if (child.name.Equals("MapBiomes"))
            {
                MapBiomes = child.gameObject;

                TextMeshProUGUI[] allTMPro = child.GetComponentsInChildren<TextMeshProUGUI>();
                TextInfo = allTMPro.Where(tmp => tmp.name.Equals("Info")).ToArray();
                Lines = allTMPro.Where(tmp => tmp.name.Equals("Line"))
                    .Select(tmp => tmp.gameObject.GetComponent<LayoutElement>()).ToArray();
            }
        }
    }

    private void Start()
    {
        RefreshDetailsActive();
        LocalizedText.OnLangugageChanged += RefreshFontSize;
    }

    private void OnDestroy()
    {
        LocalizedText.OnLangugageChanged -= RefreshFontSize;
    }

    public void SetSelected()
    {
        MapCodeBorder.SetActive(true);
    }

    public void RefreshDetailsActive()
    {
        bool active = UserConfig.ShowBiomeDetails.Value;
        TextMeshProUGUI tmp;
        int infoIndex;
        LocalizedText loc;

        for (int i = 0; i < TextInfo.Length; i++)
        {
            tmp = TextInfo[i];
            infoIndex = tmp.transform.GetSiblingIndex();

            if (infoIndex == (int)InfoType.BiomeNameInfo)
            {
                continue;
            }

            if (!active)
            {
                tmp.gameObject.SetActive(false);
                continue;
            }

            if (infoIndex == (int)InfoType.OpenTombInfo)
            {
                loc = tmp.gameObject.GetComponent<LocalizedText>();
                tmp.gameObject.SetActive(loc.index == "TOMB");
                continue;
            }

            if (infoIndex == (int)InfoType.VariantInfo)
            {
                tmp.gameObject.SetActive(!string.IsNullOrWhiteSpace(tmp.text));
                continue;
            }
        }

        RefreshFontSize();
    }

    private void RefreshFontSize()
    {
        if (TextInfo == null || TextInfo.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < TextInfo.Length; i++)
        {
            TextInfo[i].fontSize = InfoFontSize;
            TextInfo[i].fontSizeMin = InfoFontSizeMin;
            TextInfo[i].fontSizeMax = InfoFontSizeMax;
        }
        float minFontSize = TextInfo.Min(p => p.fontSize);
        for (int i = 0; i < TextInfo.Length; i++)
        {
            TextInfo[i].fontSize = minFontSize;
            TextInfo[i].fontSizeMin = minFontSize;
            TextInfo[i].fontSizeMax = minFontSize;
        }

        for (int i = 0; i < Lines.Length; i++)
        {
            if (UserConfig.ShowBiomeDetails.Value)
            {
                Lines[i].preferredHeight = LineDetailsHeight;
                Lines[i].minHeight = LineDetailsHeight;
            }
            else
            {
                Lines[i].preferredHeight = LineSimpleHeight;
                Lines[i].minHeight = LineSimpleHeight;
            }
        }
    }

    public static void UpdateAllBgColors(GameObject list)
    {
        for (int i = 0; i < list.transform.childCount; i++)
        {
            list.transform.GetChild(i).TryGetComponent(out Image image);
            image.color = GetBgColor(i, list);
        }
    }

    private static Color GetBgColor(int index, GameObject list)
    {
        int colParity;
        int rowParity;
        int numRows = (int)System.Math.Ceiling((decimal)list.transform.childCount / VisibleCols);

        if (numRows <= VisibleRows)
        {
            colParity = index / VisibleRows % 2;
            rowParity = (index - VisibleRows * colParity) % 2;
        }
        else
        {
            colParity = index / numRows % 2;
            rowParity = (index - numRows * colParity) % 2;
        }

        return colParity == rowParity ? Cell1Color : Cell2Color;
    }
}
