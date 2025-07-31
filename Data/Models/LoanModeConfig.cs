using BepInEx.Configuration;

namespace REPOLoan.Data.Models {
    public class LoanModeConfig {
        private readonly string _configCategory;

        public LoanModeConfig(ConfigFile Config, string configCategory) {
            maxLoanAmount = Config.Bind(configCategory, "MaxLoanAmount", 200_000, new ConfigDescription("The maximum amount that can be borrowed", new AcceptableValueRange<int>(1_000, 1_000_000)));
            maxLoanTerm = Config.Bind(configCategory, "MaxLoanTerm", 10, new ConfigDescription("The maximum levels to take to pay back the loan", new AcceptableValueRange<int>(1, 20)));
            loanOffers = Config.Bind(configCategory, "LoanOffersAmount", 20, new ConfigDescription("The amount of loan offers to generate", new AcceptableValueRange<int>(1, 40)));
            minInterestRate = Config.Bind(configCategory, "MinInterestRate", 2.0f, new ConfigDescription("The minimum interest rate on the loan", new AcceptableValueRange<float>(0f, 30f)));
            maxInterestRate = Config.Bind(configCategory, "MaxInterestRate", 10.0f, new ConfigDescription("The maximum interest rate on the loan", new AcceptableValueRange<float>(0f, 100f)));
            continueDebtBetweenGames = Config.Bind(configCategory, "ContinueDebtBetweenGames", false, new ConfigDescription("If the debt from a loan should carry over between games in the same lobby"));
            loanActivationMessage = Config.Bind(configCategory, "LoanActivationMessage", "I LOVE DEBT", new ConfigDescription("The message a player says when they take out a loan"));
            _configCategory = configCategory;
        }

        public ConfigEntry<int> maxLoanAmount { get; set; }
        public ConfigEntry<int> maxLoanTerm { get; set; }
        public ConfigEntry<int> loanOffers { get; set; }
        public ConfigEntry<float> minInterestRate { get; set; }
        public ConfigEntry<float> maxInterestRate { get; set; }
        public ConfigEntry<bool> continueDebtBetweenGames { get; set; }
        public ConfigEntry<string> loanActivationMessage { get; set; }
    }
}
