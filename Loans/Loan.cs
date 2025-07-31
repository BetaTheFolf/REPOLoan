using System;

namespace REPOLoan;

internal class Loan {
    public string LoanID { get; set; }
    public int Principal { get; set; }
    public int TermLength { get; set; }
    public float InterestRate { get; set; }
    public int CurrentTerm { get; set; }
    public int RemainingBalance { get; set; }
    public int PerLevelPayment { get; set; }
    public int RemainingTerm { get; set; }

    public Loan(int principal, int term, float interestRate) {
        LoanID = Guid.NewGuid().ToString();
        Principal = principal;
        TermLength = term;
        RemainingTerm = term;
        InterestRate = MathF.Round(interestRate, 2);
        PerLevelPayment = CalculatePerLevelPayment(principal, term, InterestRate);
    }

    public int MakePayment() {
        int currentMoney = SemiFunc.StatGetRunCurrency() / 1000;
        REPOLoan.Logger.LogInfo($"Making loan payment: {PerLevelPayment}");
        SemiFunc.StatSetRunCurrency(currentMoney - PerLevelPayment);

        CurrentTerm += 1;
        RemainingBalance -= PerLevelPayment;
        RemainingTerm -= 1;

        return RemainingBalance;
    }

    private int CalculatePerLevelPayment(int principal, int term, float interestRate) {
        float totalInterest = principal * (interestRate / 100f);
        float totalPayable = principal + totalInterest;

        REPOLoan.Logger.LogInfo($"totalPayable: {totalPayable} / term: {term}");

        var payment = (int)totalPayable / term;
        REPOLoan.Logger.LogInfo($"calculatePerLevelPayment: {payment}");

        return payment;
    }

    public override string ToString() {
        return $"""
            Principal: {Principal}
            Term: {TermLength}
            Interest Rate: {InterestRate}
            Payment: {PerLevelPayment}
            """;
    }
}