using BepInEx.Configuration;

namespace REPOLoan.Data.Models {
    public class LoanModeConfig {
        private readonly string _configCategory;

        public LoanModeConfig(ConfigFile Config, string configCategory) {
            MaxLoanAmount = Config.Bind(configCategory, nameof(MaxLoanAmount), 200_000, new ConfigDescription("The maximum amount that can be borrowed", new AcceptableValueRange<int>(1_000, 1_000_000)));
            MaxLoanTerm = Config.Bind(configCategory, nameof(MaxLoanTerm), 10, new ConfigDescription("The maximum levels to take to pay back the loan", new AcceptableValueRange<int>(1, 20)));
            LoanOffersAmount = Config.Bind(configCategory, nameof(LoanOffersAmount), 20, new ConfigDescription("The amount of loan offers to generate", new AcceptableValueRange<int>(1, 40)));
            MinInterestRate = Config.Bind(configCategory, nameof(MinInterestRate), 2.0f, new ConfigDescription("The minimum interest rate on the loan", new AcceptableValueRange<float>(0f, 30f)));
            MaxInterestRate = Config.Bind(configCategory, nameof(MaxInterestRate), 10.0f, new ConfigDescription("The maximum interest rate on the loan", new AcceptableValueRange<float>(0f, 100f)));
            ContinueDebtBetweenGames = Config.Bind(configCategory, nameof(ContinueDebtBetweenGames), false, new ConfigDescription("If the debt from a loan should carry over between games in the same lobby"));
            LoanActivationMessage = Config.Bind(configCategory, nameof(LoanActivationMessage), "I LOVE DEBT", new ConfigDescription("The message a player says when they take out a loan"));

            _configCategory = configCategory;
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
