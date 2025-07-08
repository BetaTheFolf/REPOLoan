using System;

namespace REPOLoan;

internal class Loan
{
    public int Principal;
    public int TermLength;
    public float InterestRate;
    public int CurrentTerm;
    public int RemainingBalance;
    public int PerLevelPayment;

    public Loan(int principal, int term, float interestRate)
    {
        Principal = principal;
        TermLength = term;
        InterestRate = MathF.Round(interestRate, 2);
        PerLevelPayment = calculatePerLevelPayment(principal, term, InterestRate);
    }

    public int MakePayment()
    {
        int currentMoney = SemiFunc.StatGetRunCurrency();
        SemiFunc.StatSetRunCurrency(currentMoney - PerLevelPayment);

        CurrentTerm += 1;
        RemainingBalance -= PerLevelPayment;

        return RemainingBalance;
    }

    private int calculatePerLevelPayment(int principal, int term, float interestRate)
    {
        float totalInterest = principal * (interestRate / 100f);
        float totalPayable = principal + totalInterest;

        return (int)Math.Ceiling(totalPayable / term);
    }

    public override string ToString()
    {
        return $"Loan: Principal=${Principal}, Term={TermLength} periods, InterestRate={InterestRate:F2}%, PerLevelPayment=${PerLevelPayment}, RemainingBalance=${RemainingBalance}, CurrentTerm={CurrentTerm}";
    }
}