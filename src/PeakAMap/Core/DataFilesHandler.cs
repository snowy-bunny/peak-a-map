using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;
using PeakAMap.Utilities;

namespace PeakAMap.Core;
public static class DataFilesHandler
{
    private static string s_dataDir = Path.GetFullPath(Plugin.LoadedInfo.Location + "/../data/");

    private static readonly string s_currVersion = new BuildVersion(Application.version).ToString();

    private static string s_fileName = $"map_rotation-{s_currVersion}.json";

    private static string s_currMapRotationPath = s_dataDir + s_fileName;

    private static string[] s_dataFiles = Directory.GetFiles(s_dataDir, "*.json").OrderByDescending(s => s).ToArray();

    public static string GetDataFileName(string version)
    {
        return $"map_rotation-{version}.json";
    }

    public static string GetDataPath(string version)
    {
        return s_dataDir + GetDataFileName(version);
    }

    internal static MapRotation? ParseMapRotation(string? version = null)
    {
        version ??= s_currVersion;
        string filename = GetDataFileName(version);
        string path = GetDataPath(version);
        string json = "";

        if (s_dataFiles.Contains(path))
        {
            try
            {
                json = File.ReadAllText(path);
                return MapRotation.FromJson(json);
            }
            catch { }
        }

        Plugin.Log.LogWarning($"Cannot get data from {filename}. Trying to get data from other files.");
        return FallbackMapRotationFiles();
    }

    private static MapRotation? FallbackMapRotationFiles()
    {
        string json = "";
        string path = "";

        for (int i = 0; i < s_dataFiles.Length; i++)
        {
            try
            {
                path = s_dataFiles[i];
                json = File.ReadAllText(path);
                MapRotation mapRotation = MapRotation.FromJson(json);
                if (MapBaker.Instance.ValidSelectedBiomes() && IsIdenticalToSelectedMaps(mapRotation))
                {
                    Plugin.Log.LogWarning($"Found file {Path.GetFileName(path)} with identical biomes to use as current map rotation data. " +
                        "Biome information details may contain inaccuracies as a result.");
                    return mapRotation;
                }
            }
            catch { }
        }

        Plugin.Log.LogError($"Cannot find data from other files. Not able to load map rotation data.");
        return null;
    }

    private static bool IsIdenticalToSelectedMaps(MapRotation mapRotation)
    {
        List<MapBaker.BiomeResult> biomeResults = mapRotation.biomeResults;

        if (biomeResults.Count != MapBaker.Instance.selectedBiomes.Count)
        {
            return false;
        }

        for (int j = 0; j < biomeResults.Count; j++)
        {
            if (!MapBaker.Instance.selectedBiomes[j].IsIdenticalTo(biomeResults[j]))
            {
                return false;
            }
        }

        return true;
    }

    internal static void WriteMapRotation(MapRotation data)
    {
        try
        {
            Directory.CreateDirectory(s_dataDir);
        }
        catch { }

        try
        {
            File.WriteAllText(s_currMapRotationPath, MapRotation.ToJson(data));
        }
        catch
        {
            Plugin.Log.LogError($"Failed to save data to file. Issue with writing to {s_currMapRotationPath}");
        }
    }
}
