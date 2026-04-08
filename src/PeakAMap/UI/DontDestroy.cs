using UnityEngine;

namespace PeakAMap.UI;
internal sealed class DontDestroy
{
    private static readonly DontDestroy _instance = new();

    private DontDestroy() { Initialize(); }

    static DontDestroy() { }

    public static DontDestroy Instance => _instance;

    public GameObject gameObject { get; private set; }

    private void Initialize()
    {
        gameObject = new GameObject("PeakAMap");
        Object.DontDestroyOnLoad(gameObject);
    }

    public static void Add(GameObject child)
    {
        child.transform.SetParent(Instance.gameObject.transform);
    }
}