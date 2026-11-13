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

    //Updates an new asset
    public void UpdateAsset(int id, string type, string brand, string model, DateTime purchaseDate, decimal priceUSD,
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
            

            IQueryable<Office>? offices = db.Offices;
            if (offices is null || !offices.Any())
            {
                AnsiConsole.MarkupLineInterpolated($"[red]No offices found.[/]");
                return;
            }
            
            
            Office o = offices.FirstOrDefault(o => o.City == office) ?? throw new ArgumentException($"Office {office} not found.");

            Asset a = GetAssetById(id);
            
            
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
    
    //Gets an array wih offices
    public Office[] GetOffices()
    {
        using (AssetDB db = new())
        {
            var offices = db.Offices;
            var officesById =
                from o in offices
                select o;
            
            return officesById.ToArray();
        }
    }


    //Check if asset exists
    public bool HasAsset(int id)
    {
        using (AssetDB db = new())
        {
            return db.Assets?.Any(a => a.Id == id) ?? false;
        }
    }
    
    //Print sorted by asset type
    public void ListAssets(OrederBy order)
    {
        using (AssetDB db = new())
        {
            IQueryable<Asset>? assets = db.Assets?.Include(a => a.Office);
            IQueryable<Office>? locations = db.Offices;
            if (assets is null || !assets.Any())
            {
                AnsiConsole.MarkupLineInterpolated($"[red]No assets found.[/]");
                return;
            }
            //LINQ queries
            string selectedOrder = "BRAND";
            IOrderedQueryable<Asset>? sortedData;

            var offices =
                from t in assets
                select t.Office;
            
            
            
            
            switch (order)
            {
                case OrederBy.Brand:
                    sortedData =
                        from t in assets
                        orderby t.AssetType
                        select t;
                    break;
                case OrederBy.ModelName:
                    sortedData =
                        from t in assets
                        orderby t.ModelName
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
    //Gets asset by ID
    public Asset GetAssetById(int id)
    {
        using (AssetDB db = new())
        {
            IQueryable<Asset>? assets = db.Assets?.Include(a => a.Office);
            if (assets is null || !assets.Any())
            {
                AnsiConsole.MarkupLineInterpolated($"[red]No assets found.[/]");
                return null!;
                
            }

                Asset? a = assets.FirstOrDefault(a => a.Id == id);
                if(a is null)
                {
                    AnsiConsole.MarkupLineInterpolated($"[red]No such asset.[/]");
                    return null!;
                }

                return a;
        }
    }

    
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