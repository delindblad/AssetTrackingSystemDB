using Spectre.Console;
using Microsoft.EntityFrameworkCore.Design;

namespace AssetTrackingSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
public class AssetDB : DbContext
{

    public DbSet<Asset>? Assets { get; set; }
    public DbSet<Employee>? Employees { get; set; }
    public DbSet<Office>? Offices { get; set; }
    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {

        string? connectionString = Environment.GetEnvironmentVariable("CONNETION_STRING");
        // string connectionString =
        //     "Server=tcp:localhost;Database=AssetTest4;User ID=sa;Password=der6TIti6DER;Encrypt=false";
        AnsiConsole.WriteLine($"Connection: {connectionString}");
        optionsBuilder.UseSqlServer(connectionString);
        /*optionsBuilder.LogTo(AnsiConsole.WriteLine, new[] { RelationalEventId.CommandExecuting }) // This is the Console method.
#if DEBUG
            .EnableSensitiveDataLogging() // Include SQL parameters.
            .EnableDetailedErrors()
#endif*/
            ;
        
    }
    
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
       
    }
}