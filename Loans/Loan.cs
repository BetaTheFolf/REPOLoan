using System;

namespace REPOLoan;

internal class Loan
{
    public string LoanID { get; set; }
    public int Principal { get; set; }
    public int TermLength { get; set; }
    public float InterestRate { get; set; }
    public int CurrentTerm { get; set; }
    public int RemainingBalance { get; set; }
    public int PerLevelPayment { get; set; }
    public int RemainingTerm { get; set; }

    public Loan(int principal, int term, float interestRate)
    {
        LoanID = Guid.NewGuid().ToString();
        Principal = principal;
        TermLength = term;
        RemainingTerm = term;
        InterestRate = MathF.Round(interestRate, 2);
        PerLevelPayment = calculatePerLevelPayment(principal, term, InterestRate);
    }

    public int MakePayment()
    {
        int currentMoney = SemiFunc.StatGetRunCurrency() / 1000;
        REPOLoan.Logger.LogInfo($"Making loan payment: {PerLevelPayment}");
        SemiFunc.StatSetRunCurrency(currentMoney - PerLevelPayment);

        CurrentTerm += 1;
        RemainingBalance -= PerLevelPayment;
        RemainingTerm -= 1;

        return RemainingBalance;
    }

    private int calculatePerLevelPayment(int principal, int term, float interestRate)
    {
        float totalInterest = principal * interestRate;
        float totalPayable = principal + totalInterest;

        return (int)totalPayable / term;
    }

    public override string ToString()
    {
        return $"""
            Principal: {Principal}
            Term: {TermLength}
            Interest Rate: {InterestRate}
            Payment: {PerLevelPayment}
            """;
    }
}