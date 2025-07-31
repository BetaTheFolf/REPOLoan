using BepInEx.Configuration;

namespace REPOLoan.Data.Models {
    public class LoanModConfig {
        private const string CONFIG_CATEGORY = "Debt Slave Options";

        public LoanModConfig(ConfigFile Config) {
            MaxLoanAmount = Config.Bind(CONFIG_CATEGORY, nameof(MaxLoanAmount), 200_000, new ConfigDescription("The maximum amount that can be borrowed", new AcceptableValueRange<int>(1_000, 1_000_000)));
            MaxLoanTerm = Config.Bind(CONFIG_CATEGORY, nameof(MaxLoanTerm), 10, new ConfigDescription("The maximum levels to take to pay back the loan", new AcceptableValueRange<int>(2, 20)));
            LoanOffersAmount = Config.Bind(CONFIG_CATEGORY, nameof(LoanOffersAmount), 20, new ConfigDescription("The amount of loan offers to generate", new AcceptableValueRange<int>(1, 40)));
            MinInterestRate = Config.Bind(CONFIG_CATEGORY, nameof(MinInterestRate), 2.0f, new ConfigDescription("The minimum interest rate on the loan", new AcceptableValueRange<float>(0f, 30f)));
            MaxInterestRate = Config.Bind(CONFIG_CATEGORY, nameof(MaxInterestRate), 10.0f, new ConfigDescription("The maximum interest rate on the loan", new AcceptableValueRange<float>(0f, 100f)));
            ContinueDebtBetweenGames = Config.Bind(CONFIG_CATEGORY, nameof(ContinueDebtBetweenGames), false, new ConfigDescription("If the debt from a loan should carry over between games in the same lobby"));
            LoanActivationMessage = Config.Bind(CONFIG_CATEGORY, nameof(LoanActivationMessage), "I LOVE DEBT", new ConfigDescription("The message a player says when they take out a loan"));
        }

        public ConfigEntry<int> MaxLoanAmount { get; set; }
        public ConfigEntry<int> MaxLoanTerm { get; set; }
        public ConfigEntry<int> LoanOffersAmount { get; set; }
        public ConfigEntry<float> MinInterestRate { get; set; }
        public ConfigEntry<float> MaxInterestRate { get; set; }
        public ConfigEntry<bool> ContinueDebtBetweenGames { get; set; }
        public ConfigEntry<string> LoanActivationMessage { get; set; }
    }
}
