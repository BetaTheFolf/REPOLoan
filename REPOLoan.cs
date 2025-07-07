using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace REPOLoan;

[BepInPlugin("BetaFolf.REPOLoan", "REPOLoan", "1.0")]
public class REPOLoan : BaseUnityPlugin
{
    internal static REPOLoan Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    internal Harmony? Harmony { get; set; }

    private ManualLogSource _logger => base.Logger;

    private ConfigEntry<int> maxLoanAmount;
    private ConfigEntry<int> minLoanAmount;
    private ConfigEntry<int> maxLoanTerm;
    private ConfigEntry<int> minLoanTerm;
    private ConfigEntry<float> minInterestRate;
    private ConfigEntry<float> maxInterestRate;

    private void Awake()
    {
        Instance = this;
        
        // Prevent the plugin from being deleted
        this.gameObject.transform.parent = null;
        this.gameObject.hideFlags = HideFlags.HideAndDontSave;

        Patch();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    private void SetupConfig()
    {

    }

    internal void Patch()
    {
        Harmony ??= new Harmony(Info.Metadata.GUID);
        Harmony.PatchAll();
    }

    internal void Unpatch()
    {
        Harmony?.UnpatchSelf();
    }

    private void Update()
    {
        // Code that runs every frame goes here
    }
}