namespace AssetTrackingSystem;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
public class Employee
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Department { get; set; }
    public Office? Office { get; set; }
    public string? Email { get; set; }
    public Asset[]? Assets { get; set; }
    public string[]? Roles { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}