using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PeakAMap.Utilities;

namespace PeakAMap.Core;
public sealed class MapRotationHandler
{
    private static readonly MapRotationHandler _instance = new();

    private MapRotationHandler()
    {
        CurrMapRotation = DataFilesHandler.ParseMapRotation() ?? new();

        for (int i = 0; i < _sceneNames.Length; i++)
        {
            _sceneNames[i] = MapBaker.Instance.GetLevel(i);
        }
    }

    static MapRotationHandler() { }

    public static MapRotationHandler Instance => _instance;

    internal readonly string[] _sceneNames = new string[MapBaker.Instance.ScenePaths.Length];

    public MapRotation CurrMapRotation { get; private set; }

    public bool CurrentlyLoading { get; private set; } = false;

    private bool _prevFoundAllMapBiomes = false;

    private bool FoundAllMapBiomes
    {
        get
        {
            if (_prevFoundAllMapBiomes)
            {
                return true;
            }

            for (int i = 0; i < _sceneNames.Length; i++)
            {
                if (!IsValidMapBiome(i))
                {
                    return false;
                }
            }

            _prevFoundAllMapBiomes = true;
            return true;
        }
    }

    internal bool _cancelIndicator = false;

    public bool NeedToLoad
    {
        get
        {
            return UserConfig.LoadMapsOnStart.Value && (!FoundAllMapBiomes) && (!_cancelIndicator);
        }
    }

    private readonly string _loadingId = "LOADING";

    private readonly int _maxLoadedScenes = 15; 

    private bool IsValidMapBiome(int index)
    {
        return !(CurrMapRotation.MapBiomes[index] == null);
    }

    internal void LoadAllMaps()
    {
        LoadingScreenHandler.Instance.Load(LoadingScreen.LoadingScreenType.Basic, null, SaveMapsInfo());
    }

    private IEnumerator SaveMapsInfo()
    {
        CurrentlyLoading = true;
        Ascents.currentAscent = -1;
        string returnScene = SceneManager.GetActiveScene().name;
        string originalLoadingText = LocalizedText.GetText(_loadingId);
        AddLoadingScreenExtraUI(out Button? button);

        for (int i = 0; i < _sceneNames.Length; i++)
        {
            if (_cancelIndicator)
            {
                break;
            }

            if (IsValidMapBiome(i))
            {
                continue;
            }

            Plugin.Log.LogWarning($"Data at {_sceneNames[i]} was not found. Map info will be loaded and saved.");

            string progressText = $"{i + 1}/{_sceneNames.Length}";
            LocalizedText.mainTable[_loadingId][(int)LocalizedText.CURRENT_LANGUAGE] = $"{originalLoadingText} ({progressText})";

            yield return LoadSceneIndex(i);
            DataFilesHandler.WriteMapRotation(CurrMapRotation);
        }

        button?.onClick.RemoveAllListeners();
        yield return EndSaveMapsLoad(returnScene, originalLoadingText);
        CurrentlyLoading = false;
    }

    private IEnumerator LoadSceneIndex(int index)
    {
        if (SceneManager.loadedSceneCount < _maxLoadedScenes)
        {
            SceneManager.LoadSceneAsync(MapBaker.Instance.GetLevel(index), LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadSceneAsync(MapBaker.Instance.GetLevel(index), LoadSceneMode.Single);
        }

        Scene scene = SceneManager.GetSceneByPath(MapBaker.Instance.ScenePaths[index]);

        while (!scene.isLoaded)
        {
            yield return null;
        }

        CurrMapRotation.FillBiomesInfo(index, out GameObject? mapObject);
        UnityEngine.Object.Destroy(mapObject);
    }

    private IEnumerator EndSaveMapsLoad(string returnScene, string originalLoadingText)
    {
        LocalizedText.mainTable[_loadingId][(int)LocalizedText.CURRENT_LANGUAGE] = originalLoadingText;
        yield return SceneManager.LoadSceneAsync(returnScene, LoadSceneMode.Single);
    }

    private void AddLoadingScreenExtraUI(out Button? button)
    {
        button = null;
        UI.LoadingMapsScreenPrefab.Instance.Instantiate(null, out GameObject? description, out GameObject? cancelButton);
        if (cancelButton == null)
        {
            Plugin.Log.LogError("Cancel Button object was not found. Will not be able to exit out of loading screen.");
            return;
        }
        if (description == null)
        {
            return;
        }

        button = cancelButton.GetComponent<Button>();
        button.onClick.AddListener(delegate
        {
            IndicateCancel(description);
        });

        cancelButton.transform.parent.TryGetComponent(out CanvasGroup group);
        group.interactable = true;
    }

    private void IndicateCancel(GameObject description)
    {
        _cancelIndicator = true;
        TextMeshProUGUI tmp = description.GetTMPro();

        if (!FoundAllMapBiomes)
        {
            tmp.text = "DIDN'T FINISHED FINDING EVERY MAPS' INFO.\n" + 
                "SEARCH WILL CONTINUE ON NEXT LAUNCH.\n";
        }
        tmp.text += "NOW CANCELING...";
    }
}
