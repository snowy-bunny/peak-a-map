namespace PeakAMap.Core;
public sealed class CustomMaps
{
    private static readonly CustomMaps _instance = new();

    private CustomMaps()
    {
        loadMode = LoadMode.Custom;
        _customMapIndex = DailyMapIndex;
    }

    static CustomMaps() { }
    
    public static CustomMaps Instance => _instance;

    public LoadMode loadMode { get; set; }

    private int _customMapIndex;

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

    public int SelectedMapIndex
    {
        get
        {
            return (loadMode == LoadMode.Custom) ? CustomMapIndex : DailyMapIndex;
        }
    }
}
