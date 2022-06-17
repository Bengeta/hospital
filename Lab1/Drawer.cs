namespace Lab1;

public static class Drawer
{
    public static void DrawHorizon(int x,int y,int xlength){
        for (int i=0;i<xlength;i++){
            Console.SetCursorPosition(x+i,y);
            Console.Write("-");
        }
    }
    public static void DrawVertical(int x,int y,int ylength){
        for (int i=0;i<ylength;i++){
            Console.SetCursorPosition(x,y+i);
            Console.Write("|");
        }
    }
    public static void SetCursorToEnd(int x){
        Console.SetCursorPosition(0,x);
    }
    public static void DeleteHorizon (int x,int y,int xlength){
        for (int i=0;i<xlength;i++){
            Console.SetCursorPosition(x+i,y);
            Console.Write(" ");
        }
    }
    
    public static void DrawFrame(int x, int y, int w, int h)
    {
        Console.SetCursorPosition(x, y);
        Console.Write("+");
        Drawer.DrawHorizon(x + 1, y, w);
        Console.SetCursorPosition(w + x, y);
        Console.Write("+");
        Drawer.DrawVertical(x, y + 1, h);
        Console.SetCursorPosition(x, h + y);
        Console.Write("+");
        Drawer.DrawHorizon(x + 1, y + h, w);
        Console.SetCursorPosition(w + x, h + y);
        Console.Write("+");
        Drawer.DrawVertical(x + w, y + 1, h - 1);
    }
}