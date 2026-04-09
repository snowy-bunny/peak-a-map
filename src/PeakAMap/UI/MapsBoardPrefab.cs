using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BepInEx.Configuration;
using PeakAMap.Core;
using PeakAMap.Utilities;
using static PeakAMap.UI.MapsBoardUI;
using System.Collections.Generic;

namespace PeakAMap.UI;

internal sealed class MapsBoardPrefab
{
    private static readonly MapsBoardPrefab _instance = new();

    private MapsBoardPrefab() { Initialize(); }

    static MapsBoardPrefab() { }

    public static MapsBoardPrefab Instance => _instance;

    public GameObject gameObject { get; private set; }

    public GameObject Line { get; set; }

    private void Initialize()
    {
        gameObject = SetBoard();
        DontDestroy.Add(gameObject);

        GameObject screen = CreateScreen(gameObject);
        GameObject header = CreateHeader(screen);
        GameObject scrollView = CreateScrollView(screen);
        GameObject viewport = CreateViewport(scrollView);
        GameObject scrollbar = CreateScrollbar(scrollView);
        GameObject mapsList = CreateMapsList(viewport);
        GameObject title = CreateTitle(header);
        GameObject selectedMap = CreateSelectedMap(header);
        GameObject modeSelection = CreateModeSelection(header);
        GameObject logo = CreateLogo(title);
        GameObject options = CreateConfigOptions(header);
        CreateBorder(gameObject);
        CreateTitleText(title);
        CreatePlane(logo);
        CreateSelectedMapLabel(selectedMap);
        CreateSelectedMapCode(selectedMap);
        CreateModeLabel(modeSelection);
        CreateDropdown(modeSelection);
        CreateCloseButton(header);
        CreateConfigEntryToggleUI(options);

        MapOptionPrefab.Instance.Line.transform
            .SetParentAndScale(gameObject.transform);
    }

    private GameObject SetBoard()
    {
        GameObject board = new("MapsBoard", typeof(RectTransform));

        RectTransform rect = board.GetRectTransform();
        RectTransform rectRef = BoardingPassUI.CustomOptions.gameObject.GetRectTransform();
        rect.sizeDelta = rectRef.sizeDelta;
        rect.anchorMin = rectRef.anchorMin;
        rect.anchorMax = rectRef.anchorMax;
        rect.pivot = rectRef.pivot;

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
        Image imgRef = BoardingPassUI.CustomWindowPanel.gameObject.GetImage();
        img.color = ScreenColor;
        img.sprite = MainSprite;
        img.material = MainMaterial;
        img.type = MainImageType;
        img.pixelsPerUnitMultiplier = imgRef.pixelsPerUnitMultiplier;

        return screen;
    }

    private GameObject CreateBorder(GameObject parent)
    {
        GameObject border = Object.Instantiate(BoardingPassUI.Border);
        border.name = "Border";
        border.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        return border;
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

    private GameObject CreateCloseButton(GameObject parent)
    {
        Button button = Object.Instantiate(BoardingPassUI.CloseButtonCustom);
        button.onClick.RemoveAllListeners();
        button.transform.SetParentAndScale(parent.transform);

        GameObject close = button.gameObject;
        close.name = "CloseButton";

        RectTransform rect = close.GetRectTransform();
        rect.anchoredPosition = new Vector2(-30, 0);
        rect.anchorMin = new Vector2(1, 0.5f);
        rect.anchorMax = new Vector2(1, 0.5f);
        rect.pivot = new Vector2(1, 0.5f);

        return close;
    }

    private GameObject CreateScrollView(GameObject parent)
    {
        GameObject view = new("ScrollView", typeof(RectTransform), typeof(ScrollRect), typeof(Mask), typeof(Image));
        view.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = view.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, -HeaderHeight);
        rect.sizeDelta = new Vector2(-roundedCornersAdjustment*2, -HeaderHeight-roundedCornersAdjustment);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0.5f, 1);

        ScrollRect scrollRect = view.GetComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
        scrollRect.scrollSensitivity = 7;
        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;

        Image img = view.GetImage();
        img.color = ScreenColor;
        img.material = MainMaterial;
        img.type = MainImageType;

        return view;
    }

    private GameObject CreateViewport(GameObject parent)
    {
        GameObject list = new("Viewport", typeof(RectTransform));
        list.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = list.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0.5f);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.sizeDelta = new Vector2(0, 0);

        return list;
    }

    private GameObject CreateScrollbar(GameObject parent)
    {
        GameObject bar = Object.Instantiate(scrollbar);
        bar.StripCloneInName();
        bar.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = bar.GetRectTransform();
        rect.sizeDelta = new Vector2(20, 0);

        return bar;
    }

    private GameObject CreateMapsList(GameObject parent)
    {
        GameObject list = new("MapsList", typeof(RectTransform), typeof(GridLayoutGroup), typeof(CanvasGroup));
        list.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = list.GetRectTransform();
        RectTransform rectParent = parent.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(0, rectParent.rect.height);
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0, 1);

        GridLayoutGroup layout = list.GetComponent<GridLayoutGroup>();
        layout.childAlignment = TextAnchor.UpperLeft;
        layout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        layout.startAxis = GridLayoutGroup.Axis.Vertical;
        layout.constraint = GridLayoutGroup.Constraint.Flexible;
        layout.constraintCount = VisibleCols;

        float cellWidth = rect.rect.width / VisibleCols;
        float cellHeight = rect.rect.height / VisibleRows;
        layout.cellSize = new Vector2(cellWidth, cellHeight);

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
        rect.anchoredPosition = new Vector2(0, 0);
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.pivot = new Vector2(0.5f, 0.5f);

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
        txt.color = MainFontColor;
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
        rect.anchoredPosition = new Vector2(-450, 0);
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
        img.color = MainFontColor;
        img.sprite = MainSprite;
        img.material = MainMaterial;
        img.type = MainImageType;
        img.pixelsPerUnitMultiplier = 12;

        return logo;
    }
    private GameObject CreateTitleText(GameObject parent)
    {
        GameObject titleText = new("MAP ROTATION", typeof(RectTransform), typeof(TextMeshProUGUI));
        titleText.transform.SetParent(parent.transform, worldPositionStays: false);

        RectTransform rect = titleText.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.localScale = new Vector3(1.2f, 1, 1);
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(0, 0.5f);

        TextMeshProUGUI txt = titleText.GetTMPro();
        txt.text = "<b>MAP ROTATION</b>";
        txt.font = AirlineFont;
        txt.color = MainFontColor;
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
        GameObject plane = Object.Instantiate(BoardingPassUI.Plane).gameObject;
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
        txt.color = MainFontColor;
        txt.fontSize = 36;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Right;
        txt.verticalAlignment = VerticalAlignmentOptions.Geometry;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        txt.overflowMode = TextOverflowModes.Overflow;

        return modeLabel;
    }

    private GameObject CreateDropdown(GameObject parent)
    {
        GameObject loadDropdown = Object.Instantiate(dropdown);
        loadDropdown.StripCloneInName();
        Object.Destroy(dropdown);

        loadDropdown.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = loadDropdown.GetRectTransform();
        rect.anchoredPosition = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(165, 50);
        rect.anchorMin = new Vector2(1, 0.5f);
        rect.anchorMax = new Vector2(1, 0.5f);
        rect.pivot = new Vector2(1, 0.5f);

        TMP_Dropdown drop = loadDropdown.GetComponent<TMP_Dropdown>();
        drop.onValueChanged.RemoveAllListeners();
        drop.ClearOptions();
        foreach (string label in LoadModeUtil.Names)
        {
            drop.AddOptions([label.ToUpper()]);
        }

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

    internal void CreateMapOptions(GameObject parent)
    {
        int total = System.Math.Max(
            MapRotationHandler.Instance.CurrMapRotation.Length,
            MapBaker.Instance.ScenePaths.Length);
        for (int i = 0; i < total; i++)
        {
            MapOption.Instantiate(i, parent.transform);
        }
    }

    internal void DynamicMapsList(GameObject mapsList, GameObject scrollView, GameObject viewport, GameObject scrollbar)
    {
        RectTransform rect = mapsList.GetRectTransform();
        GridLayoutGroup layout = mapsList.GetComponent<GridLayoutGroup>();
        ScrollRect scroll = scrollView.GetComponent<ScrollRect>();

        RectTransform viewportRect = viewport.GetComponent<RectTransform>();

        scroll.content = rect;
        scroll.viewport = viewportRect;
        scroll.verticalScrollbar = scrollbar.GetComponent<Scrollbar>();
        MapOption.UpdateAllBgColors(mapsList);

        int numRows = (int)System.Math.Ceiling((decimal)mapsList.transform.childCount / VisibleCols);

        // Check for overflow
        if (numRows <= VisibleRows)
        {
            return;
        }

        // Adjust height of MapsList to number of rows.
        float cellHeight = layout.cellSize.y;
        float totalHeight = cellHeight * numRows;
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, totalHeight);
        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        
        // Adjust width to account for scrollbar
        float cellWidth = (rect.rect.width - scrollbar.GetRectTransform().sizeDelta.x) / VisibleCols;
        layout.cellSize = new Vector2(cellWidth, layout.cellSize.y);
    }
}
