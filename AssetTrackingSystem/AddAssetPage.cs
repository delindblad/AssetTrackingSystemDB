using MenuKit;

namespace AssetTrackingSystem;

public class AddAssetPage : AbstractMenuPage
{
    AssetTracker _tracker;
    public AddAssetPage(string title, AbstractMenuPage? parent, AssetTracker t) : base(title, parent)
    {
        _tracker = t;
    }

    public override void Display()
    {
        Console.Clear();
        Console.WriteLine("ADD ASSET, FOLLOW THE INSTRUCTIONS TO ADD A NEW ASSET, TYPE 'Q' ON AN EMPTY LINE TO QUIT");
        Console.WriteLine();

    }

    public override int Interact()
    {
        string? assetType = "";
        string? office = "";
        string? modelName = "";
        string? brandName = "";
        string? priceString = "";
        decimal price;
        string? dateString = "";
        DateTime date;
        string? expDateString = "";
        DateTime expDate;
        string? serialNumber = "";
        
        
        DateTime purchaseDate;
        //Get location
        do
        {
            Console.WriteLine("Enter location (1 - Copenhagen, 2 - Frankfurt, 3 - London, 4 - New York, 5 - Tokyo):");
            var key = Console.ReadKey().KeyChar;

            if (key == 'q' || key == 'Q')
            {
                return -1;
            }

            if (key == '1')
            {
                office = "Copenhagen";
                break;
            }

            if (key == '2')
            {
                office = "Frankfurt";
                break;
            }
            if (key == '3')
            {
                office = "London";
                break;
            }
            if (key == '4')
            {
                office = "New York";
                break;
            }
            if (key == '5')
            {
                office = "Tokyo";
                break;
            }
        } 
        while (true);
        Console.WriteLine();
        
        //Get asset type
        do
        {
            Console.WriteLine("Enter asset type (1 - Desktop, 2 - Laptop, 3 - Mobile, 4 - Tablet):");
            var key = Console.ReadKey().KeyChar;

            if (key == 'q' || key == 'Q')
            {
                return -1;
            }

            if (key == '1')
            {
                assetType = "Desktop";
                break;
            }

            if (key == '2')
            {
                assetType = "Laptop";
                break;
            }
            if (key == '3')
            {
                assetType = "Mobile";
                break;
            }
            if (key == '4')
            {
                assetType = "Tablet";
                break;
            }
        } 
        while (true);
        Console.WriteLine();
        
        
        
        
        // Get brand name
        Console.Write("Enter brand name: ");
        brandName = Console.ReadLine()?.Trim();
        if (brandName?.ToLower() == "q")
        {
            return -1;
        }
        
         // Get product name 
        Console.Write("Enter model name: ");
        modelName = Console.ReadLine()?.Trim();
        if (modelName?.ToLower() == "q")
        {
            return -1;
        }
        
        // Get serial number
        Console.Write("Enter serial number: ");
        serialNumber = Console.ReadLine()?.Trim();
        if (serialNumber?.ToLower() == "q")
        {
            return -1;
        }
        
        
        //Get price
        do
        {
            Console.Write("Enter brand price in USD: ");
            priceString = Console.ReadLine()?.Trim();
            if (priceString?.ToLower() == "q")
            {
                return -1;
            }

            bool parsed = decimal.TryParse(priceString, out price);
            if (parsed)
            {
                break;
            }
            else
            {
                Utilities.WritelnRed("Invalid price format. Please enter a valid price in USD.");
                Console.Beep();
            }
        } while (true);
        
        // Get purchase date
        while(!DateTime.TryParse(dateString, out purchaseDate))
        {
            Console.Write("Enter purchase date (YYYY-MM-DD):");
            dateString = Console.ReadLine()?.Trim() ?? "";
            if (dateString.ToLower() == "q")
            {
                return -1;
            }
        }
        
        // Get warranty expiration date
        while(!DateTime.TryParse(expDateString, out expDate))
        {
            Console.Write("Enter warranty expiration date (YYYY-MM-DD):");
            expDateString = Console.ReadLine()?.Trim() ?? "";
            if (expDateString.ToLower() == "q")
            {
                return -1;
            }
        }

        _tracker.AddAsset(assetType, brandName ?? "", modelName ?? "", purchaseDate, price, office, serialNumber ?? "", 0,expDate);
        Utilities.WritelnGreen("ASSET ADDED!");
        Thread.Sleep(2000);
        return 0;
        
    }

    public override AbstractMenuPage Run()
    {
        while (true)
        {
            Display();
            var i = Interact();
            if (i == -1)
            {
                break;
            }
        }
        return Context;
    }
}