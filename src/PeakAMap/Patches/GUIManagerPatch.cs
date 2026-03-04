using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PeakAMap.Core;
using PeakAMap.Utilities;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(GUIManager))]
internal class GUIManagerPatch
{
    [HarmonyPatch(nameof(GUIManager.Start))]
    [HarmonyPostfix]
    private static void CreateMapBiomesList()
    {          
        if (MapHandler.Exists && !MapRotationHandler.Instance.CurrentlyLoading)
        {
            AddBiomesListToHUD();
            SaveLoadedMapBiomes();
        }
    }

    private static void AddBiomesListToHUD()
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

        TextMeshProUGUI refTmp = parent.GetComponentInChildren<AscentUI>().text;
        RectTransform refRect = refTmp.gameObject.GetRectTransform();
        RectTransform rect = mapBiomes.GetRectTransform();
        rect.anchoredPosition = 
            (Ascents._currentAscent == 0) ? 
            refRect.anchoredPosition : 
            new Vector2(refRect.anchoredPosition.x - 250, refRect.anchoredPosition.y);
        rect.localScale = refRect.localScale;
        rect.sizeDelta = new Vector2(refRect.sizeDelta.x * 5, refRect.sizeDelta.y);
        rect.anchorMin = refRect.anchorMax;
        rect.anchorMax = refRect.anchorMax;
        rect.pivot = refRect.pivot;

        TextMeshProUGUI tmp = mapBiomes.GetTMPro();
        tmp.font = refTmp.font;
        tmp.color = refTmp.color;
        tmp.fontSize = refTmp.fontSize;
        tmp.horizontalAlignment = refTmp.horizontalAlignment;
        tmp.verticalAlignment = refTmp.verticalAlignment;
        tmp.textWrappingMode = refTmp.textWrappingMode;
        tmp.overflowMode = refTmp.overflowMode;
    }

    private static void SaveLoadedMapBiomes()
    {
        Scene currentScene = MapHandler.Instance.gameObject.scene;
        int index = Array.IndexOf(MapRotationHandler.Instance._sceneNames, currentScene.name);
        if (!(MapRotationHandler.Instance.CurrMapRotation[index] == null))
        {
            Plugin.Log.LogInfo("Map info already saved. No need to save info.");
            return;
        }

        Plugin.Log.LogInfo("Map info wasn't saved. Now saving info.");
        MapRotationHandler.Instance.CurrMapRotation.FillBiomesInfo(index, out _);
        DataFilesHandler.WriteMapRotation(MapRotationHandler.Instance.CurrMapRotation);
    }
}
