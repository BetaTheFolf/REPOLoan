using REPOLib.Commands;
using REPOLib.Modules;
using BepInEx.Logging;

namespace REPOLoan;

public static class LoanCommand
{
    
    [CommandInitializer]
    public static void Initalize()
    {
    }

    [CommandExecution(
       "Loan",
       "Test loan command",
       enabledByDefault: true,
       requiresDeveloperMode: true
    )]
    [CommandAlias("loan")]
    public static void Execute(string args)
    {
        LoanMenu.OpenPopup();
    }
}