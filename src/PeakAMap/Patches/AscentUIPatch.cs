using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using TMPro;
using PeakAMap.Core;
using PeakAMap.Utilities;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(AscentUI))]
internal class AscentUIPatch
{
    [HarmonyPatch(nameof(AscentUI.Start))]
    [HarmonyPostfix]
    private static void AddBiomesListToHUDPatch(ref TextMeshProUGUI ___text)
    {
        if (MapHandler.Exists && !MapRotationHandler.Instance.CurrentlyLoading)
        {
            AddBiomesListToHUD(___text);
        }
    }

    private static void AddBiomesListToHUD(TextMeshProUGUI refTmp)
    {
        if (!(UserConfig.ShowBiomesInHUD.Value))
        {
            Plugin.Log.LogInfo("ShowBiomesInHUD set to false. Not creating biomes text.");
            return;
        }

        Plugin.Log.LogInfo("ShowBiomesInHUD set to true. Creating biomes text.");
        GameObject mapBiomes = new GameObject("MapBiomes", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(MapBiomes));

        MapBiomes biomes = mapBiomes.GetComponent<MapBiomes>();
        List<LocalizedText> localizedTxts = MapBiomes.GetBiomesLocalizedText(MapHandler.Instance.biomes);
        biomes.BiomesTextIds = localizedTxts;

        GameObject parent = GUIManager.instance.hudCanvas.gameObject;
        mapBiomes.transform.SetParent(parent.transform, worldPositionStays: false);

        RectTransform refRect = refTmp.gameObject.GetRectTransform();
        RectTransform rect = mapBiomes.GetRectTransform();
        rect.localScale = refRect.localScale;
        rect.sizeDelta = new Vector2(refRect.sizeDelta.x * 5, refRect.sizeDelta.y);

        TextMeshProUGUI tmp = mapBiomes.GetTMPro();
        tmp.font = refTmp.font;
        tmp.color = refTmp.color;
        tmp.fontSize = refTmp.fontSize;
        tmp.verticalAlignment = refTmp.verticalAlignment;
        tmp.textWrappingMode = refTmp.textWrappingMode;
        tmp.overflowMode = refTmp.overflowMode;

        if (string.IsNullOrEmpty(refTmp.text))
        {
            rect.anchoredPosition = refRect.anchoredPosition;
            rect.anchorMin = refRect.anchorMax;
            rect.anchorMax = refRect.anchorMax;
            rect.pivot = refRect.pivot;
            tmp.horizontalAlignment = refTmp.horizontalAlignment;
        }
        else
        {
            rect.anchoredPosition = new Vector2(0, refRect.anchoredPosition.y);
            rect.anchorMin = new Vector2(0.5f, refRect.anchorMax.y);
            rect.anchorMax = new Vector2(0.5f, refRect.anchorMax.y);
            rect.pivot = new Vector2(0.5f, refRect.pivot.y);
            tmp.horizontalAlignment = HorizontalAlignmentOptions.Center;
        }
    }
}
