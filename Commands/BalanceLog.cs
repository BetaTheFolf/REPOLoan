using REPOLib.Commands;

namespace REPOLoan;

public static class BalanceLogCommand
{
    
    [CommandInitializer]
    public static void Initalize()
    {
    }

    [CommandExecution(
       "Balance log",
       "Balance log",
       enabledByDefault: true,
       requiresDeveloperMode: true
    )]
    [CommandAlias("balance")]
    public static void Execute(string args)
    {
        int currentMoney = SemiFunc.StatGetRunCurrency();
        REPOLoan.Logger.LogInfo(currentMoney);
    }
}