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
        CurrMapRotation = new();

        for (int i = 0; i < _sceneNames.Length; i++)
        {
            _sceneNames[i] = MapBaker.Instance.GetLevel(i);
            CurrMapRotation.FillBiomesInfoFromMapBaker(i);
        }
    }

    static MapRotationHandler() { }

    public static MapRotationHandler Instance => _instance;

    internal readonly string[] _sceneNames = new string[MapBaker.Instance.ScenePaths.Length];

    public MapRotation CurrMapRotation { get; private set; }

    public bool CurrentlyLoading { get; private set; } = false;

    internal bool _cancelIndicator = false;

    private readonly string _loadingId = "LOADING";

    private readonly int _maxLoadedScenes = 15;

    private bool IsValidMapBiome(int index)
    {
        // Modify to customize maps to preload.
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

            // Use different method of logging info (below was prev method)
            //DataFilesHandler.WriteMapRotation(CurrMapRotation);
        }

        button?.onClick.RemoveAllListeners();
        yield return EndSaveMapsLoad(returnScene, originalLoadingText);
        CurrentlyLoading = false;
    }

    private IEnumerator LoadSceneIndex(int index)
    {
        MapBundleService service = GameHandler.GetService<MapBundleService>();
        MapBundleService.MapBundleData bundle = service.LoadBundlesNearestIndex(index);
        yield return new WaitUntil(() => !bundle.Loading);

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
        tmp.text = "NOW CANCELING...";
    }
}
