using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace REPOLoan;

[BepInPlugin("BetaFolf.REPOLoan", "REPOLoan", "1.0")]
[BepInDependency(REPOLib.MyPluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
public class REPOLoan : BaseUnityPlugin
{
    internal static REPOLoan Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    internal Harmony? Harmony { get; set; }

    private ManualLogSource _logger => base.Logger;

    private ConfigEntry<int> _maxLoanAmount;
    private ConfigEntry<int> _maxLoanTerm;
    private ConfigEntry<float> _minInterestRate;
    private ConfigEntry<float> _maxInterestRate;
    private ConfigEntry<bool> _continueDebtBetweenGames;
    private readonly string _configSection = "Debt Slave Options";

    private void Awake()
    {
        Instance = this;

        SetupConfig();
        
        // Prevent the plugin from being deleted
        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        Patch();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    private void SetupConfig()
    {
        _maxLoanAmount = Config.Bind<int>(_configSection, "MaxLoanAmount", 500_000, new ConfigDescription("The maximum amount that can be borrowed", new AcceptableValueRange<int>(5_000, 1_000_000)));
        _maxLoanTerm = Config.Bind<int>(_configSection, "MaxLoanTerm", 10, new ConfigDescription("The maximum levels to take to pay back the loan", new AcceptableValueRange<int>(1, 20)));
        _minInterestRate = Config.Bind<float>(_configSection, "MinInterestRate", 10.0f, new ConfigDescription("The minimum interest rate on the loan", new AcceptableValueRange<float>(0f, 30f)));
        _maxInterestRate = Config.Bind<float>(_configSection, "MaxInterestRate", 50.0f, new ConfigDescription("The maximum interest rate on the loan", new AcceptableValueRange<float>(0f, 100f)));
        _continueDebtBetweenGames = Config.Bind<bool>(_configSection, "ContinueDebtBetweenGames", false, new ConfigDescription("If the debt from a loan should carry over between games in the same lobby"));
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