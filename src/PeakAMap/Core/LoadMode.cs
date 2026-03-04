using System;
using System.Collections.Generic;

namespace PeakAMap.Core;
public enum LoadMode
{
    Custom,
    Daily
}

public static class LoadModeUtil
{
    private readonly static Dictionary<string, LoadMode> s_nameToMode;

    public readonly static string[] Names;

    public readonly static Array Values;

    public readonly static int Length;

    static LoadModeUtil()
    {
        Names = Enum.GetNames(typeof(LoadMode));
        Values = Enum.GetValues(typeof(LoadMode));

        s_nameToMode = new Dictionary<string, LoadMode>();
        foreach (LoadMode loadMode in Values)
        {
            s_nameToMode.Add(GetName(loadMode), loadMode);
        }

        Length = Names.Length;
    }

    public static string GetName(this LoadMode mode)
    {
        return Names[(int)mode];
    }

    public static LoadMode GetLoadMode(string str) 
    {
        return s_nameToMode[str];
    }
}
