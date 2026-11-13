using Microsoft.EntityFrameworkCore; // To use ExecuteUpdate, ExecuteDelete.
using Microsoft.EntityFrameworkCore.ChangeTracking; // To use EntityEntry<T>.

using System.Collections;
using System.Text.Json;
using freecurrencyapi;
using Spectre.Console;

namespace AssetTrackingSystem;

//Class provides an interface for tracking assets
public class AssetTracker
{
    public AssetTracker()
    {
        
    }

    public Hashtable CurrencyData { get; set; } = new();
    public AssetDB Db {get; set;}

    //Used for unique ID, gets incremented when used
    public long IdStore
    {
        get => field++;
        set => field = value;
    } = 0;


    //Used to check asset age
    readonly TimeSpan _yellowSpan = new TimeSpan(913, 0, 0, 0, 0); //2.5 years
    readonly TimeSpan _redSpan = new TimeSpan(1004, 0, 0, 0, 0); //2.75 years

    
    //Adds a new Office
    public void AddOffice(string city, string currency)
    {
        using (AssetDB db = new())
        {
            Office o = new()
            {
                City = city,
                Currency = currency
            };
            db.Offices.Add(o);
            EntityEntry<Office> entity = db.Offices?.Add(o) ?? throw new ArgumentNullException("db.Offices?.Add(o)");
            AnsiConsole.MarkupLineInterpolated($"[yellow]State: {entity.State}, Office ID: {o.Id}[/]");
            int affected = db.SaveChanges();
            AnsiConsole.MarkupLineInterpolated($"[yellow]State: {entity.State}, Office ID: {o.Id}[/]");
            AnsiConsole.MarkupLineInterpolated($"[yellow]Affected rows: {affected} , Office ID: {o.Id}[/]");
        }
    }
    
    //Adds a new asset
    public void AddAsset(string type, string brand, string model, DateTime purchaseDate, decimal priceUSD,
         string office, string serialNumber, int employeeId, DateTime expDate)
    {
        decimal priceLocal;
        string prefix = "$";
        switch (office)
        {
            case "Copenhagen":
                priceLocal = ConvertCurrency("DKK", priceUSD);
                break;
            case "New York":
                priceLocal = priceUSD;
                break;
            case "Frankfurt":
                priceLocal = ConvertCurrency("EUR", priceUSD);
                break;
            case "Tokyo":
                priceLocal = ConvertCurrency("JPY", priceUSD);
                break;
            case "London":
                priceLocal = ConvertCurrency("GBP", priceUSD);
                break;
            default:
                priceLocal = priceUSD;
                break;
        }
        using (AssetDB db = new())
        {
            
            // OfficeLocation o = new()
            // {
            //     City = office,
            //     Currency = GetCurrencyPrefix(office)
            //
            // };
            IQueryable<Office>? offices = db.Offices;
            if (offices is null || !offices.Any())
            {
                AnsiConsole.MarkupLineInterpolated($"[red]No offices found.[/]");
                return;
            }

            /*foreach (var of  in offices)
            {
                AnsiConsole.WriteLine($"Office: {of.City} {of.Id} {of.Currency}");
            }*/
            
            Office o = offices.FirstOrDefault(o => o.City == office) ?? throw new ArgumentException($"Office {office} not found.");
            AnsiConsole.WriteLine($"Assigned Office: {o.City} {o.Id} {o.Currency}");
            
            Asset a = new()
            {
                AssetType = type,
                Brand = brand,
                ModelName = model,
                PurchaseDate = purchaseDate,
                PurchasePrice = + priceUSD,
                PriceLocal = priceLocal,
                Office = o,
                SerialNumber = serialNumber,
                //Employee = 
                WarrantyExpirationDate = expDate 
            };
            
            EntityEntry<Asset> entity = db.Assets?.Add(a) ?? throw new ArgumentNullException("db.Assets?.Add(a)");
            AnsiConsole.MarkupLineInterpolated($"[yellow]State: {entity.State}, Asset ID: {a.Id}[/]");
            int affected = db.SaveChanges();
            AnsiConsole.MarkupLineInterpolated($"[yellow]State: {entity.State}, Asset ID: {a.Id}[/]");
            AnsiConsole.MarkupLineInterpolated($"[yellow]Affected rows: {affected} , Asset ID: {a.Id}[/]");
        }
        
        
        //Assets.Add(new Asset(brand, model, purchaseDate, priceUSD, Decimal.Round(priceLocal, 2), office, IdStore, type));

    }

    //Removes asset from list
    public void RemoveAsset(int id)
    {
        using (AssetDB db = new())
        {
            IQueryable<Asset>? assets = db.Assets?.Where(
                a => a.Id == id);
            if (assets is null || !assets.Any())
            {
                AnsiConsole.MarkupLineInterpolated($"[yellow]No asset with ID {id}[/]");
            }
            else
            {
                if (db.Assets is null) return;
                db.Assets.RemoveRange(assets);
            }
            int affected = db.SaveChanges();
            AnsiConsole.MarkupLineInterpolated($"[yellow]{affected} rows affected[/]");
        }
    }

    //List offices
    public void ListOffices()
    {
        using (AssetDB db = new())
        {
            var offices = db.Offices;
            var officesById =
                from o in offices
                select o;
            if (offices is null || !offices.Any())
            {
                AnsiConsole.MarkupLineInterpolated($"[red]No offices found.[/]");
                return;
            }
            var table = new Table()
                .RoundedBorder()
                .BorderColor(Color.Grey)
                .Title($"[green bold]Offices sorted by ID[/]");
            table.AddColumn($"[white bold]ID[/]");
            table.AddColumn("City");
            table.AddColumn("Currency");
            foreach (Office o in officesById )
            {
               table.AddRow(o.Id.ToString(), o.City ?? "", o.Currency ?? "");
            }
            AnsiConsole.Write(table);
        }
    }


    //Print sorted by asset type
    public void PrintAssets(OrederBy order)
    {
        using (AssetDB db = new())
        {
            IQueryable<Asset>? assets = db.Assets;
            IQueryable<Office>? locations = db.Offices;
            if (assets is null || !assets.Any())
            {
                AnsiConsole.MarkupLineInterpolated($"[red]No assets found.[/]");
                return;
            }
            //LINQ queries
            string selectedOrder = "BRAND";
            IOrderedQueryable<Asset> sortedData;

            var offices =
                from t in assets
                select t.Office;
            
            foreach (var v in offices )
            {
            }
            
            
            
            switch (order)
            {
                case OrederBy.Brand:
                    sortedData =
                        from t in assets
                        orderby t.AssetType
                        select t;
                    break;
                case OrederBy.Id:
                    sortedData =
                        from t in assets
                        orderby t.Id
                        select t;
                    selectedOrder = "ID";
                    break;
                case OrederBy.PurchaseDate:
                    sortedData =
                        from t in assets
                        orderby t.PurchaseDate
                        select t;
                    selectedOrder = "PURCHASE DATE";
                    break;
                case OrederBy.WarrantyExpiration:
                    sortedData =
                        from t in assets
                        orderby t.WarrantyExpirationDate
                        select t;
                    selectedOrder = "WARRANTY EXPIRY DATE";
                    break;
                default:
                    sortedData =
                        from t in assets
                        orderby t.Brand
                        select t;
                    break;
                
            }

            var table = new Table()
                .RoundedBorder()
                .BorderColor(Color.Grey)
                .Title($"[green bold]ASSETS ORDERED BY {selectedOrder}[/]");
            table.AddColumn($"[white bold]ID[/]");
            table.AddColumn("Brand");
            table.AddColumn("Model");
            table.AddColumn("Purchase Date");
            table.AddColumn("Price (USD)");
            table.AddColumn("Price (Local)");
            table.AddColumn("Warranty Expiry Date");
            table.AddColumn("Office");

            
            //Iterate over the sorted data
            foreach (var asset in sortedData)
            {
                //AnsiConsole.WriteLine($"Office: {asset.Office?.Id} {asset.Office?.City} {asset.Office?.Currency}");
                //Get currency prefix
                string? localPrefix = asset.Office?.Currency;
                //Get time since purchase
                TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
                //Print with color depending on time since purchase
                string color = "green";
                if (_redSpan.Subtract(timeSincePurchase).Days < 0)
                {
                    color = "red";
                }
                else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
                {
                    color = "yellow";
                }
                table.AddRow($"[{color}]{asset.Id.ToString()} [/]", $"[{color}]{asset.Brand} [/]", $"[{color}]{asset.ModelName} [/]" , $"[{color}]{asset.PurchaseDate.ToShortDateString()} [/]" ,
                    $"[{color}]{asset.PurchasePrice.ToString()} [/]" , $"[{color}]{asset.Office?.Currency}{asset.PriceLocal.ToString()} [/]", $"[{color}]{asset.WarrantyExpirationDate.ToShortDateString()} [/]" ,
                    $"[{color}]{asset.Office} [/]"  ?? "");
                
                
                
                
            }
            AnsiConsole.Write(table);
        }

    }

    

    //Print sorted by ID
    /*public void PrintByID()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.ID
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY ID");
        Console.WriteLine("");
        Console.WriteLine("{0,-5} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-15}",
            "ID", "Brand", "Model", "Purchase Date", "Price (USD)", "Price (Local)", "Office", "Type");
        Console.WriteLine("----------------------------------------------------------------------------" +
                          "---------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)

        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            //Print with color depending on time since purchase
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-5} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -15}",
                asset.ID, asset.Brand, asset.ModelName, asset.PurchaseDate.ToShortDateString(), "$" + asset.PurchasePrice,
                localPrefix + asset.PriceLocal, asset.OfficeLocation, asset.Type);
            Console.ResetColor();
        }

    }*/

    //Print sorted by brand
    /*public void PrintByBrand()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.Brand
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY BRAND");
        Console.WriteLine("");
        Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Brand", "Model", "Purchase Date", "Price (USD)", "Price (Local)", "Office", "Type", "ID");
        Console.WriteLine("----------------------------------------------------------------------------" +
                          "---------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)
        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            //Print with color depending on time since purchase
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.Brand, asset.ModelName, asset.PurchaseDate.ToShortDateString(), "$" + asset.PurchasePrice,
                localPrefix + asset.PriceLocal, asset.OfficeLocation, asset.Type, asset.ID);
            Console.ResetColor();
        }

    }*/

    //Print sorted by office
    /*public void PrintByOffice()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.OfficeLocation
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY OFFICE");
        Console.WriteLine("");
        Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Office", "Brand", "Model", "Purchase Date", "Price (USD)", "Price (Local)", "Type", "ID");
        Console.WriteLine("-----------------------------------------------------------------------------" +
                          "--------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)

        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
                //Print with color depending on time since purchase
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.OfficeLocation, asset.Brand, asset.ModelName, asset.PurchaseDate.ToShortDateString(), "$" + asset.PurchasePrice,
                localPrefix + asset.PriceLocal, asset.Type, asset.ID);
            Console.ResetColor();
        }

    }*/

    //Print sorted by model
    /*public void PrintByModel()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.ModelName
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY MODEL");
        Console.WriteLine("");
        Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Model", "Office", "Brand", "Purchase Date", "Price (USD)", "Price (Local)", "Type", "ID");
        Console.WriteLine("-----------------------------------------------------------------------------" +
                          "--------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)
        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
                //Print with color depending on time since purchase
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-15} | {1,-15} | {2,-15} | {3,-25} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.ModelName, asset.OfficeLocation, asset.Brand, asset.PurchaseDate.ToShortDateString(), "$" + asset.PurchasePrice,
                localPrefix + asset.PriceLocal, asset.Type, asset.ID);
            Console.ResetColor();
        }

    }*/

    //Print sorted by purchase date
    /*public void PrintByPurchaseDate()
    {
        //LINQ query
        var data = Assets.AsEnumerable();
        var sortedData =
            from t in data
            orderby t.PurchaseDate
            select t;
        //Print header
        Console.WriteLine("ASSETS ORDERED BY PURCHASE DATE");
        Console.WriteLine("");
        Console.WriteLine("{0,-25} | {1,-15} | {2,-15} | {3,-15} | {4,-15} | {5,-15} | {6,-15} | {7,-5}",
            "Purchase Date", "Model", "Office", "Brand", "Price (USD)", "Price (Local)", "Type", "ID");
        Console.WriteLine("-----------------------------------------------------------------------------" +
                          "--------------------------------------------------------------------");
        //Iterate over the sorted data
        foreach (var asset in sortedData)

        {
            //Get currency prefix
            string localPrefix = getCurrencyPrefix(asset);
            //Get time since purchase
            TimeSpan timeSincePurchase = DateTime.Today.Subtract(asset.PurchaseDate);
            //Print with color depending on time since purchase
            if (_redSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (_yellowSpan.Subtract(timeSincePurchase).Days < 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine("{0,-25} | {1,-15} | {2,-15} | {3,-15} | {4,-15} | {5,-15} | {6,-15} | {7, -5}",
                asset.PurchaseDate.ToShortDateString(), asset.ModelName, asset.OfficeLocation, asset.Brand, "$" + asset.PurchasePrice,
                localPrefix + asset.PriceLocal, asset.Type, asset.ID);
            Console.ResetColor();
        }

    }*/
    //Updates the  currency data
    public void UpdateCurrecyData()
    {   decimal d;
        //Get latest currency data
        var fx = new Freecurrencyapi("fca_live_GyuUHAV7ziIvpoHqYrSx5lGpSgmmme8xkXbOepyN");
        var latest = fx.Latest();
        var root = JsonDocument.Parse(latest).RootElement;
        var data = root.GetProperty("data");
        CurrencyData.Clear();
        CurrencyData.Add("SEK", data.GetProperty("SEK").GetDecimal());
        CurrencyData.Add("DKK", data.GetProperty("DKK").GetDecimal());
        CurrencyData.Add("EUR", data.GetProperty("EUR").GetDecimal());
        CurrencyData.Add("GBP", data.GetProperty("GBP").GetDecimal());
        CurrencyData.Add("JPY", data.GetProperty("JPY").GetDecimal());
    }
    private string GetCurrencyPrefix(string location)
    {
        switch (location)
        {
            case "Frankfurt":
                return "€";
            case "London":
                return "£";
            case "New York":
                return "$";
            case "Tokyo":
                return "¥";
            case "Copenhagen":
                return "DKK ";
            default:
                return "$";
        }
        
    }
    public decimal ConvertCurrency(string currency, decimal amount)
    {

        var value =  CurrencyData[currency];
        if (value == null)
        {
            return 0;
        }
        return (decimal) value * amount;
    }
}