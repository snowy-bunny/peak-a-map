using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zorro.Core;
using PeakAMap.Utilities;

namespace PeakAMap.Core;
[Serializable]
public class MapRotation
{
    public string BuildVersion;

    public List<BiomeInfo>?[] MapBiomes;

    public List<BiomeInfo>? this[int i]
    {
        get => MapBiomes[i];
        private set => MapBiomes[i] = value;
    }

    public int Length => MapBiomes.Length;

    internal MapRotation()
    {
        BuildVersion = new BuildVersion(Application.version).ToString();
        MapBiomes = new List<BiomeInfo>[MapBaker.Instance.ScenePaths.Length];
    }


    public List<MapBaker.BiomeResult> biomeResults
    {
        get
        {
            List<MapBaker.BiomeResult> biomeResults = new();
            List<Biome.BiomeType> biomeTypes = new();
            foreach (List<BiomeInfo> map in MapBiomes)
            {
                biomeTypes.Clear();

                foreach (BiomeInfo biomeInfo in map)
                {
                    biomeTypes.Add(biomeInfo.biomeType);
                }

                biomeResults.Add(new MapBaker.BiomeResult(biomeTypes));
            }

            return biomeResults;
        }
    }

    public static MapRotation FromJson(string json)
    {
        MapRotation mapRotation = JsonUtility.FromJson<MapRotation>(json);
        string arrayJson = JsonExtended.GetValueFromJson(json, "MapBiomes", true);

        string[] mapBiomesJson = JsonExtended.SplitSections(arrayJson, '[', ']');

        mapRotation.MapBiomes = new List<BiomeInfo>[mapBiomesJson.Length];
        for (int i = 0; i < mapBiomesJson.Length; i++)
        {
            if (mapBiomesJson[i] == "null")
            {
                mapRotation.MapBiomes[i] = null;
            }
            else
            {
                mapRotation.MapBiomes[i] = JsonExtended.ListFromJson<BiomeInfo>(mapBiomesJson[i]);

            }
        }

        return mapRotation;
    }

    public static string ToJson(MapRotation mapRotation)
    {
        string json = JsonUtility.ToJson(mapRotation, true);
        if (json.Contains("\"MapBiomes\""))
        {
            return json;
        }
        string rotationJson = JsonExtended.CreateJsonObject("MapBiomes", MapBiomesToJson(mapRotation.MapBiomes), true);
        return JsonExtended.CombineJson(true, json, rotationJson);
    }

    private static string MapBiomesToJson(List<BiomeInfo>?[] mapBiomes)
    {
        string[] mapBiomesJsons = new string[mapBiomes.Length];
        List<BiomeInfo>? biomesInfo;
        for (int i = 0; i < mapBiomes.Length; i++)
        {
            biomesInfo = mapBiomes[i];
            if (biomesInfo == null)
            {
                mapBiomesJsons[i] = "null";
            }
            else
            {
                mapBiomesJsons[i] = JsonExtended.ListToJson(biomesInfo, true);

            }
        }
        return JsonExtended.EncaseCombine(mapBiomesJsons, '[', ']', true);
    }

    internal List<BiomeInfo>? FillBiomesInfo(int index, out GameObject? mapObject)
    {
        Scene scene = SceneManager.GetSceneByPath(MapBaker.Instance.ScenePaths[index]);
        mapObject = null;

        if (!scene.isLoaded)
        {
            Plugin.Log.LogWarning("Cannot get biomes info. Scene is not loaded.");
            return MapBiomes[index];
        }

        List<BiomeInfo> biomesInfo = [];
        GameObject[] rootObjects = scene.GetRootGameObjects();
        for (int i = 0; i < scene.rootCount; i++)
        {
            if (rootObjects[i].name != "Map")
            {
                continue;
            }
            mapObject = rootObjects[i];

            // Iterate through each possible Biome. (Similar to MapHandler.DetectBiomes().)
            for (int j = 0; j < mapObject.transform.childCount; j++)
            {
                Transform child = mapObject.transform.GetChild(j);
                for (int k = 0; k < child.childCount; k++)
                {
                    if (child.GetChild(k).gameObject.activeInHierarchy && 
                        child.GetChild(k).TryGetComponent(out Biome biome))
                    {
                        biomesInfo.Add(new BiomeInfo(biome, FindVariant(biome), CheckOpenTomb(biome)));
                    }
                }
            }

            break;
        }
        MapBiomes[index] = biomesInfo;
        return biomesInfo;
    }

    private bool? CheckOpenTomb(Biome biome)
    {
        if (biome.biomeType != Biome.BiomeType.Mesa)
        {
            return null;
        }

        Transform? entrance = biome.transform.QueryChildren("Desert_Segment/Platteau/Rocks/Timple/Enterences/2")?.GetChild(0);
        return entrance?.childCount == 2;
    }

    private Component? FindVariant(Biome biome)
    {
        return (biome.biomeType) switch
        {
            Biome.BiomeType.Mesa or
            Biome.BiomeType.Roots
                => biome.GetComponentInChildren(typeof(VariantObject)),

            Biome.BiomeType.Shore or
            Biome.BiomeType.Tropics or
            Biome.BiomeType.Alpine
                => biome.GetComponentInChildren(typeof(BiomeVariant)),

            Biome.BiomeType.Volcano or
            Biome.BiomeType.Peak 
                => null,

            _ => biome.GetComponentInChildren(typeof(VariantObject))
                ?? biome.GetComponentInChildren(typeof(BiomeVariant))
        };
    }
}
