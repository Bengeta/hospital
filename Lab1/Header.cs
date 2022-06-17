using System.Runtime.CompilerServices;

namespace Lab1;

public class Header : View
{
    public TextArea header;
    public TextArea close;
    public TextArea hide;
    public TextArea addWindow;
    public bool isHide;
    private String headerName;
    public List<TextArea> list;

    public Header(int x, int y, int w, int h, String headerName) : base(x, y, w, h)
    {
        list = new List<TextArea>();
        AddHeader(x + w / 10, y + h / 2, headerName.Length, h / 100 * 50, headerName); // todo check how to add correct
        AddCloseButton(x + w / 100 * 95, y + h / 2, 1, h / 100 * 50);
        AddHideButton(x + w / 100 * 90, y + h / 2, 1, h / 100 * 50);
        AddWindowButton(x + w / 100 * 85, y + h / 2, 1, h / 100 * 50);
        this.headerName = headerName;
    }

    private void AddCloseButton(int x, int y, int w, int h)
    {
        Func<int,bool> action = ((int i) =>
        {
            Console.Clear();
            return false;
        });
        close = new TextArea(x, y, w, h, 1, "X", action);
        list.Add(close);
    }

    public void AddWindowButton(int x, int y, int w, int h)
    {
        Func<int,bool> action = ((int i) =>
        {
            Console.Clear();
            return false;
        });
        addWindow = new TextArea(x, y, w, h, 1, "+", action);
        list.Add(addWindow);
    }

    private void AddHideButton(int x, int y, int w, int h)
    {
        hide = new TextArea(x, y, w, h, 1, "_", ((int i) =>
        {
            isHide = !isHide;
            return true;
        }));
        list.Add(hide);
    }

    private void AddHeader(int x, int y, int w, int h, String name)
    {
        header = new TextArea(x, y, w, h, 0, name, (int i) => { return false; });
        list.Add(header);
    }

    public void Draw()
    {
        Drawer.DrawFrame(x, y, width, height);
        foreach (var item in list)
            item.Draw();
    }

    public void Move(int x, int y)
    {
        this.x += x;
        this.y += y;
        foreach (var item in list)
            item.Move(x,y);
    }
    public void Scale(int n)
    {
        this.width += n;
       // this.height += n;
        addWindow.Move(n, 0);
        hide.Move(n, 0);
        close.Move(n, 0);
    }
}