using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PeakAMap.Core;
using PeakAMap.Utilities;
using static PeakAMap.UI.MapsBoardUI;

namespace PeakAMap.UI;

internal sealed class MapsBoardPrefab
{
    private static readonly MapsBoardPrefab _instance = new();

    private MapsBoardPrefab() { Initialize(); }

    static MapsBoardPrefab() { }

    public static MapsBoardPrefab Instance => _instance;

    public static MapsBoard Instance => _instance;

    public GameObject gameObject { get; set; }

    private void Initialize()
    {
        gameObject = SetBoard();
        DontDestroy.Add(gameObject);

        GameObject screen = CreateScreen(gameObject);
        GameObject header = CreateHeader(screen);
        GameObject scrollView = CreateScrollView(screen);
        GameObject mapsList = CreateMapsList(scrollView);
        GameObject title = CreateTitle(header);
        GameObject selectedMap = CreateSelectedMap(header);
        GameObject modeSelection = CreateModeSelection(header);
        GameObject logo = CreateLogo(title);
        CreateTitleText(title);
        CreatePlane(logo);
        CreateSelectedMapLabel(selectedMap);
        CreateSelectedMapCode(selectedMap);
        CreateModeLabel(modeSelection);
        CreateDropdown(modeSelection);
        CreateMapOptions(mapsList);

        ScrollRect scroll = scrollView.GetComponent<ScrollRect>();
        scroll.content = mapsList.GetRectTransform();

        UpdateMapsListFromCount(mapsList);
    }

    private GameObject SetBoard()
    {
        GameObject board = new("MapsBoard", typeof(RectTransform), typeof(Image));

        RectTransform rect = board.GetRectTransform();
        rect.sizeDelta = new Vector2(BoardWidth, BoardHeight);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = BoardPivot;

        Image img = board.GetImage();
        img.color = BoardColor;
        img.sprite = MainSprite;
        img.material = MainMaterial;
        img.type = MainImageType;
        img.pixelsPerUnitMultiplier = 3;

        return board;
    }

    private GameObject CreateScreen(GameObject parent)
    {
        GameObject screen = new("Screen", typeof(RectTransform), typeof(Image), typeof(Mask));
        screen.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = screen.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(ScreenWidth, ScreenHeight);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        Image img = screen.GetImage();
        img.color = ScreenColor;
        img.sprite = MainSprite;
        img.material = MainMaterial;
        img.type = MainImageType;
        img.pixelsPerUnitMultiplier = 4;

        return screen;
    }

    private GameObject CreateHeader(GameObject parent)
    {
        GameObject header = new("Header", typeof(RectTransform));
        header.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = header.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, -roundedCornersAdjustment);
        rect.sizeDelta = new Vector2(0, HeaderHeight - roundedCornersAdjustment);
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 1);

        return header;
    }

    private GameObject CreateScrollView(GameObject parent)
    {
        GameObject view = new("ScrollView", typeof(RectTransform), typeof(ScrollRect), typeof(Mask), typeof(Image));
        view.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = view.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(ScreenWidth, ScreenHeight - HeaderHeight);
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 0);
        rect.pivot = new Vector2(0.5f, 0);

        ScrollRect scroll = view.GetComponent<ScrollRect>();
        scroll.horizontal = false;
        scroll.vertical = true;
        scroll.movementType = ScrollRect.MovementType.Clamped;
        scroll.scrollSensitivity = 3;

        Image img = view.GetImage();
        img.color = ScreenColor;
        img.material = MainMaterial;
        img.type = MainImageType;

        return view;
    }

    private GameObject CreateMapsList(GameObject parent)
    {
        GameObject list = new("MapsList", typeof(RectTransform), typeof(GridLayoutGroup), typeof(CanvasGroup));
        list.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = list.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(MapsListWidth, MapsListHeight);
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0, 1);

        GridLayoutGroup layout = list.GetComponent<GridLayoutGroup>();
        layout.childAlignment = TextAnchor.UpperLeft;
        layout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        layout.startAxis = GridLayoutGroup.Axis.Vertical;
        layout.cellSize = new Vector2(CellWidth, CellHeight); 
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = VisibleCols;
        layout.padding.left = roundedCornersAdjustment;
        layout.padding.right = roundedCornersAdjustment;
        layout.padding.bottom = roundedCornersAdjustment;

        return list;
    }

    private GameObject CreateTitle(GameObject parent)
    {
        GameObject title = new("Title", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        title.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = title.GetRectTransform();
        rect.anchoredPosition = new Vector2(40, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0, 0.5f);

        HorizontalLayoutGroup layout = title.GetComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.spacing = 20;
        layout.padding.left = 10;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childControlWidth = false;
        layout.childControlHeight = false;

        return title;
    }

    private GameObject CreateSelectedMap(GameObject parent)
    {
        GameObject selection = new("SelectedMap", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        selection.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = selection.GetRectTransform();
        rect.anchoredPosition = new Vector2(-460, 0);
        rect.localScale = new Vector3(1, 1, 1);
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 0.5f);

        HorizontalLayoutGroup layout = selection.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 15;
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childControlWidth = true;
        layout.childControlHeight = false;

        return selection;
    }

    private GameObject CreateSelectedMapLabel(GameObject parent)
    {
        GameObject selectedMapLabel = new("MAP:", typeof(RectTransform), typeof(TextMeshProUGUI));
        selectedMapLabel.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = selectedMapLabel.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(1, 0.5f);

        TextMeshProUGUI txt = selectedMapLabel.GetTMPro();
        txt.text = "MAP:";
        txt.font = MainFont;
        txt.color = MainColor;
        txt.fontSize = 36;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Right;
        txt.verticalAlignment = VerticalAlignmentOptions.Geometry;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        txt.overflowMode = TextOverflowModes.Overflow;

        return selectedMapLabel;
    }

    private GameObject CreateSelectedMapCode(GameObject parent)
    {
        GameObject selectedMapCode = new("SelectedMapCode", typeof(RectTransform), typeof(TextMeshProUGUI));
        selectedMapCode.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = selectedMapCode.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(225, 50);
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 0.5f);

        TextMeshProUGUI txt = selectedMapCode.GetTMPro();
        txt.text = "--";
        txt.font = MainFont;
        txt.color = MapCodeColor;
        txt.fontSize = 36;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Left;
        txt.verticalAlignment = VerticalAlignmentOptions.Geometry;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        txt.overflowMode = TextOverflowModes.Overflow;

        return selectedMapCode;
    }

    private GameObject CreateModeSelection(GameObject parent)
    {
        GameObject selection = new("ModeSelection", typeof(RectTransform), typeof(HorizontalLayoutGroup));
        selection.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = selection.GetRectTransform();
        rect.anchoredPosition = new Vector2(-30, 0);
        rect.sizeDelta = new Vector2(320, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 0.5f);

        HorizontalLayoutGroup layout = selection.GetComponent<HorizontalLayoutGroup>();
        layout.spacing = 20;
        layout.childAlignment = TextAnchor.MiddleRight;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.childControlWidth = false;
        layout.childControlHeight = false;

        return selection;
    }

    private GameObject CreateLogo(GameObject parent)
    {
        GameObject logo = new("Logo", typeof(RectTransform), typeof(Image));
        logo.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = logo.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(40, 40);
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0, 0.5f);
        rect.pivot = new Vector2(0, 0.5f);

        Image img = logo.GetImage();
        img.color = MainColor;
        img.sprite = MainSprite;
        img.material = MainMaterial;
        img.type = MainImageType;
        img.pixelsPerUnitMultiplier = 12;

        return logo;
    }
    private GameObject CreateTitleText(GameObject parent)
    {
        GameObject titleText = new("CURRENT MAP ROTATION", typeof(RectTransform), typeof(TextMeshProUGUI));

        titleText.transform.SetParent(parent.transform, worldPositionStays: false);

        RectTransform rect = titleText.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.localScale = new Vector3(1.2f, 1, 1);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0, 0.5f);

        TextMeshProUGUI txt = titleText.GetTMPro();
        txt.text = "<b>CURRENT MAP ROTATION</b>";
        txt.font = AirlineFont;
        txt.color = MainColor;
        txt.fontStyle = FontStyles.Italic;
        txt.fontWeight = FontWeight.Black;
        txt.fontSize = 30;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        txt.overflowMode = TextOverflowModes.Overflow;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Left;
        txt.verticalAlignment = VerticalAlignmentOptions.Geometry;
        txt.characterSpacing = -3;
        txt.wordSpacing = 20;

        return titleText;
    }

    private GameObject CreatePlane(GameObject parent)
    {
        GameObject plane = Object.Instantiate(boardingPassUI.Plane).gameObject;

        plane.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = plane.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(35, 35);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        Image img = plane.GetImage();
        img.color = ScreenColor;

        return plane;
    }

    private GameObject CreateModeLabel(GameObject parent)
    {
        GameObject modeLabel = new("MODE:", typeof(RectTransform), typeof(TextMeshProUGUI));
        modeLabel.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = modeLabel.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 0.5f);

        TextMeshProUGUI txt = modeLabel.GetTMPro();
        txt.text = "MODE:";
        txt.font = MainFont;
        txt.color = MainColor;
        txt.fontSize = 36;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Right;
        txt.verticalAlignment = VerticalAlignmentOptions.Geometry;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        txt.overflowMode = TextOverflowModes.Overflow;

        return modeLabel;
    }

    private GameObject CreateDropdown(GameObject parent)
    {
        GameObject loadDropdown = Object.Instantiate(MapsBoardUI.Dropdown);
        loadDropdown.StripCloneInName();
        Object.Destroy(MainMenuUI.Dropdown);

        loadDropdown.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = loadDropdown.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.localScale = new Vector3(1, 1, 1);
        rect.sizeDelta = new Vector2(225, 50);
        rect.anchorMin = new Vector2(1, 0.5f);
        rect.anchorMax = new Vector2(1, 0.5f);
        rect.pivot = new Vector2(1, 0.5f);

        TMP_Dropdown dropdown = loadDropdown.GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.ClearOptions();
        dropdown.AddOptions([.. LoadModeUtil.Names]);

        return loadDropdown;
    }

    private GameObject CreateConfigOptions(GameObject parent)
    {
        GameObject options = new GameObject("ConfigOptions", typeof(VerticalLayoutGroup));
        options.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        options.GetComponent<VerticalLayoutGroup>().spacing = -25;

        RectTransform rect = options.GetRectTransform();
        rect.sizeDelta = new Vector2(325, 0);
        rect.anchoredPosition = new Vector2(-100, 0);
        rect.pivot = new Vector2(1, 0.5f);
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 1);

        return options;
    }

    private void CreateConfigEntryToggleUI(GameObject parent)
    {
        GameObject config;
        foreach (KeyValuePair<string, ConfigEntry<bool>> item in ConfigEntryToggle.entryConfigs)
        {
            config = ConfigEntryToggle.Instantiate(item.Key, parent.transform);
        }
    }
    {
        for (int i = 0; i < MapRotationHandler.Instance.CurrMapRotation.Length; i++)
        {
            MapOption.Instantiate(i, parent.transform);
        }
    }

    private static void UpdateMapsListFromCount(GameObject mapsList)
    {
        RectTransform rect = mapsList.GetRectTransform();

        // Adjusting height of MapsList to number of rows.
        int numRows = mapsList.transform.childCount / MapsBoardUI.VisibleCols;
        float totalHeight = (MapsBoardUI.CellHeight * numRows) + MapsBoardUI.roundedCornersAdjustment;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, totalHeight);

        if (numRows <= MapsBoardUI.VisibleRows)
        {
            return;
        }

        // Adjusting background color of each cell in case of overflow.
        int colParity;
        int rowParity;
        Image img;
        for (int i = 0; i < mapsList.transform.childCount; i++)
        {
            colParity = (i / numRows) % 2;
            rowParity = (i - (numRows * colParity)) % 2;

            img = mapsList.transform.GetChild(i).GetImage();
            img.color = (colParity == rowParity) ? MapsBoardUI.Cell1Color : MapsBoardUI.Cell2Color;
        }
    }
}
