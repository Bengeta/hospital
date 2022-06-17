namespace Lab1;

public abstract class View
{
     public int x;
     public int y;
     public int width;
     public int height;
     public int type;
    
    public View(int x, int y, int w, int h)
    {
        this.x = x;
        this.y = y;
        this.width = w;
        this.height = h;
    }

    public void Draw(){}

    public void Move(int x, int y)
    {
        this.x += x;
        this.y += y;
    }
    public void Scale(int n)
    {
        this.x += n;
        this.y += n;
    }
}