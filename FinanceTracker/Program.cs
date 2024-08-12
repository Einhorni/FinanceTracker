using FinanceTracker.View;
using Microsoft.Identity.Client;
using MoneyManagement;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();


try 
{
    MoneyManagementService accountManager = MoneyManagementService.Create();

    //TODO: alles asyncen
    /*await */View.MainLoop(accountManager);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Log.Error(ex, "An error ocurred");
}
finally
{
    Console.ForegroundColor = ConsoleColor.Red;
    Log.Information("Applications is going to be closed");
    Log.CloseAndFlush();
}








