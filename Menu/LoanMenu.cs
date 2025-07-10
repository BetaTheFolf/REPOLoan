using System.Collections.Generic;
using MenuLib;
using MenuLib.MonoBehaviors;
using UnityEngine;

namespace REPOLoan;

internal static class LoanMenu

{
    private static List<Loan> _loans;
    private static string _loanMessage;

    static LoanMenu()
    {
        _loans = LoanManager.GetAvailableLoans();
        _loanMessage = REPOLoan.loanActivationMessage.Value;
    }

    internal static void OnLoanButtonClick(REPOPopupPage popupPage, Loan loan)
    {
        popupPage.ClosePage(true);
        LoanManager.ActivateLoan(loan);

        ChatManager instance = ChatManager.instance;

        if (instance != null)
        {
            instance.PossessChatScheduleStart(-1);
            instance.PossessChat(ChatManager.PossessChatID.Betrayal, _loanMessage, 1, Color.red, 0f, true);
            // TODO: Make the loan details show up on the taxman screen without saying it
            // instance.PossessChat(ChatManager.PossessChatID.None, loan.ToString(), 1, Color.black, -1f, true);
            instance.PossessChatScheduleEnd();
        }
    }

    internal static void OpenPopup()
    {
        REPOPopupPage popupPage = MenuAPI.CreateREPOPopupPage("Repo Loan", false, false, 0, null);
        popupPage.scrollView.scrollSpeed = 3f;

        if (LoanManager.ActiveLoans.Count > 0)
        {
            popupPage.AddElementToScrollView(parent =>
            {
                REPOLabel label = MenuAPI.CreateREPOLabel("Active Loans", parent);
                return label.rectTransform;
            });

            popupPage.AddElementToScrollView(parent =>
            {
                REPOSpacer spacer = MenuAPI.CreateREPOSpacer(parent, size: new Vector2(0, 20));
                return spacer.rectTransform;
            });

            foreach (Loan loan in LoanManager.ActiveLoans)
            {
                CreateModItem(popupPage, loan, false, true);
            }

            popupPage.AddElementToScrollView(parent =>
            {
                REPOSpacer spacer = MenuAPI.CreateREPOSpacer(parent, size: new Vector2(0, 20));
                return spacer.rectTransform;
            });
        }

        popupPage.AddElementToScrollView(parent =>
        {
            REPOLabel label = MenuAPI.CreateREPOLabel("Loan Offers", parent);
            return label.rectTransform;
        });

        popupPage.AddElementToScrollView(parent =>
        {
            REPOSpacer spacer = MenuAPI.CreateREPOSpacer(parent, size: new Vector2(0, 20));
            return spacer.rectTransform;
        });

        foreach (Loan loan in _loans)
        {
            CreateModItem(popupPage, loan, true, false);
        }

        popupPage.AddElement(parent =>
        {
            MenuAPI.CreateREPOButton("Back", () => popupPage.ClosePage(true), parent, new Vector2(60f, 25f));
        });

        popupPage.onEscapePressed += () =>
        {
            popupPage.ClosePage(true);
            return false;
        };

        popupPage.OpenPage(true);
    }

    internal static void CreateModItem(REPOPopupPage popupPage, Loan loan, bool addButton, bool showRemainingTerm)
    {
        popupPage.AddElementToScrollView(parent =>
            {
                REPOLabel label = MenuAPI.CreateREPOLabel("Principal: $" + loan.Principal, parent);
                return label.rectTransform;
            });

        popupPage.AddElementToScrollView(parent =>
        {
            REPOLabel label = MenuAPI.CreateREPOLabel("Interest Rate: " + loan.InterestRate + "%", parent);
            return label.rectTransform;
        });

        popupPage.AddElementToScrollView(parent =>
        {
            REPOLabel label = MenuAPI.CreateREPOLabel("Term Length: " + loan.TermLength, parent);
            return label.rectTransform;
        });

        popupPage.AddElementToScrollView(parent =>
        {
            REPOLabel label = MenuAPI.CreateREPOLabel("Payment: $" + loan.PerLevelPayment, parent);
            return label.rectTransform;
        });

        if (showRemainingTerm)
        {
            popupPage.AddElementToScrollView(parent =>
            {
                REPOLabel label = MenuAPI.CreateREPOLabel("Remaining Term: " + loan.RemainingTerm, parent);
                return label.rectTransform;
            });
        }

        if (addButton)
        {
            popupPage.AddElementToScrollView(parent =>
                {
                    REPOButton button = MenuAPI.CreateREPOButton("Take Loan", () => OnLoanButtonClick(popupPage, loan), parent);
                    return button.rectTransform;
                });
        }
        
        popupPage.AddElementToScrollView(parent =>
        {
            REPOSpacer spacer = MenuAPI.CreateREPOSpacer(parent, size: new Vector2(0, 20));
            return spacer.rectTransform;
        });
    }
}