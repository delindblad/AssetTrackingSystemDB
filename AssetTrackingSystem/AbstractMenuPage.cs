namespace MenuKit;

//Abstract base class for menu pages
public abstract class AbstractMenuPage
{
    public AbstractMenuPage? Context { get; set; } = null;
    public AbstractMenuPage? Parent { get; set; }
    public List<AbstractMenuPage> ChildPages { get; set; }
    public string Title { get; set; }

    public AbstractMenuPage(string title, AbstractMenuPage? parent)
    {
        Title = title;
        ChildPages = new List<AbstractMenuPage>();
        Context = Parent = parent;
    
        
    }
    //Adds child pages
    public void AddChildPage(AbstractMenuPage page)
    {
        page.Parent = this;
        ChildPages.Add(page);
    }

    //Displays information when loading page
    public abstract void Display();
    //Handle interaction here, returns int signaling result
    public abstract int Interact();
    //Runs page, returns new context
    public abstract AbstractMenuPage Run();
}