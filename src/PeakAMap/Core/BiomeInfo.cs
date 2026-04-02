using System;
using System.Collections.Generic;
using UnityEngine;

namespace PeakAMap.Core;
[Serializable]
public class BiomeInfo
{
    public static readonly Dictionary<Biome.BiomeType, string[]> BiomeTextIds = new Dictionary<Biome.BiomeType, string[]>
    {
        { Biome.BiomeType.Shore, ["SHORE"] },
        { Biome.BiomeType.Tropics, ["TROPICS"] },
        { Biome.BiomeType.Alpine, ["ALPINE"] },
        { Biome.BiomeType.Volcano, ["CALDERA", "THE KILN"] },
        { Biome.BiomeType.Peak, ["PEAK"] },
        { Biome.BiomeType.Mesa, ["MESA"] },
        { Biome.BiomeType.Roots, ["ROOTS"] } 
    };

    public int BiomeTypeInt;

    public string Variant;

    public bool OpenTomb;

    public Biome.BiomeType biomeType => (Biome.BiomeType)BiomeTypeInt;

    public string[] TextId => BiomeTextIds[biomeType];

    public BiomeInfo(Biome biome, Component? variant = null, bool? openTomb = false)
    {
        BiomeTypeInt = (int)biome.biomeType;
        Variant = variant?.name ?? "";
        OpenTomb = openTomb ?? false;
    }

    public BiomeInfo(int biomeTypeInt, string variant = "", bool openTomb = false)
    {
        BiomeTypeInt = biomeTypeInt;
        Variant = variant;
        OpenTomb = openTomb;
    }

    public override string ToString()
    {
        string text = string.Join(", ", TextId);
        if (!(string.IsNullOrEmpty(Variant)))
        {
            text += $" ({Variant})";
        }
        if ((Biome.BiomeType)BiomeTypeInt == Biome.BiomeType.Mesa)
        {
            text += $" [OpenTomb: {OpenTomb}]";
        }
        return text;
    }
}
