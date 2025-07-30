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

    internal static ConfigEntry<int> maxLoanAmountConfig;
    internal static ConfigEntry<int> maxLoanTermConfig;
    internal static ConfigEntry<int> loanOffersConfig;
    internal static ConfigEntry<float> minInterestRateConfig;
    internal static ConfigEntry<float> maxInterestRateConfig;
    internal static ConfigEntry<bool> continueDebtBetweenGamesConfig;
    internal static ConfigEntry<string> loanActivationMessage;
    private readonly string _configSection = "Debt Slave Options";

    private void Awake()
    {
        Instance = this;

        SetupConfig();
        
        // Prevent the plugin from being deleted
        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;

        Patch();

        SaveDataManager.Initialize();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
    }

    private void SetupConfig()
    {
        maxLoanAmountConfig = Config.Bind<int>(_configSection, "MaxLoanAmount", 200_000, new ConfigDescription("The maximum amount that can be borrowed", new AcceptableValueRange<int>(1_000, 1_000_000)));
        maxLoanTermConfig = Config.Bind<int>(_configSection, "MaxLoanTerm", 10, new ConfigDescription("The maximum levels to take to pay back the loan", new AcceptableValueRange<int>(1, 20)));
        loanOffersConfig = Config.Bind<int>(_configSection, "LoanOffersAmount", 20, new ConfigDescription("The amount of loan offers to generate", new AcceptableValueRange<int>(1, 40)));
        minInterestRateConfig = Config.Bind<float>(_configSection, "MinInterestRate", 2.0f, new ConfigDescription("The minimum interest rate on the loan", new AcceptableValueRange<float>(0f, 30f)));
        maxInterestRateConfig = Config.Bind<float>(_configSection, "MaxInterestRate", 10.0f, new ConfigDescription("The maximum interest rate on the loan", new AcceptableValueRange<float>(0f, 100f)));
        loanActivationMessage = Config.Bind<string>(_configSection, "LoanActivationMessage", "I LOVE DEBT", new ConfigDescription("The message a player says when they take out a loan"));
        continueDebtBetweenGamesConfig = Config.Bind<bool>(_configSection, "ContinueDebtBetweenGames", false, new ConfigDescription("If the debt from a loan should carry over between games in the same lobby"));
    }

    internal void Patch()
    {
        if (Harmony == null)
        {
            Harmony = new Harmony(Info.Metadata.GUID);
        }
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