using System;
using System.Linq;
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

    public static GameObject QueryChildren(this GameObject gameObject, string[] queries)
    {
        return gameObject.transform.QueryChildren(queries).gameObject;
    }

    public static GameObject QueryChildren(this GameObject gameObject, string queryOrPath)
    {
        return gameObject.transform.QueryChildren(queryOrPath).gameObject;
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent(out T retrieved))
        {
            return retrieved;
        }
        return gameObject.AddComponent<T>();
    }
    
    public static Component GetOrAddComponent(this GameObject gameObject, Type type)
    {
        if (gameObject.TryGetComponent(type, out Component retrieved))
        {
            return retrieved;
        }
        return gameObject.AddComponent(type);
    }

    public static GameObject PasteComponent<T>(this GameObject gameObject, T component, params string[] exclude) where T : Component
    {
        Type type = component.GetType();
        Component gameObjectComponent = gameObject.GetOrAddComponent(type);

        PropertyInfo[] propInfo = type.GetProperties();
        foreach (PropertyInfo prop in propInfo)
        {
            if (prop.CanRead && prop.CanWrite && !exclude.Contains(prop.Name))
            {
                try
                {
                    prop.SetValue(gameObjectComponent, prop.GetValue(component));
                }
                catch { }
            }
        }

        return gameObject;
    }

    public static string StripCloneInName(this GameObject gameObject)
    {
        string cloneConcat = "(Clone)";
        if (gameObject.name.EndsWith(cloneConcat))
        {
            int startIndex = gameObject.name.Length - cloneConcat.Length;
            gameObject.name = gameObject.name.Remove(startIndex);
        }
        return gameObject.name;
    }
}
