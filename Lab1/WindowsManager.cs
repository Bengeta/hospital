namespace Lab1;

public class WindowsManager
{
    public List<Layer> showWindows;
    public int currentWindowIndex;

    public WindowsManager()
    {
        showWindows = new List<Layer>();
    }

    public Layer AddLayer()
    {
        var layer = new Layer(0, 0, 100, 15, (int i) =>
        {
            AddLayer();
            return true;
        });
        showWindows.Add(layer);
        return layer;
    }

    public void Draw()
    {
        Console.Clear();
        foreach (var layer in showWindows)
        {
            if(layer.isWork)
                layer.currentLayer.Draw();
        }
    }


}