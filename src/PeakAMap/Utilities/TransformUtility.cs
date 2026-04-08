using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PeakAMap.Utilities;

public static class TransformUtility
{
    public static Color GetColor(this Transform transform)
    {
        return transform.GetComponent<Graphic>().color;
    }

    public static Image GetImage(this Transform transform)
    {
        return transform.GetComponent<Image>();
    }

    public static TextMeshProUGUI GetTMPro(this Transform transform)
    {
        return transform.GetComponent<TextMeshProUGUI>();
    }

    public static Transform FindFromChildren(string path)
    {
        string[] queries = path.Split("/");

        Transform start = GameObject.Find(queries[0]).transform;

        Transform results = start.QueryChildren(queries[1..]);

        return results;
    }

    public static Transform QueryChildren(this Transform current, string[] queries)
    {
        if (queries.Length == 0)
        {
            return current;
        }

        foreach (Transform child in current)
        {
            if (!child.name.Equals(queries[0]))
            {
                continue;
            }

            if (queries.Length == 1)
            {
                return child;
            }

            return child.QueryChildren(queries[1..]);
        }

        if (queries.Length == 1 && String.IsNullOrEmpty(queries[0]))
        {
            return current;
        }

        Plugin.Log.LogWarning($"Cannot find {queries[0]} in {current.name}. Returned null");
        return null;
    }

    public static Transform QueryChildren(this Transform current, string queryOrPath)
    {
        string[] queries = queryOrPath.Split("/");

        Transform results = current.QueryChildren(queries);

        return results;
    }

    public static Transform SetParentAndScale(this Transform child, Transform parent, bool worldPositionStays = true)
    {
        child.SetParent(parent, worldPositionStays: worldPositionStays);
        if (child.gameObject.TryGetComponent(out RectTransform rect))
        {
            rect.localScale = new Vector3(1, 1, 1);
        }

        return child;
    }
}
