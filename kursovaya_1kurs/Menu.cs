using System;
using Users;
using Ideas;

namespace Menu
{
    class Menuu
    {
        static bool KeepGoing = true;
        static System.Action Action = () => { };

        public static void MainMenu()
        {
            Userr.thisUser.LoggedOut();

            string[] intro = { "���� �����", "��������� �� ������ ������" };
            string[] MenuItems = { "���� � �������", "�������� ����", "�����" };
            int index = 0; //������ ���������� ������ ����

            System.Action[] funcs = { LogIn, LookThrough, Exit };

            MenuItems = intro.Concat(MenuItems).ToArray();
            while (KeepGoing)
            {
                Console.Clear();
                DrawMenu(MenuItems, index, intro.Length); //��������� ����
                CheckKeys(ref index, MenuItems.Length, intro.Length, funcs);
            }
        }

        public static void AdminMenu()
        {
            string[] intro = {"���� ������" };
            string[] MenuItems = { "���������� ��������", "���������� ����", "�������� ����", "�����" };
            int index = 0; //������ ���������� ������ ����

            System.Action[] funcs = { 
                () => { Console.Clear(); Userr.AddAcc(); }, 
                Ideaa.AddIdea,
                LookThrough, 
                Exit};

            MenuItems = intro.Concat(MenuItems).ToArray();
            while (KeepGoing)
            {
                Console.Clear();
                DrawMenu(MenuItems, index, intro.Length); //��������� ����
                CheckKeys(ref index, MenuItems.Length, intro.Length, funcs);
            }
            KeepGoing = true;
        }

        public static void UserMenu()
        {
            string[] intro = { "���� ������������" };
            string[] MenuItems = { "���������� ����", "�������� ����", "�����" };
            int index = 0; //������ ���������� ������ ����

            System.Action[] funcs = {
                Ideaa.AddIdea,
                LookThrough,
                Exit};

            MenuItems = intro.Concat(MenuItems).ToArray();
            while (KeepGoing)
            {
                Console.Clear();
                DrawMenu(MenuItems, index, intro.Length); //��������� ����
                CheckKeys(ref index, MenuItems.Length, intro.Length, funcs);
            }
            KeepGoing = true;
        }

        public static void DrawMenu(string[] items, int index = 0, int intro = 0, int roww = 0)
        {
            int row = Console.WindowHeight/2 - items.Length/2 + roww;
            int col;
            for(int i = 0; i < items.Length; i++)
            {
                col = Console.WindowWidth/2 - items[i].Length/2;
                Console.SetCursorPosition(col, row+i);
                if(i == index + intro)
                {
                    //�������� ������ ��� ���������� ������ ����
                    Console.BackgroundColor = Console.ForegroundColor; 
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(items[i]);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        public static void CheckKeys(ref int index, int maxIndex, int minIndex, System.Action[] funcs)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.DownArrow:
                    if (index == maxIndex - minIndex - 1) index = -1;
                    index++;
                    break;
                case ConsoleKey.UpArrow:
                    if (index == 0) index = maxIndex - minIndex;
                    index--;
                    break;
                case ConsoleKey.Enter:
                    Action = funcs[index];
                    Action();
                    break;
            }
        }

        static void LogIn()
        {
            Userr.LogIn();
        }

        static void LookThrough()
        {
            Ideaa.ShowIdeas();
        }

        static void Exit()
        {
            string[] sure = { "�� �������?" };
            string[] items = { "��", "���" };
            int index = 1;
            bool back = false;
            items = sure.Concat(items).ToArray();
            System.Action[] funcs = { () => KeepGoing = false, () => { back = true; } };

            while (KeepGoing)
            {
                Console.Clear();
                DrawMenu(items, index, sure.Length);
                CheckKeys(ref index, items.Length, sure.Length, funcs);
                if (back)
                {
                    return;
                }
            }
        }
    }
}