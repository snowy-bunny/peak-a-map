using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zorro.ControllerSupport;
using PeakAMap.Utilities;
using PeakAMap.UI;

namespace PeakAMap.Core;
public class MapsBoard : MenuWindow
{
    public BoardingPass boardingPass { get; set; }

    public static MapsBoard Instance { get; set; }

    public TextMeshProUGUI SelectedMapCode { get; set; }

    public GameObject MapsList { get; private set; }

    public GameObject Header { get; private set; }

    public Button CloseButton { get; private set; }

    public ConfigEntryToggle DisplayBiomesHUDToggle { get; private set; }

    public ConfigEntryToggle MoreDetailsToggle { get; private set; }

    public GameObject[] MapSelectedIndicators { get; private set; }


    public override bool selectOnOpen => true;

    public override bool closeOnPause => true;

    public override bool closeOnUICancel => true;

    public static void Instantiate()
    {
        Plugin.Log.LogInfo("Creating Canvas_MapsBoard.");
        GameObject canvas = Object.Instantiate(GUIManager.instance.boardingPass.customOptionsWindow.gameObject);
        Object.Destroy(canvas.GetComponent<CustomOptionsWindow>());
        Object.Destroy(canvas.QueryChildren("CustomOptions"));
        Object.Destroy(canvas.QueryChildren("Option_LINEBREAK"));
        canvas.transform.SetParentAndScale(GUIManager.instance.transform, worldPositionStays: false);

        RectTransform canvasRect = canvas.GetRectTransform();
        canvasRect.anchoredPosition = new Vector2(0, 0);

        canvas.name = "Canvas_MapsBoard";
        GameObject board = Object.Instantiate(MapsBoardPrefab.Instance.gameObject);
        board.StripCloneInName();
        board.transform.SetParentAndScale(canvas.transform, worldPositionStays: false);

        RectTransform boardRect = canvas.GetRectTransform();
        boardRect.anchoredPosition = new Vector2(0, 0);

        GameObject scrollView = board.QueryChildren("Screen/ScrollView");
        GameObject mapsList = scrollView.QueryChildren("MapsList");

        MapsBoardPrefab.Instance.CreateMapOptions(mapsList);
        MapsBoardPrefab.Instance.DynamicMapsList(mapsList, scrollView);

        Instance = canvas.AddComponent<MapsBoard>();
    }

    private void Awake()
    {
        boardingPass = GUIManager.instance.boardingPass;

        SelectedMapCode = gameObject
            .QueryChildren("MapsBoard/Screen/Header/SelectedMap/SelectedMapCode")
            .GetComponent<TextMeshProUGUI>();

        MapsList = gameObject.QueryChildren("MapsBoard/Screen/ScrollView/MapsList");

        Header = gameObject.QueryChildren("MapsBoard/Screen/Header");

        CloseButton = Header.QueryChildren("CloseButton")
            .GetComponent<Button>();

        DisplayBiomesHUDToggle = Header.QueryChildren($"ConfigOptions/{ConfigEntryToggle.ConfigNames.ShowBiomesInHUD}")
            .GetComponent<ConfigEntryToggle>();

        MoreDetailsToggle = Header.QueryChildren($"ConfigOptions/{ConfigEntryToggle.ConfigNames.ShowBiomeDetails}")
            .GetComponent<ConfigEntryToggle>();

        MapSelectedIndicators = MapsList
            .GetComponentsInChildren<MapOption>()
            .Select(option => option.MapCodeBorder)
            .ToArray();
    }

    public override void Initialize()
    {
        CloseButton.onClick.AddListener(base.Close);
        UpdateSelectedMapCode();
        AddMapOptionFunctions();
        AddDropdownFunctions();
        MoreDetailsToggle.toggle.onValueChanged.AddListener(delegate
        {
            MoreDetailsToggle.OnClick();
            RefreshBiomeDetailsActive(MoreDetailsToggle);
        });
        DisplayBiomesHUDToggle.toggle.onValueChanged.AddListener(delegate
        {
            DisplayBiomesHUDToggle.OnClick();
        });
    }

    private new void Update()
    {
        if (isOpen)
        {
            INavigationContainer.PushActive(this);
            if (CustomMaps.Instance.loadMode == LoadMode.Daily)
            {
                UpdateSelectedMapCode();
            }
        }
        TestCloseViaInput();
    }

    private new void OnDestroy()
    {
        if (AllActiveWindows.Contains(this))
        {
            AllActiveWindows.Remove(this);
        }
        RemoveButtonListeners();
        RemoveDropdownListener();
        MoreDetailsToggle.toggle.onValueChanged.RemoveAllListeners();
        DisplayBiomesHUDToggle.toggle.onValueChanged.RemoveAllListeners();
    }

    public override void OnClose()
    {
        base.OnClose();
        OpenBoardingPass();
    }

    private void OpenBoardingPass()
    {
        boardingPass.customOptionsWindow.OpenBoardingPass();
    }

    private void AddMapOptionFunctions()
    {
        for (int i = 0; i < MapsList.transform.childCount; i++)
        {
            MapsList.transform.GetChild(i).TryGetComponent(out Button button);
            button.onClick.AddListener(delegate 
            { 
                SetPlayMap(button);
            });
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
        CloseButton.onClick.RemoveAllListeners();

        for (int i = 0; i < MapsList.transform.childCount; i++)
        {
            MapsList.transform.GetChild(i).TryGetComponent(out Button button);
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
        Plugin.Log.LogInfo("Map mode set to " + CustomMaps.Instance.LoadModeName);
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
        UpdateButtonsSFX(canvas.interactable);
    }

    private void UpdateSelectedMapCode()
    {
        if (SelectedMapCode == null)
        {
            Plugin.Log.LogError("SelectedMapCode was not found. Cannot update selected map display.");
            return;
        }
        SelectedMapCode.text = CustomMaps.Instance.SelectedMapIndex.ToString(MapOption.MapCodeFormat);

        for (int i = 0; i < MapSelectedIndicators.Length; i++)
        {
            MapSelectedIndicators[i].SetActive(i == CustomMaps.Instance.SelectedMapIndex);
        }
    }

    private void UpdateButtonsSFX(bool playSFX)
    {
        for (int i = 0; i < MapsList.transform.childCount; i++)
        {
            MapsList.transform.GetChild(i).TryGetComponent(out NicksButtonSFX sfx);
            sfx.enabled = playSFX;
        }
    }

    private void RefreshBiomeDetailsActive(ConfigEntryToggle toggle)
    {
        MapOption mapOption;
        for (int i = 0; i < MapsList.transform.childCount; i++)
        {
            mapOption = MapsList.transform.GetChild(i).GetComponent<MapOption>();
            mapOption.RefreshDetailsActive();
        }
    }
}
