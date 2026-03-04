using System.Collections.Generic;
using UnityEngine;


namespace PeakAMap.Utilities;
public class JsonExtended
{
    private static HashSet<char> s_startDelimiters = new HashSet<char>() { '"', '{', '(', '[' };
    private static HashSet<char> s_endDelimiters = new HashSet<char>() { '"', '}', ')', ']' };
    private static string s_tab = "    ";

    public static string Tab
    {
        get { return s_tab; }
        set { s_tab = value; }
    }

    public static string ListToJson<T>(List<T> items, bool prettyPrint = false)
    {
        return ArrayToJson<T>(items.ToArray(), prettyPrint);
    }

    public static List<T> ListFromJson<T>(string json)
    {
        return [.. ArrayFromJson<T>(json)];
    }

    public static string ArrayToJson<T>(T[] items, bool prettyPrint = false)
    {
        string[] jsonList = new string[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            jsonList[i] = JsonUtility.ToJson(items[i], prettyPrint);
        }

        return EncaseCombine(jsonList, '[', ']', prettyPrint);
    }

    public static T[] ArrayFromJson<T>(string json)
    {
        string[] parse = SplitSections(json, '[', ']');

        T[] objectList = new T[parse.Length];

        for (int i = 0; i < objectList.Length; i++)
        {
            objectList[i] = JsonUtility.FromJson<T>(parse[i]);
        }

        return objectList;
    }

    public static string CombineJson(bool prettyPrint, params string[] objectJsons)
    {
        char start = objectJsons[0][0];
        char end = objectJsons[0][^1];
        string[] combined = new string[objectJsons.Length];
        for (int i = 0; i < objectJsons.Length; i++)
        {
            combined[i] = objectJsons[i][2..^2];
        }

        return start + "\n" + string.Join(",\n", combined) + "\n" + end;
    }

    public static string EncaseCombine(string[] objectJsons, char start, char end, bool prettyPrint = false)
    {
        if (!prettyPrint)
        {
            return start + string.Join(",", objectJsons) + end;
        }

        string pretty = start + "\n";
        string[] lines;
        for (int i = 0; i < objectJsons.Length; i++)
        {
            lines = objectJsons[i].Split("\n");
            for (int j = 0; j < lines.Length; j++)
            {
                if (j < lines.Length - 1)
                {
                    pretty += Tab + lines[j] + "\n";
                }
                else
                {
                    pretty += Tab + lines[j];
                }

            }
            if (i < objectJsons.Length - 1)
            {
                pretty += ",\n";
            }
        }
        pretty += "\n" + end;
        return pretty;
    }

    // Values that are originally string types should have escaped double quotes included in argument.
    // ex)  CreateObject("key", "\"some value\"");
    //      returns """{"key":"some value"}"""
    public static string CreateJsonObject(string key, string value, bool prettyPrint = false)
    {
        if (!prettyPrint)
        {
            return "{\"" + key + "\":" + value + "}";
        }

        string mapping = "\"" + key + "\": " + value;
        return EncaseCombine([mapping], '{', '}', prettyPrint);
    }

    public static string[] SplitSections(string json, char start, char end, bool prettyPrint = false)
    {
        List<string> parse = new();
        string substring = "";

        int startIndex = _startSectionIndex(json, start);
        int endIndex = _endSectionIndex(json, end);

        int stack = 0;
        bool inString = false;
        for (int i = startIndex; i <= endIndex; i++)
        {
            char prev = json[i - 1];
            char curr = json[i];

            // Handle double quote
            if (curr == '"' && prev != '\\')
            {
                if (inString)
                {
                    stack--;
                }
                else
                {
                    stack++;
                }
                inString = !inString;
                substring += curr;
                continue;
            }

            if (inString)
            {
                substring += curr;
                continue;
            }

            if (char.IsWhiteSpace(curr))
            {
                if (prettyPrint)
                {
                    substring += curr;
                }
                continue;
            }

            // Found item so reset substring.
            if (curr == ',' && stack == 0)
            {
                parse.Add(substring);
                substring = "";
                continue;
            }

            if (s_startDelimiters.Contains(curr))
            {
                stack++;
                substring += curr;
                continue;
            }

            if (s_endDelimiters.Contains(curr))
            {
                stack--;
                substring += curr;
                continue;
            }

            substring += curr;
        }
        parse.Add(substring);

        return parse.ToArray();
    }

    public static string GetValueFromJson(string json, string key, bool prettyPrint = false)
    {
        string[] objects = SplitSections(json, '{', '}', prettyPrint);
        for (int i = 0; i < objects.Length; i++)
        {
            if (!objects[i].Contains(key))
            {
                continue;
            }

            int start = _startSectionIndex(objects[i], ':');
            return objects[i][start..];
        }
        return "";
    }

    // Returns index after first target character
    private static int _startSectionIndex(string text, char target)
    {
        bool startParse = false;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (_foundChar(ref startParse, c, target))
            {
                return i + 1;
            }
        }
        return -1;
    }

    // Returns index before last target character
    private static int _endSectionIndex(string text, char target)
    {
        bool startParse = false;

        for (int i = text.Length-1; i > -1; i--)
        {
            char c = text[i];
            if (_foundChar(ref startParse, c, target))
            {
                return i - 1;
            }
        }
        return -1;
    }

    private static bool _foundChar(ref bool started, char currChar, char target)
    {
        if (!started)
        {
            started = (currChar == target);
        }
        return started;
    }
}
