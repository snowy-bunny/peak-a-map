using UnityEngine;
using TMPro;
using PeakAMap.Utilities;
using static UnityEngine.UI.Image;

namespace PeakAMap.UI;

public static class MapsBoardUI
{
    public static float ScreenWidth = BoardingPassUI.CustomOptions.GetRectTransform().sizeDelta.x;

    public static float ScreenHeight = BoardingPassUI.CustomOptions.GetRectTransform().sizeDelta.y;

    public static int roundedCornersAdjustment => 13;

    public static int HeaderHeight => 90;

    public static int VisibleRows = 11;

    public static int VisibleCols = 2;

    public static float MapsListWidth => ScreenWidth;

    public static float MapsListHeight => ScreenHeight - HeaderHeight;

    // Need to adjust for rounded corners from left and right sides.
    public static float CellWidth => (MapsListWidth - roundedCornersAdjustment * 2) / VisibleCols;

    // Need to adjust for rounded corners from bottom side.
    public static float CellHeight => (MapsListHeight - roundedCornersAdjustment) / VisibleRows;

    public static int MapOptionSpacing => 30;

    public static int MapCodeWidth => 30;

    public static float BiomesWidth => CellWidth - MapCodeWidth - (MapOptionSpacing * 3);

    public static Color MainFontColor = BoardingPassUI.Panel.gameObject.GetColor();

    public static Color SubtitleColor = new Color(MainFontColor.r, MainFontColor.g, MainFontColor.b, 0.55f);

    public static Color DarkFontColor = BoardingPassUI.AscentDescription.color;

    public static Color AccentColor = new Color(0.731f, 1, 0.998f, 1);     // Light Cyan

    public static Color MapCodeColor => AccentColor;

    public static Color ScreenColor = BoardingPassUI.boardingPass.defaultColor;

    public static Color Cell1Color = BoardingPassUI.BlueTop.GetColor();

    public static Color Cell2Color => ScreenColor;

    public static TMP_FontAsset MainFont = BoardingPassUI.PlayerName.font;

    public static TMP_FontAsset AirlineFont = PassportUI.PassportText.font;

    public static Sprite MainSprite = BoardingPassUI.Panel.sprite;

    public static Sprite RoughSprite = BoardingPassUI.IncrementAscentButton.image.sprite;

    public static Material MainMaterial = BoardingPassUI.Panel.material;

    public static Type MainImageType = BoardingPassUI.Panel.type;

    public static GameObject Dropdown = MainMenuUI.Dropdown;

    public static SFX_Instance buttonClickSFX = BoardingPassUI.CustomOptionItemTogglePrefab.sfxClick;

    public static SFX_Instance buttonHoverSFX = BoardingPassUI.CustomOptionItemTogglePrefab.sfxHover;

    public static int InfoFontSize => 20;

    public static int InfoFontSizeMin => 18;

    public static int InfoFontSizeMax => 24;

    public static int LineSimpleHeight => 24;

    public static int LineDetailsHeight => 28;

    public static float OpenButtonYPosition = BoardingPassUI.Title.GetRectTransform().anchoredPosition.y;
}
