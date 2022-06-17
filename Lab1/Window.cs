namespace Lab1;

public class Window:View
{
    public Header Header;
    public Field Field;
    public String windowName;
    public Window(int x, int y, int w, int h,String windowName) : base(x, y, w, h)
    {
        Header = new Header(x, y, w, h / 5,windowName);
        Field = new Field(x, y + h / 5, w, h * 4 / 5);
        this.windowName = windowName;

    }

    public void Draw()
    {
        this.Header.Draw();
        if(!this.Header.isHide)
            this.Field.Draw();
        Console.SetCursorPosition(Header.header.x - 1, Header.header.y);
    }

    public void Move(int x, int y)
    {
        this.x += x;
        this.y += y;
        this.Field.Move(x, y);
        this.Header.Move(x, y );
    }
    public void Scale(int n)
    {
        this.width += n;
        this.height += n;
        this.Field.Scale(n);
        this.Header.Scale(n);
    }
    
}