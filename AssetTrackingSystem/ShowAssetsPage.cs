using MenuKit;

namespace AssetTrackingSystem;

public class ShowAssetsPage : AbstractMenuPage
{
    AssetTracker tracker;
    private int option = 4;
    public ShowAssetsPage(string title, AbstractMenuPage? parent, AssetTracker t) : base(title, parent)
    {
        tracker = t;
    }

    public override void Display()
    {
        Console.Clear();
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("ASSET TRACKER");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine();
        Console.WriteLine("Type 'D' - to sort by purchase date, 'M' - to sort by Model, 'T' - to sort by Type, 'B' - to sort by brand, 'I' - to sort by ID, 'O' - to sort by office, 'Q' - to quit");
        Console.WriteLine();
        switch (option)
        {
            case 1:
                tracker.ListAssets(OrederBy.PurchaseDate);
                break;
            case 2:
                tracker.ListAssets(OrederBy.ModelName);
                break;
            case 3:
                tracker.ListAssets(OrederBy.Type);
                break;
            case 4:
                tracker.ListAssets(OrederBy.Brand);
                break;
            case 5:
                tracker.ListAssets(OrederBy.Id);
                break;
            case 6:
                tracker.ListAssets(OrederBy.OfficeLocation);
                break;
        }

        

    }

    public override int Interact()
    {
        var key = Console.ReadKey().KeyChar;
        if (key == 'Q' || key == 'q')
        {
            return 0;
        }
        else if (key == 'D' || key == 'd')
        {
            return 1;
        }
        else if (key == 'M' || key == 'm')
        {
            return 2;
        }
        else if (key == 'T' || key == 't')
        {
            return 3;
        }
        else if (key == 'B' || key == 'b')
        {
            return 4;
        }
        else if (key == 'I' || key == 'i')
        {
            return 5;
        }
        else if (key == 'O' || key == 'o')
        {
            return 6;
        }
        else
        {
            return -1;
        }
        
    }

    public override AbstractMenuPage Run()
    {
        while (true)
        {
            Display();
            option = Interact();
            if (option == -1)
            {
                continue;
            }
            else if (option == 0)
            {
                goto exit;
            }
        }
        exit:
        return Context;
    }
}