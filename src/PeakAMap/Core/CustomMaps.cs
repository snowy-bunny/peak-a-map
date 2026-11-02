using System;

namespace PeakAMap.Core;
public sealed class CustomMaps
{
    private static readonly CustomMaps _instance = new();

    private CustomMaps()
    {
        loadMode = LoadMode.Custom;
        _customMapIndex = DailyMapIndex;
        _random = new Random();
    }

    static CustomMaps() { }
    
    public static CustomMaps Instance => _instance;

    public LoadMode loadMode { get; set; }

    public string LoadModeName => LoadModeUtil.GetName(loadMode);

    private int _customMapIndex;

    private Random _random;

    public int CustomMapIndex
    {
        get { return _customMapIndex; }
        set { _customMapIndex = value % MapBaker.Instance.ScenePaths.Length; }
    }

    public int DailyMapIndex
    {
        get
        {
            NextLevelService service = GameHandler.GetService<NextLevelService>();
            int mapIndex = service.NextLevelIndexOrFallback + NextLevelService.debugLevelIndexOffset;
            mapIndex %= MapBaker.Instance.ScenePaths.Length;
            return mapIndex;
        }
    }

    public int RandomMapIndex
    {
        get { return _random.Next(0, MapBaker.Instance.ScenePaths.Length); }
    }

    public int SelectedMapIndex
    {
        get
        {
            return (loadMode) switch
            {
                LoadMode.Custom => CustomMapIndex,
                LoadMode.Random => RandomMapIndex,
                _ => DailyMapIndex
            };
        }
    }
}
