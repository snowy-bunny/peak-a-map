using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BepInEx.Configuration;
using PeakAMap.UI;
using PeakAMap.Utilities;

namespace PeakAMap.Core;

public class ConfigEntryToggle : CustomOptionBase
{
    public enum ConfigNames
    {
        ShowBiomesInHUD,
        ShowBiomeDetails
    }

    public static Dictionary<string, ConfigEntry<bool>> entryConfigs = new Dictionary<string, ConfigEntry<bool>>
    {
        { nameof(ConfigNames.ShowBiomesInHUD), UserConfig.ShowBiomesInHUD },
        { nameof(ConfigNames.ShowBiomeDetails), UserConfig.ShowBiomeDetails }
    };
    public static Dictionary<string, string> entryLabels = new Dictionary<string, string>
    {
        { nameof(ConfigNames.ShowBiomesInHUD), "DISPLAY BIOMES IN RUN" },
        { nameof(ConfigNames.ShowBiomeDetails), "MORE DETAILS" }
    };    

    public Toggle toggle { get; set; }

    public TextMeshProUGUI tmp { get; set; }

    public ConfigEntry<bool> entry { get; set; }

    public static GameObject Instantiate(string name, Transform parent)
    {
        GameObject gameObject = Object.Instantiate(ConfigEntryTogglePrefab.Instance.gameObject, parent.transform);
        gameObject.name = name;

        ConfigEntryToggle entry = gameObject.AddComponent<ConfigEntryToggle>();
        entry.sfxClick = MapsBoardUI.buttonClickSFX;
        entry.sfxHover = MapsBoardUI.buttonHoverSFX;

        return gameObject;
    }

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Toggle"))
            {
                toggle = child.GetComponent<Toggle>();
                continue;
            }

            if (child.name.Equals("Title"))
            {
                tmp = child.gameObject.GetTMPro();
                tmp.text = entryLabels[name];
            }
        }

        entry = entryConfigs[name];
        toggle.gameObject.GetComponent<CustomOptionButtonHelper>().option = this;

        Refresh();
    }

    private void Refresh()
    {
        toggle.SetIsOnWithoutNotify(entry.Value);
    }

    private void OnEnable()
    {
        Refresh();
    }

    public override void OnClick()
    {
        base.OnClick();
        entry.Value = !entry.Value;
        Refresh();
    }
}