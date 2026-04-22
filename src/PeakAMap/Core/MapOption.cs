using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PeakAMap.Utilities;
using static PeakAMap.UI.MapsBoardUI;
using PeakAMap.UI;

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

        int count;
        if (biomesInfo != null)
        {
            count = biomesInfo.Count;
        }
        else if (MapBaker.Instance.ValidSelectedBiomes())
        {
            count = MapBaker.Instance.selectedBiomes[mapIndex].biomeTypes.Count;
        }
        else
        {
            count = DefaultNumBiomes;
        }

        for (int i = 0; i < count; i++)
        {
            int numText;
            if (biomesInfo != null)
            {
                numText = biomesInfo[i].TextId.Length;
            }
            else if (MapBaker.Instance.ValidSelectedBiomes() &&
                BiomeInfo.BiomeTextIds.TryGetValue(MapBaker.Instance.selectedBiomes[mapIndex].biomeTypes[i], 
                out string[] textIds))
            {
                numText = textIds.Length;
            }
            else
            {
                numText = DefaultNumTextIDs;
            }


            for (int j = 0; j < numText; j++)
            {
                if (i > 0 || j > 0)
                {
                    GameObject line = Object.Instantiate(MapOptionPrefab.Instance.Line);
                    line.StripCloneInName();
                    line.transform.SetParentAndScale(mapBiomes.transform, worldPositionStays: false);
                    line.SetActive(true);
                }

                InstantiateInfo(mapBiomes, biomesInfo, mapIndex, i, j);
            }
        }
    }

    private static void InstantiateInfo(GameObject parent, List<BiomeInfo>? biomesInfo, int mapIndex, int biomeIndex, int idIndex)
    {
        GameObject info = Object.Instantiate(MapOptionPrefab.Instance.Info);
        info.StripCloneInName();
        info.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        // All biome info from PeakAMap data file
        if (biomesInfo != null)
        {
            SetInfoBiomeText(info, biomesInfo[biomeIndex].biomeType, idIndex);

            LocalizedText tombLoc = info.transform
                .GetChild((int)InfoType.OpenTombInfo).gameObject
                .AddComponent<LocalizedText>();
            TextMeshProUGUI tombTmp = tombLoc.gameObject
                .GetComponent<TextMeshProUGUI>();
            tombLoc.index = biomesInfo[biomeIndex].OpenTomb ? "TOMB" : "";

            TextMeshProUGUI variantTmp = info.transform
                .GetChild((int)InfoType.VariantInfo).gameObject
                .GetTMPro();
            string variantText = biomesInfo[biomeIndex].Variant;
            variantTmp.text = variantText;

            return;
        }

        // Biome info from MapBaker with missing variant and tomb info
        else if (MapBaker.Instance.ValidSelectedBiomes())
        {
            Biome.BiomeType biomeType = MapBaker.Instance
                .selectedBiomes[mapIndex]
                .biomeTypes[biomeIndex];

            SetInfoBiomeText(info, biomeType, idIndex);

            // Indicate missing variant and tomb info for appropriate biomes
            info.transform
                .GetChild((int)InfoType.OpenTombInfo).gameObject
                .SetActive(biomeType == Biome.BiomeType.Mesa);
            info.transform
                .GetChild((int)InfoType.VariantInfo).gameObject
                .SetActive(BiomeInfo.HasVariants.Contains(biomeType));

            return;
        }

        // Default if missing biome info
        else
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
        }
    }

    private static void SetInfoBiomeText(GameObject info, Biome.BiomeType biomeType, int idIndex)
    {
        if (BiomeInfo.BiomeTextIds.TryGetValue(biomeType, out string[] textIds))
        {
            LocalizedText biomeLoc = info.transform
                .GetChild((int)InfoType.BiomeNameInfo).gameObject
                .GetOrAddComponent<LocalizedText>();

            biomeLoc.index = textIds[idIndex];
        }
        // Fallback for biomes without localized text listed, i.e., biomes not listed in BiomeInfo.BiomeTextIds
        else
        {
            TextMeshProUGUI biomeTmp = info.transform
                .GetChild((int)InfoType.BiomeNameInfo).gameObject
                .GetTMPro();
            biomeTmp.text = System.Enum.GetName(typeof(Biome.BiomeType), biomeType).ToUpperInvariant();
        }
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
                if (tmp.gameObject.TryGetComponent(out LocalizedText loc))
                {
                    tmp.gameObject.SetActive(loc.index == "TOMB");
                }
                else
                {
                    tmp.gameObject.SetActive(true);
                }
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
