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

    View.MainLoop(accountManager);
}
catch (Exception ex)
{
    //Console.WriteLine($"An error occurred {ex}");
    Console.ForegroundColor = ConsoleColor.Red;
    Log.Error(ex, "An error ocurred");
    //TODO: Logging implementieren
}
finally
{
    Console.ForegroundColor = ConsoleColor.Red;
    Log.Information("Applications is going to be closed");
    Log.CloseAndFlush();
}








