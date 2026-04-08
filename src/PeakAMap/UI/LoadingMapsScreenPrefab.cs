using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PeakAMap.Utilities;

namespace PeakAMap.UI;

public sealed class LoadingMapsScreenPrefab
{
    private static readonly LoadingMapsScreenPrefab _instance = new();

    private LoadingMapsScreenPrefab() { Initialize(); }

    static LoadingMapsScreenPrefab() { }

    public static LoadingMapsScreenPrefab Instance => _instance;

    public GameObject Description { get; set; }

    public GameObject QuitButton { get; set; }

    public GameObject gameObject { get; set; }

    private void Initialize()
    {
        gameObject = new GameObject("CustomLoadingMapsUI", typeof(RectTransform));
        Description = CreateDescription(gameObject);
        QuitButton = CreateQuitButton(gameObject);

        DontDestroy.Add(gameObject);
    }

    public void Instantiate(GameObject? parent = null)
    {
        Instantiate(parent, out GameObject? _, out GameObject? _);
    }

    public void Instantiate(GameObject? parent, out GameObject? description, out GameObject? quitButton)
    {
        description = null;
        quitButton = null;
        parent ??= GetLoadingScreen();

        if (parent == null)
        {
            Plugin.Log.LogError("No valid parent object was found. Cannot add description and quit button."); 
            return;
        }

        description = Object.Instantiate(Description);
        description.transform.SetParentAndScale(parent.transform, worldPositionStays: false);
        RectTransform descriptionRect = description.GetRectTransform();
        descriptionRect.anchoredPosition = new Vector2(35, -20);

        quitButton = Object.Instantiate(QuitButton);
        quitButton.transform.SetParentAndScale(parent.transform, worldPositionStays: false);
        RectTransform quitButtonRect = quitButton.GetRectTransform();
        quitButtonRect.anchoredPosition = new Vector2(-30, -25);
    }

    private GameObject? GetLoadingScreen()
    {
        Scene notDestroyedScene = GameHandler.Instance.gameObject.scene;
        GameObject[] rootObjects = notDestroyedScene.GetRootGameObjects();
        GameObject? loadingScreen = null;

        for (int i = 0; i < notDestroyedScene.rootCount; i++)
        {
            if (rootObjects[i].name.Equals("LoadingScreenSimple(Clone)"))
            {
                loadingScreen = rootObjects[i];
            }
        }

        return loadingScreen;
    }

    private GameObject CreateDescription(GameObject parent)
    {
        GameObject description = new GameObject("Description", typeof(RectTransform), typeof(TextMeshProUGUI));

        description.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = description.GetRectTransform();
        rect.sizeDelta = new Vector2(500, 100);
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);

        TextMeshProUGUI txt = description.GetTMPro();
        TextMeshProUGUI refTxt = MainMenuUI.VersionTMPro;
        txt.text = "COULD NOT FIND FULL FILE FOR THIS PATCH'S MAP ROTATION.\nNOW SEARCHING FOR MAP INFO MANUALLY...";
        txt.font = refTxt.font;
        txt.color = refTxt.color;
        txt.fontSize = 24;
        txt.horizontalAlignment = HorizontalAlignmentOptions.Left;
        txt.verticalAlignment = VerticalAlignmentOptions.Top;
        txt.textWrappingMode = TextWrappingModes.NoWrap;
        txt.overflowMode = TextOverflowModes.Overflow;
        txt.lineSpacing = 15;

        return description;
    }

    private GameObject CreateQuitButton(GameObject parent)
    {
        GameObject quitButton = Object.Instantiate(MainMenuUI.SettingsBackButton);

        quitButton.transform.SetParentAndScale(parent.transform, worldPositionStays: false);

        RectTransform rect = quitButton.GetRectTransform();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);

        quitButton.TryGetComponent(out Button button);
        button.onClick.RemoveAllListeners();

        LocalizedText localTxt = quitButton.GetComponentInChildren<LocalizedText>();
        localTxt.index = "QUIT";

        return quitButton;
    }
}
