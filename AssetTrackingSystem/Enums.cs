namespace AssetTrackingSystem;

public enum AssetType
{
    
    Desktop = 0,
    Laptop = 1,
    Mobile = 2,
    Tablet = 3
}

public enum Location
{
    Copenhagen,
    Frankfurt,
    London,
    NewYork, 
    Tokyo
    
    
}

public enum OrederBy
{
    Type,
    Price,
    PurchaseDate,
    OfficeLocation,
    Id,
    Brand,
    WarrantyExpiration,
    ModelName
}