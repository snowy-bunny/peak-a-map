using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zorro.Core;
using PeakAMap.Utilities;
using PeakAMap.UI;

namespace PeakAMap.Core;
public class MapsBoardHandler : Singleton<MapsBoardHandler>
{
    public TextMeshProUGUI SelectedMapCode { get; set; }

    public Transform MapsList { get; private set; }

    public Transform Header { get; private set; }

    private static Transform s_parent => GUIManager.instance.boardingPass.playerName.transform.parent.parent;

    public static void Instantiate()
    {
        Plugin.Log.LogInfo("Creating MapsBoard.");
        GameObject gameObject = Object.Instantiate(MapsBoard.Instance.gameObject);
        gameObject.transform.SetParent(s_parent, worldPositionStays: false);
        gameObject.AddComponent<MapsBoardHandler>();

        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.localScale = new Vector3(1, 1, 1);
    }

    protected override void Awake()
    {
        SelectedMapCode = transform.
            QueryChildren("Screen/Header/SelectedMap/SelectedMapCode")?.
            GetTMPro() 
            ?? new();

        MapsList = transform.
            QueryChildren("Screen/ScrollView/MapsList") 
            ?? new GameObject().transform;

        Header = transform.QueryChildren("Screen/Header")
            ?? new GameObject().transform;
    }

    private void Start()
    {
        UpdateSelectedMapCode();
        UpdateUIPosition();
        AddMapOptionFunctions();
        AddDropdownFunctions();
    }

    private void Update()
    {
        if (CustomMaps.Instance.loadMode == LoadMode.Daily)
        {
            UpdateSelectedMapCode();
        }
    }

    public override void OnDestroy()
    {
        RemoveButtonListeners();
        RemoveDropdownListener();
    }

    private void UpdateUIPosition()
    {
        RectTransform rect = s_parent.GetComponent<RectTransform>();
        rect.pivot = MapsBoardUI.boardingPassNewPivot;
    }

    private void AddMapOptionFunctions()
    {
        for (int i = 0; i < MapsList.childCount; i++)
        {
            MapsList.GetChild(i).TryGetComponent(out Button button);
            button.onClick.AddListener(delegate
            {
                SetPlayMap(button);
            });

            MapBiomes mapBiomes = MapsList.GetChild(i).GetComponentInChildren<MapBiomes>();
            mapBiomes.BiomesTextIds = MapBiomes.GetBiomesLocalizedText(MapRotationHandler.Instance.CurrMapRotation[i]);
        }
    }

    private void SetPlayMap(Button button)
    {
        string? mapCode = button.gameObject.QueryChildren("MapCode")?.GetTMPro().text;
        int index = int.Parse(mapCode);
        CustomMaps.Instance.CustomMapIndex = index;
        UpdateSelectedMapCode();
        Plugin.Log.LogInfo("Custom map set to " + index);
    }

    private void RemoveButtonListeners()
    {
        for (int i = 0; i < MapsList.childCount; i++)
        {
            MapsList.GetChild(i).TryGetComponent(out Button button);
            button.onClick.RemoveAllListeners();
        }
    }

    private void AddDropdownFunctions()
    {
        TMP_Dropdown dropdown = Header.GetComponentInChildren<TMP_Dropdown>();

        dropdown.onValueChanged.AddListener(delegate
        {
            UpdateDropdown(dropdown);
        });

        dropdown.value = (int)CustomMaps.Instance.loadMode;
        UpdateInteractableButtons(dropdown);
    }

    private void UpdateDropdown(TMP_Dropdown dropdown)
    {
        CustomMaps.Instance.loadMode = (LoadMode)dropdown.value;
        UpdateInteractableButtons(dropdown);
        Plugin.Log.LogInfo("Map mode set to " + LoadModeUtil.GetName((LoadMode)dropdown.value));
    }

    private void RemoveDropdownListener()
    {
        TMP_Dropdown dropdown = Header.GetComponentInChildren<TMP_Dropdown>();
        dropdown.onValueChanged.RemoveAllListeners();
    }

    private void UpdateInteractableButtons(TMP_Dropdown dropdown)
    {
        CanvasGroup? canvas = MapsList?.GetComponent<CanvasGroup>();
        if (canvas == null)
        {
            Plugin.Log.LogError("Maps List component not found. Cannot update interactability of buttons");
            return;
        }
        canvas.interactable = (LoadMode)dropdown.value == LoadMode.Custom;
        UpdateSelectedMapCode();
    }

    private void UpdateSelectedMapCode()
    {
        if (SelectedMapCode == null)
        {
            Plugin.Log.LogError("SelectedMapCode was not found. Cannot update selected map display.");
            return;
        }
        SelectedMapCode.text = CustomMaps.Instance.SelectedMapIndex.ToString(MapOption.MapCodeFormat);
    }
}
