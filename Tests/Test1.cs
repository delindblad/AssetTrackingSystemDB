using AssetTrackingSystem;

namespace TestProject1;
//AI GENERATED!
public class Test1
{
    [Fact]
    public void Test()
    {
        var tracker = new AssetTracker();
        var purchaseDate = new DateTime(2024, 1, 15);

        tracker.AddAsset(AssetType.Laptop, "Apple", "MacBook Pro", purchaseDate, 2000m, Location.NewYork);
        tracker.AddAsset(AssetType.Mobile, "Samsung", "Galaxy S23", purchaseDate, 800m, Location.NewYork);

        Assert.Equal(2, tracker.Assets.Count);

        var firstAsset = tracker.Assets[0];
        Assert.Equal(0, firstAsset.ID);
        Assert.Equal("Apple", firstAsset.Brand);
        Assert.Equal("MacBook Pro", firstAsset.Model);
        Assert.Equal(purchaseDate, firstAsset.PurchaseDate);
        Assert.Equal(2000m, firstAsset.PriceUSD);
        Assert.Equal(2000m, firstAsset.PriceLocal);
        Assert.Equal(Location.NewYork, firstAsset.Office);
        Assert.Equal(AssetType.Laptop, firstAsset.Type);

        var secondAsset = tracker.Assets[1];
        Assert.Equal(1, secondAsset.ID);
        Assert.Equal("Samsung", secondAsset.Brand);
        Assert.Equal("Galaxy S23", secondAsset.Model);
        Assert.Equal(AssetType.Mobile, secondAsset.Type);

        tracker.RemoveAsset(0);
        Assert.Single(tracker.Assets);
        Assert.Equal(secondAsset, tracker.Assets[0]);
    }
}
