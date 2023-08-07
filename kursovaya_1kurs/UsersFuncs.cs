using System;
using Ideas;
using Menu;

namespace Users
{
    class Userr
    {
        public static string file = @"..\..\..\\Users.txt";
        public static int row = 2;

        public static User thisUser = new User();

        static User[] users = { };
        static string[] usr = { };

        static int role = 0;
        public const int admin = 1;
        public const int user = 0;

        static string login = "";
        private static string password = "";

        public static string name;
        // TODO список придуманных идей

        public static void Restart_Input()
        {
            row = 2;
        }

        public static void Print(string x)
        {
            int col = Console.WindowWidth / 2 - x.Length / 2;
            Console.SetCursorPosition(col, row);
            Console.WriteLine(x);
            row++;
        }

        public static string Read()
        {
            Console.CursorVisible = true;
            int col = Console.WindowWidth / 2;
            Console.SetCursorPosition(col, row);
            row++;
            string x = Console.ReadLine();
            Console.CursorVisible = false;
            return x;
        }

        public static void AddAcc()
        {
            Restart_Input();
            Print("Сколько еще юзеров вы хотите добавить?");
            int kol_more = int.Parse(Read());
            users = new User[1];

            for (int i = 0; i < kol_more; i++)
            {
                AskLoginPassword(out login, out password);
                Array.Resize(ref users, users.Length + 1);
                users[i] = new User();
                users[i].Set(login,password,user);
                
                File.AppendAllText(file, $"{login} {password} {Userr.user}\n");
            }

            //CheckUsersList();


            Print("Успешно");
        }

        /*private static void CheckUsersList()
        {
            User[] copy = new User[1] { new User() };
            for(int i = 0; i < users.Length-1; i++)
            {
                if (!users[i].IsThemHere(copy))
                {
                    Array.Resize(ref copy, copy.Length + 1);
                    copy[copy.Length-1] = users[i];
                }
            }
            users = new User[1] { new User() };
            Array.Copy(copy, users, copy.Length);
        }*/

        static void AskLoginPassword(out string login, out string password)
        {
            Print("Введите логин:");
            login = Read().Replace(' ', '_');
            Print("Введите пароль:");
            password = Read().Replace(' ', '_');
        }

        public static void GetUsersFromFile(string file, out User[] users)
        {
            string[] usr = File.ReadAllLines(file);
            users = new User[usr.Length];
            string[] user;
            for(int i = 0; i < usr.Length; i++)
            {
                users[i] = new User();
                user = usr[i].Split(" ");
                (users[i].login, users[i].password, users[i].role) = (user[0], user[1], int.Parse(user[2]));
            }
        }

        static bool GetUsers(ref User[] users)
        {
            if (!File.Exists(file))
            {
                return false;
            }
            else
            {
                GetUsersFromFile(file, out users);
                return true;
            }
        }

        static bool CheckUser(string login, string password, out int role)
        {
            User user = new User();
            for (int i = 0; i < users.Length; i++)
            {
                user = users[i];
                if (user.login == login && user.password == password)
                {
                    role = user.role;
                    return true;
                }
            }
            role = 0;
            return false;
        }

        static void CoutRole(int role)
        {
            switch (role)
            {
                case admin:
                    Print("Ваша роль - админ");
                    break;
                case user:
                    Print("Ваша роль - юзер");
                    break;
            }
        }

        static void FirstUser()
        {
            Print("Похоже, еще нет ни одного юзера, добавьте свои данные:");
            File.Create(file).Close();

            AskLoginPassword(out string login, out string password);
            File.AppendAllLines(file, new string[] { $"{login} {password} {admin}" });
            role = admin;
            name = login;

            thisUser.Set(login, password, role);

            AddAcc();
        }

        static void Exit()
        {
            Menuu.DrawMenu(new string[] { "Готово" }, roww: row);
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    switch (role)
                    {
                        case admin:
                            Menuu.AdminMenu();
                            break;
                        case user:
                            Menuu.UserMenu();
                            break;
                    }
                    thisUser = new User();
                    return;
                }
            }
        }

        public static void LogIn()
        {
            Console.Clear();
            Restart_Input();
            if (GetUsers(ref users))
            {
                AskLoginPassword(out string login, out string password);
                if (CheckUser(login, password, out role))
                {
                    CoutRole(role);
                    name = login;

                    thisUser.Set(login, password, role);
                }
                else
                {
                    Print("Такого пользователя нет");
                    Menuu.DrawMenu(new string[] { "Готово" }, roww: row);
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                FirstUser();
            }
            Restart_Input();
            Exit();
        }
    }
}