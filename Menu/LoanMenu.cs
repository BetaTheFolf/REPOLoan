using System;
using MenuLib;
using MenuLib.MonoBehaviors;

namespace REPOLoan;

internal static class LoanMenu
{
    private static REPOPopupPage popupPage;
    private static Boolean hasInitalized = false;

    internal static void Initialize()
    {
        if (hasInitalized)
        {
            return;
        }

        REPOLoan.Logger.LogInfo("Initialize popup");
        popupPage = MenuAPI.CreateREPOPopupPage("Repo Loan", false, false, 0, null);
        popupPage.AddElement(parent =>
        {
            MenuAPI.CreateREPOButton("Fuck You", OnButtonClick, parent);
        });

        hasInitalized = true;
    }

    internal static void OnButtonClick() {
        REPOLoan.Logger.LogInfo("Fuck you");
    }

    internal static void OpenPopup()
    {
        var loans = LoanManager.GetAvailableLoans();
        foreach (var loan in loans)
        {
            REPOLoan.Logger.LogInfo(loan.ToString());
        }
        popupPage.OpenPage(true);
    }
}