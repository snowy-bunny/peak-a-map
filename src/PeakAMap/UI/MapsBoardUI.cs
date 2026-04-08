using UnityEngine;
using TMPro;
using PeakAMap.Utilities;
using static UnityEngine.UI.Image;

namespace PeakAMap.UI;

public static class MapsBoardUI
{
    // (0.5f, 0.5f) centers boardingPass.
    // Increase values to move boardingPass LEFT and DOWN.
    // Decrease values to move boardingPass RIGHT and UP.
    // (Note: Current values move the boardingPass down.)
    public static Vector2 boardingPassNewPivot = new Vector2(0.5f, 0.8f);

    // (0.5f, 0.5f) centers MapsBoard relative to boardingPass.
    // Increase values to move MapsBoard LEFT and DOWN.
    // Decrease values to move MapBoard RIGHT and UP.
    // (Note: Current values move the MapsBoard up.)
    public static Vector2 BoardPivot = new Vector2(0.5f, -0.8f);

    public static int BoardWidth = 1640;

    public static int BoardHeight = 350;

    public static int BezelSize = 15;

    public static int ScreenWidth => BoardWidth - BezelSize * 2;

    public static int ScreenHeight => BoardHeight - BezelSize * 2;

    public static int roundedCornersAdjustment => 8;

    public static int HeaderHeight = 80;

    public static int VisibleRows = 5;

    public static int VisibleCols = 3;

    public static float MapsListWidth => ScreenWidth;

    public static float MapsListHeight => ScreenHeight - HeaderHeight;

    // Need to adjust for rounded corners from left and right sides.
    public static float CellWidth => (MapsListWidth - roundedCornersAdjustment * 2) / VisibleCols;

    // Need to adjust for rounded corners from bottom side.
    public static float CellHeight => (MapsListHeight - roundedCornersAdjustment) / VisibleRows;

    public static int MapOptionSpacing => 20;

    public static int MapCodeWidth => 30;

    public static float BiomesWidth => CellWidth - MapCodeWidth - (MapOptionSpacing * 3);

    public static Color MainColor = boardingPassUI.Panel.gameObject.GetColor();

    public static Color SubtitleColor = BoardingPassUI.PlayerName.color;

    public static Color DarkFontColor = BoardingPassUI.AscentDescription.color;

    public static Color BoardColor = new Color(0.20f, 0.22f, 0.32f);        // Dark Grey

    public static Color MapCodeColor = new Color(0.731f, 1, 0.998f, 1);     // Light Cyan

    public static Color ScreenColor = boardingPassUI.boardingPass.startGameButton.image.color;

    public static Color Cell1Color = BoardingPassUI.BlueTop.GetColor();

    public static Color Cell2Color = ScreenColor;

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
