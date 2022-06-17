namespace Lab1;

public class Field : View
{
    public List<TextArea> list;

    public Field(int x, int y, int w, int h) : base(x, y, w, h)
    {
        list = new List<TextArea>();
    }

    public void AddText(int x, int y, int w, int h, int type, String text, Func<int,bool> action)
    {
        list.Add(new TextArea(this.x + x % width, this.y + y % height, w, h, type, text,
            action)); //todo check how to add %
    }

    public void Draw()
    {
        foreach (var item in list)
            (item as TextArea)?.Draw();


        Drawer.DrawFrame(x, y, width, height);
    }

    public void Move(int x, int y)
    {
        this.x += x;
        this.y += y;
        foreach (var item in list)
            (item as TextArea)?.Move(x, y);
    }
    public void Scale(int n)
    {
        this.width += n;
        this.height += n;
        /*foreach (var item in list)
            (item as TextArea)?.Move(0,n);*/
    }
}