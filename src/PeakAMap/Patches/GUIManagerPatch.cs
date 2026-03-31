using System;
using HarmonyLib;
using UnityEngine.SceneManagement;
using PeakAMap.Core;

namespace PeakAMap.Patches;
[HarmonyPatch(typeof(GUIManager))]
internal class GUIManagerPatch
{
    [HarmonyPatch(nameof(GUIManager.Start))]
    [HarmonyPostfix]
    private static void SaveLoadedMapBiomesPatch()
    {          
        if (MapHandler.Exists && !MapRotationHandler.Instance.CurrentlyLoading)
        {
            SaveLoadedMapBiomes();
        }
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
