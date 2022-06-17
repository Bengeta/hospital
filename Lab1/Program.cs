namespace Lab1;

public class Program
{
    public static void Main()
    {
        ConsoleKeyInfo key = new ConsoleKeyInfo();
        while (key.Key != ConsoleKey.A)
        {
            Console.Clear();
            var windowsManager = new WindowsManager();
            var layer = windowsManager.AddLayer();
            windowsManager.Draw();
            var cursorMode = false;
            bool cursorInHeader = false;
            var cursorPosition = 0;
            while (true)
            {
                key = Console.ReadKey();
                var currentWindow = windowsManager.showWindows[windowsManager.currentWindowIndex];
                var currentLayer = currentWindow.currentLayer;

                switch (@key.Key)
                {
                    case ConsoleKey.A:
                        Console.Clear();
                        if (currentWindow.x > 0)
                        {
                            currentWindow.Move(-1, 0);
                            windowsManager.Draw();
                        }

                        break;
                    case ConsoleKey.S:
                        Console.Clear();
                        currentWindow.Move(0, 1);
                        windowsManager.Draw();
                        break;
                    case ConsoleKey.D:
                        Console.Clear();
                        currentWindow.Move(1, 0);
                        windowsManager.Draw();
                        break;
                    case ConsoleKey.W:
                        Console.Clear();
                        if (currentLayer.y > 0)
                        {
                            currentWindow.Move(0, -1);
                            windowsManager.Draw();
                        }

                        break;
                    case ConsoleKey.Spacebar:
                        cursorMode = !cursorMode;
                        cursorInHeader = true;
                        Console.SetCursorPosition(currentLayer.Header.header.x - 1, currentLayer.Header.header.y);
                        break;
                    case ConsoleKey.DownArrow:
                        if (!cursorMode)
                            break;
                        if (!cursorInHeader)
                        {
                            cursorPosition = cursorPosition + 1;
                            if (cursorPosition == currentLayer.Field.list.Count)
                            {
                                cursorInHeader = true;
                                cursorPosition = 0;
                                Console.SetCursorPosition(currentLayer.Header.list[cursorPosition].x,
                                    currentLayer.Header.list[cursorPosition].y);
                                break;
                            }

                            Console.SetCursorPosition(currentLayer.Field.list[cursorPosition].x,
                                currentLayer.Field.list[cursorPosition].y);
                        }
                        else
                        {
                            cursorInHeader = false;
                            cursorPosition = 0;
                            if (currentLayer.Field.list.Count > 0)
                                Console.SetCursorPosition(currentLayer.Field.list[0].x, currentLayer.Field.list[0].y);
                        }

                        break;
                    case ConsoleKey.UpArrow:
                        if (!cursorMode)
                            break;
                        if (!cursorInHeader)
                        {
                            cursorPosition = cursorPosition - 1;
                            if (cursorPosition < 0)
                            {
                                cursorInHeader = true;
                                cursorPosition = 0;
                                Console.SetCursorPosition(currentLayer.Header.list[cursorPosition].x,
                                    currentLayer.Header.list[cursorPosition].y);
                                break;
                            }

                            Console.SetCursorPosition(currentLayer.Field.list[cursorPosition].x,
                                currentLayer.Field.list[cursorPosition].y);
                        }
                        else
                        {
                            cursorInHeader = false;
                            cursorPosition = currentLayer.Field.list.Count - 1;
                            if (currentLayer.Field.list.Count > 0)
                                Console.SetCursorPosition(currentLayer.Field.list[cursorPosition].x,
                                    currentLayer.Field.list[cursorPosition].y);
                        }

                        break;
                    case ConsoleKey.LeftArrow:
                        if (!cursorMode)
                            break;
                        if (cursorInHeader)
                        {
                            cursorPosition = (cursorPosition + 1) % currentLayer.Header.list.Count;
                            Console.SetCursorPosition(currentLayer.Header.list[cursorPosition].x,
                                currentLayer.Header.list[cursorPosition].y);
                        }

                        break;
                    case ConsoleKey.RightArrow:
                        if (!cursorMode)
                            break;
                        if (cursorInHeader)
                        {
                            cursorPosition = (cursorPosition - 1 + currentLayer.Header.list.Count) %
                                             currentLayer.Header.list.Count;
                            Console.SetCursorPosition(currentLayer.Header.list[cursorPosition].x,
                                currentLayer.Header.list[cursorPosition].y);
                        }

                        break;
                    case ConsoleKey.Enter:
                        if (!cursorMode)
                            break;
                        var x = 0;
                        var y = 0;
                        var isNeedDraw = true;
                        if (cursorInHeader)
                        {
                            isNeedDraw = currentLayer.Header.list[cursorPosition].MakeAction();
                            x = currentLayer.Header.list[cursorPosition].x;
                            y = currentLayer.Header.list[cursorPosition].y;
                        }
                        else
                        {
                            isNeedDraw = currentLayer.Field.list[cursorPosition].MakeAction(currentLayer.Field.list[cursorPosition].day);
                            x = currentLayer.Field.list[cursorPosition].x;
                            y = currentLayer.Field.list[cursorPosition].y;
                        }

                        if (isNeedDraw)
                        {
                            windowsManager.Draw();
                            cursorPosition = 0;
                            cursorInHeader = true;
                        }
                        else
                            Console.SetCursorPosition(x, y);
                        break;
                    case ConsoleKey.Z:
                        currentWindow.DrawWindow(0, false);
                        windowsManager.Draw();
                        break;
                    case ConsoleKey.O:
                        windowsManager.currentWindowIndex =
                            (windowsManager.currentWindowIndex - 1 + windowsManager.showWindows.Count) %
                            windowsManager.showWindows.Count;
                        currentWindow = windowsManager.showWindows[windowsManager.currentWindowIndex];
                        Console.SetCursorPosition(currentLayer.Header.header.x, currentLayer.Header.header.y);
                        break;
                    case ConsoleKey.P:
                        windowsManager.currentWindowIndex = (windowsManager.currentWindowIndex + 1) %
                                                            windowsManager.showWindows.Count;
                        currentWindow = windowsManager.showWindows[windowsManager.currentWindowIndex];
                        Console.SetCursorPosition(currentWindow.currentLayer.Header.header.x,
                            currentWindow.currentLayer.Header.header.y);
                        break;
                    case ConsoleKey.M:
                        currentWindow.Scale(2);
                        currentWindow.DrawWindow();
                        break;
                    case ConsoleKey.N:
                        currentWindow.Scale(-2);
                        currentWindow.DrawWindow();
                        break;
                }
            }
        }
    }
}