namespace Lab1;

public class Layer : View
{
    public List<Window> _containers;
    public List<Window> userWindows;
    public Window currentLayer;
    private int startWidth;
    private int startHeight;
    private Func<int, bool> _action;
    public bool isWork;
    private User currentUser;

    public Layer(int x, int y, int w, int h, Func<int, bool> action) : base(x, y, w, h)
    {
        _action = action;
        isWork = true;
        startHeight = h;
        startWidth = w;
        currentUser = new User();
        _containers = new List<Window>();
        userWindows = new List<Window>();
        _containers.Add(MakeMainWindow(x, y, w, h));
        _containers.Add(MakeWindowRegistration(x, y, w, h));
        _containers.Add(MakeWindowRegistrationSecsess(x, y, w, h));
        _containers.Add(MakeNewWindow(x, y, w, h));
        _containers.Add(MakeWindowSelect(x, y, w, h));
        _containers.Add(MakeWindowSignIn(x, y, w, h)); //5
        _containers.Add(MakeMainMenu_(x, y, w, h));
        _containers.Add(MakeWeekMenu(x, y, w, h));
        _containers.Add(MakeDayMenu(x, y, w, h));
        _containers.Add(MakeAddPatientWindow(x, y, w, h));
        currentLayer = _containers[5]; //5
    }

    private Window MakeMainWindow(int x, int y, int w, int h)
    {
        String sr1 = "1) Открыть распсание на неделю";
        String sr2 = "2) Выйти";
        String sr3 = "Добро Пожаловать " + currentUser.name;
        var window = new Window(this.x, this.y, this.width, this.height, "Window1");
        window.Header.addWindow._action = _action;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };
        window.Field.AddText(1, 1, sr1.Length, 1, 1, sr1, (int i) =>
        {
            currentLayer = _containers[7];
            ReDrawWeekMenu(BDRequests.GetWeekPatients(currentUser.id));
            return true;
        });
        window.Field.AddText(1, 2, sr1.Length, 1, 1, sr2, (int i) =>
        {
            currentLayer = _containers[5];
            ReDrawSignInWindow();
            return true;
        });
        return window;
    }

    private void ReDrawMainWindow()
    {
        _containers[0].Header.list[0].text = currentUser.name;
    }

    private Window MakeWeekMenu(int x, int y, int w, int h)
    {
        String sr3 = "Назад";
        var week = new List<String>() {"Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс"};
        var window = new Window(this.x, this.y, this.width, this.height, "Window1");
        window.Header.addWindow._action = _action;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };
        int y_ = 1;
        for (int i = 0; i < week.Count; i++)
        {
            window.Field.AddText(1, y_, 2, 1, 1, week[i], (int i) =>
            {
                ReDrawDayMenu(BDRequests.GetDayPatients(i, currentUser.id));
                currentLayer = _containers[8];
                return true;
            });
            window.Field.list[i].day = i + 1;

            y_++;
        }

        window.Field.AddText(1, 8, sr3.Length, 1, 1, sr3, (int i) =>
        {
            currentLayer = _containers[0];
            return true;
        });
        return window;
    }

    private Window MakeDayMenu(int x, int y, int w, int h)
    {
        var window = new Window(this.x, this.y, this.width, this.height, "Window1");
        window.Header.addWindow._action = _action;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };

        return window;
    }

    private void ReDrawDayMenu(List<Patient> patients)
    {
        String sr3 = "Назад";
        String sr4 = "Добавить";
        _containers[8].Field.list.Clear();
        _containers[8].Field.AddText(1, 1, sr3.Length, 1, 1, sr3, (int i) =>
        {
            currentLayer = _containers[7];
            return true;
        });
        _containers[8].Field.AddText(1, 2, sr4.Length, 1, 1, sr4, (int i) =>
        {
            RedrawAddPatientWindow(null);
            currentLayer = _containers[9];
            return true;
        });
        for (int i = 0; i < patients.Count; i++)
        {
            var record = patients[i].timeStart + " - " + patients[i].timeEnd + " " + patients[i].name.Split(' ')[0];
            _containers[8].Field.list.Add(
                new TextArea(this.x + 1, _containers[8].Field.list[1].y + 1 + i, record.Length, 1, 1, record,
                    (int i) =>
                    {
                        RedrawAddPatientWindow(patients[i]);
                        currentLayer = _containers[9];
                        return true;
                    }));
            _containers[8].Field.list[i + 2].day = i;
        }
    }

    private void ReDrawWeekMenu(List<int> patients)
    {
        for (int i = 0; i < patients.Count; i++)
        {
            _containers[7].Field.list[i].textField = " " + patients[i];
        }
    }

    private Window MakeAddPatientWindow(int x, int y, int w, int h)
    {
        String sr1 = "1) Фио:";
        String sr2 = "2) Время начала:";
        String sr3 = "3) Время конца:";
        String sr6 = "4) День недели:";
        String sr4 = "5) Добавить";
        String sr5 = "6) Назад";
        String sr7 = "7) Удалить";
        var window = new Window(this.x, this.y, this.width, this.height, "Window1");
        window.Header.addWindow._action = _action;
        var window_ = window.Field.list;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };
        window.Field.AddText(1, 1, sr1.Length, 1, 1, sr1, (int i) => { return false; });
        window_[0]._action = (int i) =>
        {
            if (window_[0].textField != null)
            {
                Drawer.DeleteHorizon(window_[0].text.Length + 1, window_[0].y, window_[0].textField.Length);
            }

            window_[0].Validation(window_[0].x, window_[0].y, window_[0].width, 1);
            return false;
        };
        window.Field.AddText(1, 2, sr2.Length, 1, 1, sr2, (int i) => { return false; });
        window_[1]._action = (int i) =>
        {
            if (window_[1].textField != null)
            {
                Drawer.DeleteHorizon(window_[1].text.Length + 1, window_[1].y, window_[1].textField.Length);
            }

            window_[1].Validation(window_[1].x, window_[1].y, window_[1].width, 4);
            return false;
        };
        window.Field.AddText(1, 3, sr3.Length, 1, 1, sr3, (int i) => { return false; });
        window_[2]._action = (int i) =>
        {
            if (window_[2].textField != null)
            {
                Drawer.DeleteHorizon(window_[2].text.Length + 1, window_[2].y, window_[2].textField.Length);
            }

            window_[2].Validation(window_[2].x, window_[2].y, window_[2].width, 4);
            return false;
        };
        window.Field.AddText(1, 4, sr6.Length, 1, 1, sr6, (int i) => { return false; });
        window_[3]._action = (int i) =>
        {
            if (window_[3].textField != null)
            {
                Drawer.DeleteHorizon(window_[3].text.Length + 1, window_[3].y, window_[3].textField.Length);
            }

            window_[3].Validation(window_[2].x, window_[3].y, window_[3].width, 5);
            return false;
        };
        window.Field.AddText(1, 5, sr4.Length, 1, 1, sr4, (int i) =>
        {
            if (window_[0].textField != null && window_[1].textField != null && window_[2].textField != null &&
                window_[3].textField != null)
            {
                var newPatient = new Patient();
                newPatient.day = int.Parse(window_[3].textField);
                var patientList = BDRequests.GetDayPatients(newPatient.day, currentUser.id);
                newPatient.name = window_[0].textField;
                newPatient.id = window_[0].id;
                newPatient.timeStart = ConverToTimeOnly(window_[1].textField);
                newPatient.timeEnd = ConverToTimeOnly(window_[2].textField);
                foreach (var patient in patientList)
                {
                    if (newPatient.id == patient.id)
                        continue;
                    if (newPatient.timeStart.IsBetween(patient.timeStart, patient.timeEnd) ||
                        newPatient.timeEnd.IsBetween(patient.timeStart, patient.timeEnd))
                    {
                        return true;
                    }
                }

                List<PatientEntity> list;
                if (_containers[9].Field.list[4].text != "5) Добавить")
                {
                    var oldPatient = new Patient();
                    oldPatient.day = int.Parse(window_[3].textFieldBuff);
                    oldPatient.name = window_[0].textFieldBuff;
                    oldPatient.timeStart = ConverToTimeOnly(window_[1].textFieldBuff);
                    oldPatient.timeEnd = ConverToTimeOnly(window_[2].textFieldBuff);
                    list = BDRequests.UpdatePatient(oldPatient, newPatient, currentUser.id);
                }
                else
                    list = BDRequests.AddPatient(newPatient, currentUser.id);


                if (list != null)
                {
                    ReDrawWeekMenu(BDRequests.GetWeekPatients(currentUser.id));
                    currentLayer = _containers[7];
                    window_[0].textField = null;
                    window_[1].textField = null;
                    window_[2].textField = null;
                    return true;
                }
            }

            return false;
        });
        window.Field.AddText(1, 6, sr5.Length, 1, 1, sr5, (int i) =>
        {
            ReDrawWeekMenu(BDRequests.GetWeekPatients(currentUser.id));
            currentLayer = _containers[7];
            return true;
        });
        window.Field.AddText(1, 7, sr7.Length, 1, 1, sr7, (int i) =>
        {
            var oldPatient = new Patient();
            oldPatient.day = int.Parse(window_[3].textFieldBuff);
            oldPatient.name = window_[0].textFieldBuff;
            oldPatient.timeStart = ConverToTimeOnly(window_[1].textFieldBuff);
            oldPatient.timeEnd = ConverToTimeOnly(window_[2].textFieldBuff);
            BDRequests.DeletePatient(oldPatient);
            ReDrawWeekMenu(BDRequests.GetWeekPatients(currentUser.id));
            currentLayer = _containers[7];
            return true;
        });

        return window;
    }

    private TimeOnly ConverToTimeOnly(string time)
    {
        var time_ = time.Split(':');
        return new TimeOnly(int.Parse(time_[0]), int.Parse(time_[1]));
    }

    private void RedrawAddPatientWindow(Patient? patient, String error = "Добавление пациента")
    {
        if (patient != null)
        {
            _containers[9].Field.list[0].textField = patient.name;
            _containers[9].Field.list[0].id = patient.id;
            _containers[9].Field.list[1].textField = patient.timeStart.ToString();
            _containers[9].Field.list[2].textField = patient.timeEnd.ToShortTimeString();
            _containers[9].Field.list[3].textField = patient.day.ToString();
            _containers[9].Field.list[4].text = "5) Изменить";
            for (int i = 0; i < 4; i++)
                _containers[9].Field.list[i].textFieldBuff = _containers[9].Field.list[i].textField;
        }
        else
        {
            for (int i = 0; i < 4; i++)
                _containers[9].Field.list[i].textField = null;
            _containers[9].Field.list[4].text = "5) Добавить";
        }
    }

    private Window MakeWindowSignIn(int x, int y, int w, int h)
    {
        String sr3 = "1) Email:";
        String sr2 = "2) Пароль:";
        String sr4 = "3) Sign in:";
        String sr5 = "4) Sign up:";

        var window = new Window(this.x, this.y, this.width, this.height, "Вход");
        window.Header.addWindow._action = _action;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };
        var window_ = window.Field.list;
        window.Field.AddText(1, 1, sr3.Length, 1, 2, sr3, (int i) => { return false; });
        window_[0]._action = (int i) =>
        {
            window_[0].Validation(window_[0].x, window_[0].y, window_[0].width, 3);
            return false;
        };
        window.Field.AddText(1, 2, sr2.Length, 1, 2, sr2, (int i) => { return false; });
        window_[1]._action = (int i) =>
        {
            window_[1].Validation(window_[1].x, window_[1].y, window_[1].width, 2);
            return false;
        };

        window.Field.AddText(1, 3, sr4.Length, 1, 2, sr4, (int i) =>
        {
            if (window_[1].textField != null && window_[0].textField != null)
            {
                var name = BDRequests.SignIn(window_[0].textField, window_[1].textField);
                if (name != null)
                {
                    currentUser = name;
                    currentLayer = _containers[0];
                    ReDrawMainWindow();
                }
                else
                {
                    ReDrawSignInWindow("Неправильный пароль и email");
                }
            }

            return true;
        });

        window.Field.AddText(1, 4, sr5.Length, 1, 2, sr5, (int i) =>
        {
            ReDrawSignUpWindow();
            currentLayer = _containers[1];
            return true;
        });


        return window;
    }

    private void ReDrawSignInWindow(String error = "Вход")
    {
        _containers[5].Header.header.text = error;
        foreach (var item in _containers[5].Field.list)
            item.textField = null;
    }

    private Window MakeWindowRegistration(int x, int y, int w, int h)
    {
        String sr1 = "1) Фио:";
        String sr3 = "2) Email:";
        String sr2 = "3) Пароль:";
        String sr4 = "4) Повторите Пароль:";
        String sr5 = "5) Sign up:";
        String sr6 = "6) Sign in:";

        var window = new Window(this.x, this.y, this.width, this.height, "Регистрация");
        window.Header.addWindow._action = _action;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };
        var window_ = window.Field.list;
        window.Field.AddText(1, 1, sr1.Length, 1, 2, sr1, (int i) => { return false; });
        window_[0]._action = (int i) =>
        {
            window_[0].Validation(window_[0].x, window_[0].y, window_[0].width, 1);
            return false;
        };
        window.Field.AddText(1, 2, sr3.Length, 1, 2, sr3, (int i) => { return false; });
        window_[1]._action = (int i) =>
        {
            window_[1].Validation(window_[1].x, window_[1].y, window_[1].width, 3);
            return false;
        };
        window.Field.AddText(1, 3, sr2.Length, 1, 2, sr2, (int i) => { return false; });
        window_[2]._action = (int i) =>
        {
            window_[2].Validation(window_[2].x, window_[2].y, window_[2].width, 2);
            return false;
        };

        window.Field.AddText(1, 4, sr4.Length, 1, 2, sr4, (int i) => { return false; });
        window_[3]._action = (int i) =>
        {
            window_[3].Validation(window_[3].x, window_[3].y, window_[3].width, 2);
            return false;
        };

        window.Field.AddText(1, 5, sr5.Length, 1, 2, sr5, (int i) =>
        {
            if (window_[0].textField != null && window_[1].textField != null && window_[2].textField != null &&
                window_[3].textField == window_[2].textField)
                switch (BDRequests.SignUp(window_[1].textField, window_[2].textField, window_[0].textField))
                {
                    case 200:
                        currentUser = BDRequests.SignIn(window_[1].textField, window_[2].textField);
                        ReDrawMainWindow();
                        currentLayer = _containers[0];
                        break;
                    case 100:
                        ReDrawSignUpWindow("Email знанят");
                        break;
                }

            return true;
        });
        window.Field.AddText(1, 6, sr6.Length, 1, 2, sr6, (int i) => { return false; });
        window_[5]._action = (int i) =>
        {
            ReDrawSignInWindow();
            currentLayer = _containers[5];
            return true;
        };
        return window;
    }

    private void ReDrawSignUpWindow(String error = "Регистрация")
    {
        _containers[1].Header.header.text = error;
        foreach (var item in _containers[1].Field.list)
            item.textField = null;
    }


    private Window MakeNewWindow(int x, int y, int w, int h)
    {
        String sr1 = "1) Введите название окна:";

        var window = new Window(this.x, this.y, this.width, this.height, "Добавление Окна");
        window.Header.addWindow._action = _action;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };
        window.Field.AddText(1, 1, sr1.Length, 1, 2, sr1, (int i) => { return false; });
        window.Field.list[0]._action = (int i) =>
        {
            var window_ = window.Field.list[0];
            var result = window_.Validation(window.Field.list[0].x, window.Field.list[0].y,
                window.Field.list[0].width,
                0);
            var windowName = window.Field.list[0].textField;
            if (windowName != null)
            {
                userWindows.Add(new Window(this.x, this.y, this.width, this.height, windowName));
                DrawWindow(userWindows.Count - 1, true);
                _containers[4].Field.AddText(1, userWindows.Count, windowName.Length, 1, 1, windowName, (int i) =>
                {
                    currentLayer = userWindows[^1];
                    return true;
                });
            }

            return true;
        };

        return window;
    }

    private Window MakeWindowRegistrationSecsess(int x, int y, int w, int h)
    {
        String sr1 = "Вы зарегистрированы!";

        var window = new Window(this.x, this.y, this.width, this.height, "Регистрация");
        window.Header.addWindow._action = _action;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };
        window.Field.AddText(1, 50, sr1.Length, 1, 0, sr1, (int i) => { return false; });
        return window;
    }

    private Window MakeWindowSelect(int x, int y, int w, int h)
    {
        var window = new Window(this.x, this.y, this.width, this.height, "Выберите окно");
        window.Header.addWindow._action = _action;
        return window;
    }

    public void DrawWindow(int i, bool isUserWindow)
    {
        if (!isUserWindow)
        {
            _containers[i].Draw();
            currentLayer = _containers[i];
        }
        else
        {
            userWindows[i].Draw();
            currentLayer = userWindows[i];
        }

        Console.SetCursorPosition(currentLayer.Header.header.x - 1, currentLayer.Header.header.y);
    }

    public void DrawWindow()
    {
        Console.Clear();
        currentLayer.Draw();
        Console.SetCursorPosition(currentLayer.Header.header.x - 1, currentLayer.Header.header.y);
    }

    public void Move(int x, int y)
    {
        this.x += x;
        this.y += y;
        foreach (var window in _containers)
            window.Move(x, y);
        foreach (var window in userWindows)
            window.Move(x, y);
    }

    public void Scale(int n)
    {
        if (this.width + n < startWidth)
            return;
        this.width += n;
        this.height += n;
        foreach (var window in _containers)
            window.Scale(n);
        foreach (var window in userWindows)
            window.Scale(n);
    }


    private Window MakeMainMenu_(int x, int y, int w, int h)
    {
        String sr1 = "Добро пожаловать " + currentUser.name;
        String sr2 = "1) Открыть меню расписания";
        String sr3 = "2) Выйти";
        var window = new Window(this.x, this.y, this.width, this.height, "Window1");
        window.Header.addWindow._action = _action;
        window.Header.close._action = (int i) =>
        {
            isWork = false;
            return true;
        };
        window.Field.AddText(1, 1, sr1.Length, 1, 1, sr1, (int i) => { return false; });
        window.Field.AddText(1, 2, sr1.Length, 1, 1, sr2, (int i) =>
        {
            currentLayer = _containers[0];
            return true;
        });
        window.Field.AddText(1, 3, sr1.Length, 1, 1, sr3, (int i) =>
        {
            currentLayer = _containers[5];
            return true;
        });
        return window;
    }
}