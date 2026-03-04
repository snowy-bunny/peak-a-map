using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PeakAMap.Core;
using PeakAMap.Utilities;
using static PeakAMap.UI.MapsBoardUI;

namespace PeakAMap.UI;

public class MapOption : MonoBehaviour
{
    public static string Separator = " | ";

    public static string MapCodeFormat = "00";

    public MapBiomes Biomes { get; set; }

    public TextMeshProUGUI MapCode { get; set; }

    public Image BGImage { get; set; }

    public Button button { get; set; }

    private int _mapIndex;

    public int MapIndex
    {
        get { return _mapIndex; }
        set
        {
            _mapIndex = value;
            MapCode.text = MapCodeText;
            BGImage.color = BgColor;
            List<BiomeInfo>? biomesInfo = MapRotationHandler.Instance.CurrMapRotation[_mapIndex];
            Biomes.BiomesTextIds = MapBiomes.GetBiomesLocalizedText(biomesInfo);
        }
    }

    public Color BgColor
    {
        get
        {
            int colParity = (MapIndex / VisibleRows) % 2;
            int rowParity = (MapIndex - (VisibleRows * colParity)) % 2;
            return (colParity == rowParity) ? Cell1Color : Cell2Color;
        }
    }

    public string MapCodeText
    {
        get
        {
            return MapIndex.ToString(MapCodeFormat);
        }
    }

    public static void Instantiate(int index, Transform parent)
    {
        GameObject gameObject = Object.Instantiate(MapOptionPrefab.Instance.gameObject);
        gameObject.transform.SetParent(parent, worldPositionStays: false);
        MapOption mapOption = gameObject.AddComponent<MapOption>();
        mapOption.MapIndex = index;

        RectTransform rect = gameObject.GetRectTransform();
        rect.localScale = new Vector3(1, 1, 1);
    }

    private void Awake()
    {
        BGImage = GetComponent<Image>();
        button = GetComponent<Button>();

        foreach (Transform child in gameObject.transform)
        {
            if (child.name.Equals("MapCode"))
            {
                MapCode = child.GetTMPro();
                continue;
            }

            if (child.name.Equals("MapBiomes"))
            {
                Biomes = child.gameObject.GetOrAddComponent<MapBiomes>();
            }
        }
    }
}
