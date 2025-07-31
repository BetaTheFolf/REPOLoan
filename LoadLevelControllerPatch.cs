using System;
using HarmonyLib;
using UnityEngine;

namespace REPOLoan;

[HarmonyPatch(typeof(EnvironmentDirector), "Setup")]
public class LoadLevelControllerPatch
{
    private static string lastLevelName = "";
    private static int originalMoney = 0;
    private readonly static string storeLevelName = "Service Station";

    [HarmonyPrefix, HarmonyPatch(nameof(EnvironmentDirector.Setup))]
    private static void Start_Prefix(EnvironmentDirector __instance)
    {
        REPOLoan.Logger.LogDebug($"{__instance} Start Prefix");
        // https://thunderstore.io/c/repo/p/DirtyGames/REPOShopLib/
    }

    [HarmonyPostfix]
    private static void ManageLoans()
    {
        if ((LevelGenerator.Instance.Level is not null) && LevelGenerator.Instance.Level.NarrativeName.Equals(storeLevelName, StringComparison.OrdinalIgnoreCase))
        {
            lastLevelName = storeLevelName;
            LoanManager.MakeLoanPayments();
            REPOLoan.Logger.LogInfo("Making loan payments");
            originalMoney = SemiFunc.StatGetRunCurrency();
        }
        // When leaving the shop then reset balance
        else if (lastLevelName.Equals(storeLevelName, StringComparison.OrdinalIgnoreCase))
        {
            // Check if users spent more than loan amount
            var currentMoney = SemiFunc.StatGetRunCurrency();
            var totalNewLoans = LoanManager.GetNewLoans();

            // Total amount for new loans
            var totalNewLoanAmount = 0;
            foreach (var loan in totalNewLoans)
            {
                totalNewLoanAmount += loan.Principal;
            }

            // If the user spent more than the loan amount then reset balance to original minus the extra spent
            if (currentMoney < originalMoney - totalNewLoanAmount)
            {
                REPOLoan.Logger.LogInfo("Resetting balance to original minus extra spent");
                SemiFunc.StatSetRunCurrency(originalMoney - totalNewLoanAmount);
            }
            else
            {
                REPOLoan.Logger.LogInfo("Resetting balance to original");
                SemiFunc.StatSetRunCurrency(originalMoney);
            }
       
            StatsManager.instance.SaveFileSave();
        }
        
        if (LevelGenerator.Instance.Level is not null)
        {
            lastLevelName = LevelGenerator.Instance.Level.NarrativeName;
        }
    }

    [HarmonyPrefix, HarmonyPatch(nameof(EnvironmentDirector.Setup))]
    private static void Start_Postfix(EnvironmentDirector __instance)
    {
        REPOLoan.Logger.LogDebug($"{__instance} Start Postfix");
    }
}