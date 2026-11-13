namespace AssetTrackingSystem;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
public class Office
{
    public int Id { get; set; }
    public string? City { get; set; }
    public string? Currency { get; set; }
    
    public virtual HashSet<Asset> Assets { get; set; } = new HashSet<Asset>();

    public override string? ToString()
    {
        return City;
    }
    
    
    

}