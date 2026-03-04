using System.IO;
using UnityEngine;
using Zorro.Core;

namespace PeakAMap.Core;
public static class DataFilesHandler
{
    private static string s_dataDir = Path.GetFullPath(Plugin.LoadedInfo.Location + "/../data/");

    private static readonly string s_currVersion = new BuildVersion(Application.version).ToString();

    private static string s_fileName = $"map_rotation-{s_currVersion}.json";

    private static string s_currMapRotationPath = s_dataDir + s_fileName;

    public static string GetFileName(string version)
    {
        return $"map_rotation-{version}.json";
    }

    public static string GetPath(string version)
    {
        return s_dataDir + GetFileName(version);
    }

    internal static MapRotation? ParseMapRotation(string? version = null)
    {
        version ??= s_currVersion;
        string filename = GetFileName(version);
        string path = GetPath(version);
        string json = "";
        MapRotation? mapRotation = null;

        try
        {
            json = File.ReadAllText(path);
        }
        catch
        {
            Plugin.Log.LogWarning($"Cannot find file at {path}. Not able to load map rotation data.");
            return mapRotation;
        }

        try
        {
            mapRotation = MapRotation.FromJson(json);
        }
        catch
        {
            Plugin.Log.LogWarning($"Issue with data in {filename}. Not able to load map rotation data.");
            return mapRotation;
        }

        return mapRotation;
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
