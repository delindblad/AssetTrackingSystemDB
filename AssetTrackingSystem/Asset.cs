using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using AssetTrackingSystem;


namespace AssetTrackingSystem;

public class Asset
{
    public int Id { get; set; }
    public string? AssetType { get; set; }
    public string? Brand { get; set; }
    public string? ModelName { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal PriceLocal { get; set; }
    public virtual Office? Office { get; set; }
    public string? SerialNumber { get; set; }
    public Employee? Employee { get; set; }
    public DateTime WarrantyExpirationDate { get; set; }

    
    

  

}

