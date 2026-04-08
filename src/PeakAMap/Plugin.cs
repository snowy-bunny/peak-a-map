using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace PeakAMap;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;

    internal static PluginInfo LoadedInfo { get; private set; } = null!;

    private readonly Harmony _harmony = new(Id);

    private void Awake()
    {
        Log = Logger;

        LoadedInfo = Info;

        Config.SaveOnConfigSet = true;
        Core.UserConfig.Initialize(Config);

        _harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}
