using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

using HarmonyLib;

using REPOLoan.Data.Models;

using UnityEngine;

namespace REPOLoan;

[BepInPlugin("BetaFolf.REPOLoan", "REPOLoan", "1.0")]
[BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
public class REPOLoan : BaseUnityPlugin {
    internal static REPOLoan Singleton { get; private set; } = null!;

    public LoanModeConfig LoanModeConfig { get; set; }

    internal new static ManualLogSource Logger => Singleton._logger;
    private ManualLogSource _logger => base.Logger;

    internal Harmony? Harmony { get; set; }

    private void Awake() {
        Singleton = this;
        LoanModeConfig = new LoanModeConfig(Config, "Debt Slave Options");

        // Prevent the plugin from being deleted
        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        Patch();

        SaveDataManager.Initialize();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    internal void Patch() {
        if (Harmony == null) {
            Harmony = new Harmony(Info.Metadata.GUID);
        }
        Harmony.PatchAll();
    }

    internal void Unpatch() {
        Harmony?.UnpatchSelf();
    }

    private void Update() {
        // Code that runs every frame goes here
    }
}