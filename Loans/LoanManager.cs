using System;
using System.Collections.Generic;

namespace REPOLoan;

internal static class LoanManager
{
    private static List<Loan> _activeLoans;
    private static int _maxLoanAmount;
    private static int _maxLoanTerm;
    private static int _loanOffersAmount;
    private static float _minInterestRate;
    private static float _maxInterestRate;

    private static Random _rand = new Random();

    // Replaces the constructor
    static LoanManager()
    {
        _maxLoanAmount = REPOLoan.maxLoanAmountConfig.Value;
        _maxLoanTerm = REPOLoan.maxLoanTermConfig.Value;
        _loanOffersAmount = REPOLoan.loanOffersConfig.Value;
        _minInterestRate = REPOLoan.minInterestRateConfig.Value;
        _maxInterestRate = REPOLoan.maxInterestRateConfig.Value;

        // TODO: Persist loans
        _activeLoans = new List<Loan>();
    }

    public static List<Loan> GetAvailableLoans()
    {
        List<Loan> loans = new List<Loan>();

        for (int i = 0; i < _loanOffersAmount; i++)
        {
            // 1. Random balance (principal)
            int principal = _rand.Next(1000, _maxLoanAmount + 1);

            // 2. Interest rate: higher principal gets closer to minInterestRate, lower gets closer to maxInterestRate
            float t = (float)(principal - 1000) / (_maxLoanAmount - 1000);
            float minRateForThisLoan = Math.Max(_minInterestRate, _maxInterestRate - (_maxInterestRate - _minInterestRate) * t);
            float maxRateForThisLoan = _maxInterestRate - (_maxInterestRate - _minInterestRate) * t * 0.5f; // prevent overlap
            float interestRate = (float)(_rand.NextDouble() * (maxRateForThisLoan - minRateForThisLoan) + minRateForThisLoan);

            REPOLoan.Logger.LogInfo(interestRate);

            // 3. Max term: longer for higher balances
            int minTerm = 2;
            int maxTermForThisLoan = (int)(minTerm + (_maxLoanTerm - minTerm) * t);
            int term = (int)Math.Round(minTerm + (_maxLoanTerm - minTerm) * t);
            term = Math.Clamp(term, minTerm, _maxLoanTerm);

            // 4. Construct loan (interest rate is in percent, as expected)
            Loan loan = new Loan(principal, term, interestRate);
            loans.Add(loan);
        }

        loans.Sort((a, b) => a.Principal.CompareTo(b.Principal));

        return loans;
    }

    public static void ActivateLoan(Loan loan)
    {
        _activeLoans.Add(loan);
        // TODO: Give users money
    }

    public static void MakeLoanPayments()
    {
        for (int i = _activeLoans.Count - 1; i >= 0; i--)
        {
            Loan loan = _activeLoans[i];
            int remainingBalance = loan.MakePayment();

            if (remainingBalance < 0)
            {
                _activeLoans.RemoveAt(i);
            }
        }
    }
}
