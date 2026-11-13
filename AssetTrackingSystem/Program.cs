using System.Text.Json;
using freecurrencyapi;
using MenuKit;
using Spectre.Console;

namespace AssetTrackingSystem;

class Program
{
    static void Main(string[] args)
    {   
        //Run();
        AnsiConsole.MarkupLine("[yellow]Welcome to the Asset Tracker![/]");
        
        AddStuff();

    }

    static void AddStuff()
    {
        AssetTracker assetTracker = new();
        assetTracker.UpdateCurrecyData();
        
        
        // assetTracker.AddOffice("New York", "$");
        // assetTracker.AddOffice("London", "£");
        // assetTracker.AddOffice("Frankfurt", "€");
        // assetTracker.AddOffice("Tokyo", "¥");
        // assetTracker.AddOffice("Copenhagen", "DKK");
        // assetTracker.AddAsset("Mobile", "Apple", "iPhone 14", DateTime.Parse("2025-02-06"),600,
        //     "London", "4354364366656", 0, DateTime.Parse("2028-03-06"));
        // assetTracker.AddAsset("Laptop", "Lenovo", "Think Pad X1", DateTime.Parse("2024-06-06"),600,
        //     "Frankfurt", "435-4657-666-56", 0, DateTime.Parse("2027-06-06"));
        // assetTracker.AddAsset("Tablet", "Apple", "iPad 12", DateTime.Parse("2025-08-06"),600,
        //     "Tokyo", "435-4657-666-56", 0, DateTime.Parse("2030-02-06"));
        // assetTracker.AddAsset("Tablet", "Apple", "iPad 14", DateTime.Parse("2025-08-06"),600,
        //     "Copenhagen", "435-4657-666-56", 0, DateTime.Parse("2030-02-06"));
        // assetTracker.PrintAssets(OrederBy.Id);
        
        assetTracker.ListOffices();
        
    }
    
    // static void Run()
    // {
    //     //Create asset tracker
    //     AssetTracker assetTracker;
    //     //If there's an assets.json file, load it
    //     if (File.Exists("assets.json"))
    //     {
    //         try
    //         {
    //             Utilities.WritelnGreen($"Loading from assets.json");
    //             string jsonString = File.ReadAllText("assets.json");
    //             assetTracker = JsonSerializer.Deserialize<AssetTracker>(jsonString)!;
    //         }
    //         catch (Exception e)
    //         {
    //
    //             Console.WriteLine(e);
    //             throw;
    //         }
    //
    //     }
    //     else
    //     //Otherwise, create a new one
    //     {
    //         assetTracker = new AssetTracker();
    //         
    //     }
    //     //Update currency data
    //     assetTracker.UpdateCurrecyData();
    //     //Create root page
    //     var root = new RootPage("ASSET TRACKER type 'Q' TO QUIT", assetTracker);
    //     
    //     root.AddChildPage(new ShowAssetsPage("SHOW ASSETS", root, assetTracker));
    //     root.AddChildPage(new AddAssetPage("ADD ASSET", root, assetTracker));
    //
    //    
    //    AbstractMenuPage? context = root;
    //    while (true)
    //    {
    //        context = context.Run();
    //        if (context == null!)
    //        {
    //            return;
    //        }
    //    }
    // }
}