using System.Text.Json;
using freecurrencyapi;
using MenuKit;
using Spectre.Console;

namespace AssetTrackingSystem;

class Program
{
    static void Main(string[] args)
    {   
        
        AnsiConsole.MarkupLine("[yellow]Welcome to the Asset Tracker![/]");
        if (args.Length > 1 && args[1] == "init")
        {
            AnsiConsole.WriteLine("Adding sample data...");
            AddStuff();
            Environment.Exit(0);
        }
        string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        if (connectionString == null)
        {
            AnsiConsole.WriteLine("Connection string is null, please check your environment variables.");
            Environment.Exit(-1);
        }

        
        Run();

    }

    static void AddStuff()
    {
        AssetTracker assetTracker = new();
        assetTracker.UpdateCurrecyData();
        
        
        assetTracker.AddOffice("New York", "$");
        assetTracker.AddOffice("London", "£");
        assetTracker.AddOffice("Frankfurt", "€");
        assetTracker.AddOffice("Tokyo", "¥");
        assetTracker.AddOffice("Copenhagen", "DKK ");
        assetTracker.AddAsset("Mobile", "Apple", "iPhone 14", DateTime.Parse("2025-02-06"),600,
            "London", "4354364366656", 0, DateTime.Parse("2026-03-06"));
        assetTracker.AddAsset("Laptop", "Lenovo", "Think Pad X1", DateTime.Parse("2024-06-06"),400,
            "Frankfurt", "435-4657-666-56", 0, DateTime.Parse("2027-06-06"));
        assetTracker.AddAsset("Tablet", "Apple", "iPad 12", DateTime.Parse("2025-08-06"),1100,
            "Tokyo", "435-4657-666-56", 0, DateTime.Parse("2030-02-06"));
        assetTracker.AddAsset("Tablet", "Apple", "iPad 14", DateTime.Parse("2025-08-06"),1200,
            "Copenhagen", "435-4657-666-56", 0, DateTime.Parse("2030-02-06"));
        
        
    }
    
    static void Run()
    {
        //Create asset tracker

        AssetTracker assetTracker = new AssetTracker();
            

        //Update currency data
        assetTracker.UpdateCurrecyData();
        //Create root page
        var root = new RootPage("ASSET TRACKER type 'Q' TO QUIT", assetTracker);
        
        root.AddChildPage(new ShowAssetsPage("SHOW ASSETS", root, assetTracker));
        root.AddChildPage(new AddAssetPage("ADD ASSET", root, assetTracker));
    
       
       AbstractMenuPage? context = root;
       while (true)
       {
           context = context.Run();
           if (context == null!)
           {
               return;
           }
       }
    }
}