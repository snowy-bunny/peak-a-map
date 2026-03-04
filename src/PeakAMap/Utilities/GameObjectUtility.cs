using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PeakAMap.Utilities;
public static class GameObjectUtility
{
    public static RectTransform GetRectTransform(this GameObject gameObject)
    {
        return gameObject.GetComponent<RectTransform>();
    }

    public static Image GetImage(this GameObject gameObject)
    {
        return gameObject.GetComponent<Image>();
    }

    public static Color GetColor(this GameObject gameObject)
    {
        return gameObject.GetComponent<Graphic>().color;
    }

    public static TextMeshProUGUI GetTMPro(this GameObject gameObject)
    {
        return gameObject.GetComponent<TextMeshProUGUI>();
    }

    public static GameObject? QueryChildren(this GameObject gameObject, string[] queries)
    {
        GameObject? results = gameObject.transform.QueryChildren(queries)?.gameObject;

        return results;
    }

    public static GameObject? QueryChildren(this GameObject gameObject, string queryOrPath)
    {
        GameObject? results = gameObject.transform.QueryChildren(queryOrPath)?.gameObject;

        return results;
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent(out T retrieved))
        {
            return retrieved;
        }
        return gameObject.AddComponent<T>();
    }

    public static GameObject PasteComponent<T>(this GameObject gameObject, T component)
    {
        Type type = typeof(T);

        T gameObjectComponent = gameObject.GetComponent<T>();
        if (gameObjectComponent == null)
        {
            gameObject.AddComponent(type);
            gameObjectComponent = gameObject.GetComponent<T>();
        }

        PropertyInfo[] propInfo = type.GetProperties();
        foreach (PropertyInfo prop in propInfo)
        {
            if (prop.CanRead && prop.CanWrite)
            {
                prop.SetValue(gameObjectComponent, prop.GetValue(component));
            }
        }

        FieldInfo[] fieldInfo = type.GetFields();
        foreach (FieldInfo field in fieldInfo)
        {
            field.SetValue(gameObjectComponent, field.GetValue(component));
        }

        return gameObject;
    }
}
