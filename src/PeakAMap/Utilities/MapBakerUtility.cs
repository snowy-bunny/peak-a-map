namespace PeakAMap.Utilities;
public static class MapBakerUtility
{
    public static bool ValidSelectedBiomes(this MapBaker instance)
    {
        return instance.selectedBiomes.Count == instance.ScenePaths.Length;
    }
}