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
    public int RemainingTerm;

    public Loan(int principal, int term, float interestRate)
    {
        Principal = principal;
        TermLength = term;
        RemainingTerm = term;
        InterestRate = MathF.Round(interestRate, 2);
        PerLevelPayment = calculatePerLevelPayment(principal, term, InterestRate);
    }

    public int MakePayment()
    {
        int currentMoney = SemiFunc.StatGetRunCurrency() / 1000;
        SemiFunc.StatSetRunCurrency(currentMoney - PerLevelPayment);

        CurrentTerm += 1;
        RemainingBalance -= PerLevelPayment;
        RemainingTerm -= 1;

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
        return "Principal: " + Principal + "\nTerm: " + TermLength + "\nInterest Rate: " + InterestRate + "%" + "\nPayment: " + PerLevelPayment;
    }
}