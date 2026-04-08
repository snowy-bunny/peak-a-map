using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using PeakAMap.UI;

namespace PeakAMap.Core;
public class MapBiomes : MonoBehaviour
{
    public TextMeshProUGUI TMPro { get; set; }

    private List<LocalizedText> _biomesTextIds;

    public List<LocalizedText> BiomesTextIds
    {
        get
        {
            return _biomesTextIds;
        }
        set
        {
            _biomesTextIds = value;
            UpdateLocalizedText();
        }
    }

    public static string Separator = " | ";

    public static List<LocalizedText> GetBiomesLocalizedText(List<BiomeInfo>? biomesInfo)
    {
        if (biomesInfo == null)
        {
            return new();
        }

        List<Biome.BiomeType>? biomeTypes = biomesInfo?.Select((info) => (Biome.BiomeType)info.BiomeTypeInt).ToList();
        return GetBiomesLocalizedText(biomeTypes);
    }

    public static List<LocalizedText> GetBiomesLocalizedText(List<Biome.BiomeType>? biomeTypes)
    {
        if (biomeTypes == null)
        {
            return new();
        }

        List<LocalizedText> biomesTextIds = new();
        for (int i = 0; i < biomeTypes.Count; i++)
        {
            string[] biomesIds = BiomeInfo.BiomeTextIds[biomeTypes[i]];

            for (int j = 0; j < biomesIds.Length; j++)
            {
                biomesTextIds.Add(new LocalizedText { index = biomesIds[j] });
            }
        }

        return biomesTextIds;
    }

    private void Awake()
    {
        TMPro = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        LocalizedText.OnLangugageChanged += UpdateLocalizedText;
    }

    private void OnDestroy()
    {
        LocalizedText.OnLangugageChanged -= UpdateLocalizedText;
    }

    private void UpdateLocalizedText()
    {
        try
        {
            string[] biomesTextArr = BiomesTextIds.Select(ids => LocalizedText.GetText(ids.index)).ToArray();

            TMPro.text = string.Join(Separator, biomesTextArr);
            if (string.IsNullOrEmpty(TMPro.text))
            {
                TMPro.text = MapOptionPrefab.Instance.InfoTextPlaceholder;
            }
        }
        catch (Exception e)
        {
            Plugin.Log.LogError("Issue with trying to update text for biomes.\n" + e.Message);
        }
    }
}
