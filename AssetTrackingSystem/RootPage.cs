using System.Text.Json;
using MenuKit;
using Spectre.Console;

namespace AssetTrackingSystem;

public class RootPage : AbstractMenuPage
{
    AssetTracker _tracker;
    public RootPage(string title, AssetTracker tracker) : base(title, null)
    {
        
        _tracker = tracker;
    }

    public override void Display()
    {
        Console.Clear();
        Console.WriteLine($"{Title}");
        var i = 1;
        foreach (var page in ChildPages)
        {
            Console.WriteLine($"({i}) {page.Title}");
            i++;
        }

        Console.WriteLine("");
    }

    public override int Interact()
    {
        Console.WriteLine("Select option:");
        var input = Console.ReadKey().KeyChar;
        if (input.ToString().Trim().ToLower() == "q")
        {
            return -1;
        }
        try
        {
            var n = int.Parse(input.ToString());
            Context = ChildPages[n - 1];
        }
        catch (Exception e)
        {
            Utilities.WritelnRed(e.ToString());
        }

        return 0;
    }

    public override AbstractMenuPage? Run()
    {
        Display();
        if (Interact() == -1)
        {   
            AnsiConsole.MarkupLine("\n[yellow]Exiting[/]");
            Environment.Exit(0);
            return null;
        }
        return Context;
    }
}