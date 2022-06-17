using System.Runtime.CompilerServices;

namespace Lab1;

public class TextArea : View
{
    public String text;
    public Func<int, bool> _action;
    public String? textField = null;
    public String? textFieldBuff = null;
    public int day = -1;
    public int id = -1;

    public TextArea(int x, int y, int w, int h, int type, String text, Func<int, bool> action) : base(x, y, w, h)
    {
        this.type = type; // 0 - text, 1 - button, 2 - input field
        this.text = text;
        _action = action;
    }

    public void Draw(int x, int y, int w, int h, string Text)
    {
        Console.SetCursorPosition(x, y);
        for (int k = 0; k == h - y; k++)
            Console.SetCursorPosition(x, k + y);
        for (int i = 0; i < w; i++)
        {
            Console.Write(Text[i]);
        }
    }

    public void Draw()
    {
        var text_ = text;
        if (textField != null)
            text_ += textField;
        Console.SetCursorPosition(x, y);
        for (int k = 0; k == height - y; k++)
            Console.SetCursorPosition(x, k + y);
        for (int i = 0; i < text_.Length; i++)
        {
            Console.Write(text_[i]);
        }
    }

    public void Draw(String text)
    {
        Console.SetCursorPosition(x, y);
        for (int k = 0; k == height - y; k++)
            Console.SetCursorPosition(x, k + y);
        for (int i = 0; i < text.Length; i++)
        {
            Console.Write(text[i]);
        }
    }

    public bool MakeAction(int i = 0)
    {
        return _action(i);
    }

    public bool Validation(int x, int y, int w, int type)
    {
        Console.SetCursorPosition(x + w + 1, y);
        string? input = null;
        switch (type)
        {
            case 0: // название окна
                input = Console.ReadLine();
                if (input == null) return false;
                textField = input;
                break;
            case 1: //фио
                input = Console.ReadLine();
                if (input == null) return false;
                var fio = input.Split(' ').ToList();
                if (fio.Count == 3)
                {
                    textField = input;
                    return true;
                }

                break;
            case 2: //password
                bool read = true;
                while (read)
                {
                    var key = Console.ReadKey();
                    var position = Console.GetCursorPosition();
                    if (position.Left > 0)
                        Console.SetCursorPosition(position.Left - 1, position.Top);
                    switch (@key.Key)
                    {
                        case ConsoleKey.Backspace:
                            Console.SetCursorPosition(position.Left - 2, position.Top);
                            Drawer.DeleteHorizon(position.Left - 2, position.Top, 2);
                            Console.SetCursorPosition(position.Left - 2, position.Top);
                            input = input?.Remove(input.Length - 1);
                            break;
                        case ConsoleKey.Enter:
                            read = false;
                            break;
                        default:
                            Console.Write('*');
                            input += key.Key;
                            break;
                    }
                }

                if (input != null)
                {
                    textField = input;
                    return true;
                }

                break;
            case 3: //email
                input = Console.ReadLine();
                if (input == null) return false;
                if (input.Contains('@'))
                {
                    textField = input;
                    return true;
                }

                break;
            case 4: //time
                input = Console.ReadLine();
                if (input == null) return false;
                if (input.Contains(':'))
                {
                    var in_ = input.Split(':');
                    int a;
                    if (in_.Length == 2 && int.TryParse(in_[0], out a) && int.TryParse(in_[1], out a))
                    {
                        var i1 = int.Parse(in_[0]);
                        var i2 = int.Parse(in_[1]);
                        if (i1 >= 8 && i1 <= 17 && i2 >= 0 && i2 <= 60)
                        {
                            textField = input;
                            return true;
                        }
                    }
                }

                break;
            case 5: //day
                input = Console.ReadLine();
                if (input == null) return false;
                // if (int.TryParse(input))
            {
                var t = int.Parse(input);
                if (t < 8 && t > 0)
                {
                    textField = input;
                    return true;
                }
            }

                break;
        }

        if (input != null)
            Drawer.DeleteHorizon(x + w + 1, y, input.Length);
        return false;
    }
}