// using System.Text.Json;
// using MenuKit;
//
// namespace AssetTrackingSystem;
//
// public class RootPage : AbstractMenuPage
// {
//     AssetTracker _tracker;
//     public RootPage(string title, AssetTracker tracker) : base(title, null)
//     {
//         
//         _tracker = tracker;
//     }
//
//     public override void Display()
//     {
//         Console.Clear();
//         Console.WriteLine($"{Title}");
//         var i = 1;
//         foreach (var page in ChildPages)
//         {
//             Console.WriteLine($"({i}) {page.Title}");
//             i++;
//         }
//
//         Console.WriteLine("");
//     }
//
//     public override int Interact()
//     {
//         Console.WriteLine("Select option:");
//         var input = Console.ReadKey().KeyChar;
//         if (input.ToString().Trim().ToLower() == "q")
//         {
//             return -1;
//         }
//         try
//         {
//             var n = int.Parse(input.ToString());
//             Context = ChildPages[n - 1];
//         }
//         catch (Exception e)
//         {
//             Utilities.WritelnRed(e.ToString());
//         }
//
//         return 0;
//     }
//
//     public override AbstractMenuPage? Run()
//     {
//         Display();
//         if (Interact() == -1)
//         {
//             try
//             {
//                 Console.WriteLine();
//                 Utilities.WritelnGreen($"Saving to assets.json");
//                 string jsonString = JsonSerializer.Serialize(_tracker);
//                 File.WriteAllText("assets.json", jsonString);
//                 Thread.Sleep(2000);
//
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine(e);
//                 throw;
//             }
//             return null;
//         }
//         return Context;
//     }
// }