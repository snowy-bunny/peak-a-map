using System;
using HarmonyLib;
using UnityEngine.SceneManagement;
using PeakAMap.Core;

// DEBUG ONLY
//using UnityEngine.InputSystem;
//using UnityEngine;

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

    // DEBUG ONLY
    //[HarmonyPatch(nameof(GUIManager.LateUpdate))]
    //[HarmonyPrefix]
    //private static void CustomMapsChanged()
    //{
    //    if (Keyboard.current.rightCtrlKey.wasPressedThisFrame)
    //    {
    //        GameObject go = GameObject.Find("Map/BL_Airport/Fences/Check In desk/AirportGateKiosk");
    //        AirportCheckInKiosk kiosk = go.GetComponent<AirportCheckInKiosk>();
    //        kiosk.Interact_CastFinished(Character.AllCharacters[0]);
    //    }
    //    if (Keyboard.current.rightAltKey.wasPressedThisFrame)
    //    {
    //        MapsBoard.Instance.Open();
    //    }
    //    if (Keyboard.current.minusKey.wasPressedThisFrame)
    //    {
    //        LocalizedText.SetLanguage(AddLanguageIndex(-1));
    //    }
    //    if (Keyboard.current.equalsKey.wasPressedThisFrame)
    //    {
    //        LocalizedText.SetLanguage(AddLanguageIndex(1));
    //    }
    //}

    //private static int AddLanguageIndex(int i)
    //{
    //    int total = Enum.GetNames(typeof(LocalizedText.Language)).Length;
    //    int current = (int)LocalizedText.CURRENT_LANGUAGE;
    //    int next = (total + current + i) % total;
    //    return next;
    //}
}
