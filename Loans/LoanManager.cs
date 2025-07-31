using System;
using System.Collections.Generic;
using System.Linq;

namespace REPOLoan;

internal static class LoanManager {
    public static List<Loan> ActiveLoans;

    static LoanManager() {
        ActiveLoans = [];
    }

    public static List<Loan> ConstructLoansLoans() {
        var loanModConfig = REPOLoan.LoanModConfig;
        List<Loan> loans = [];

        var rng = new Random();
        for (var x = 0; x < loanModConfig.LoanOffersAmount.Value; x++) {
            /*
             Make random principal
             higher principal gets closer to minInterestRate, lower gets closer to maxInterestRate
             Hgher principal longer term, lower smaller term
             */

            var principal = rng.Next(1000, loanModConfig.MaxLoanAmount.Value);
            var principalMultiplier = (float)principal / loanModConfig.MaxLoanAmount.Value;

            var interestRate = (loanModConfig.MaxInterestRate.Value - loanModConfig.MinInterestRate.Value) * (1 - principalMultiplier);
            interestRate += loanModConfig.MinInterestRate.Value;

            var minTerm = 2;
            var loanTerm = (loanModConfig.MaxLoanTerm.Value - minTerm) * principalMultiplier;
            loanTerm += minTerm;

            loanTerm = loanTerm < minTerm ? minTerm : loanTerm;
            interestRate = interestRate < loanModConfig.MinInterestRate.Value ? loanModConfig.MinInterestRate.Value : interestRate;
            loans.Add(new Loan(principal, (int)loanTerm, interestRate));
        }

        loans = loans.OrderBy(i => i.Principal).ToList();
        return loans;
    }

    public static void ActivateLoan(Loan loan) {
        ActiveLoans.Add(loan);
        SaveDataManager.AddLobbyLoan(GetLobbyId(), loan);

        int currentMoney = SemiFunc.StatGetRunCurrency();
        int total = currentMoney + (loan.Principal / 1000);

        REPOLoan.Logger.LogInfo("Loan activated");
        SemiFunc.StatSetRunCurrency(total);
    }

    public static void MakeLoanPayments() {
        for (int i = ActiveLoans.Count - 1; i >= 0; i--) {
            Loan loan = ActiveLoans[i];
            int remainingBalance = loan.MakePayment();

            if (remainingBalance < 0) {
                ActiveLoans.RemoveAt(i);
                SaveDataManager.RemoveLobbyLoan(GetLobbyId(), loan.LoanID);
            }
        }
    }

    private static string GetLobbyId() {
        return StatsManager.instance.saveFileCurrent;
    }

    public static Loan[] GetNewLoans() {
        List<Loan> newLoans = new List<Loan>();
        foreach (var loan in ActiveLoans) {
            if (loan.RemainingTerm == loan.TermLength) {
                newLoans.Add(loan);
            }
        }
        return newLoans.ToArray();
    }
}
