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

    public const int NumInfo = 3;

    public int BiomeTypeInt;

    public string Variant;

    public bool OpenTomb;

    public bool HasVariant => !string.IsNullOrEmpty(Variant);

    public Biome.BiomeType biomeType => (Biome.BiomeType)BiomeTypeInt;

    public string[] TextId => BiomeTextIds[biomeType];

    public BiomeInfo(Biome biome, Component? variant = null, bool? openTomb = false)
    {
        BiomeTypeInt = (int)biome.biomeType;
        Variant = CleanVariant(variant?.name ?? "", biome.biomeType);
        OpenTomb = openTomb ?? false;
    }

    public BiomeInfo(int biomeTypeInt, string variant = "", bool openTomb = false)
    {
        BiomeTypeInt = biomeTypeInt;
        Variant = CleanVariant(variant, (Biome.BiomeType)biomeTypeInt);
        OpenTomb = openTomb;
    }

    private static HashSet<string> s_rootsIgnoreInVariant = new HashSet<string>
    { 
        "Redwoods", 
        "Redwood", 
        "Variant" 
    };

    public static string CleanVariant(string variant, Biome.BiomeType biome)
    {
        if (biome == Biome.BiomeType.Roots)
        {
            return CleanRootsVariant(variant);
        }
        if (biome == Biome.BiomeType.Mesa)
        {
            return CleanMesaVariant(variant);
        }
        return variant;
    }

    private static string CleanRootsVariant(string variant)
    {
        string[] cleanArr = variant.Split(' ', '-');
        string cleanSub;
        for (int i = 1; i < cleanArr.Length; i++)
        {
            cleanSub = cleanArr[i];

            if (cleanSub.Length > 0)
            {
                cleanSub = cleanSub.Substring(0, 1).ToUpper() + cleanSub.Substring(1).ToLower();
            }

            if (s_rootsIgnoreInVariant.Contains(cleanSub))
            {
                cleanSub = "";
            }

            cleanArr[i] = cleanSub;
        }

        return string.Join("", cleanArr);
    }

    private static string CleanMesaVariant(string variant)
    {
        if (variant.Equals("NoVariant"))
        {
            return "Default";
        }
        return variant;
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
