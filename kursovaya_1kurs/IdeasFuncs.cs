using System;
using System.Data;
using System.Security.Cryptography;
using Menu;
using Users;

namespace Ideas
{
    class Ideaa
    {
        public static string folder = @"..\..\..\Ideas";
        public static string ideas_list = @"..\..\..\Ideas.txt";

        public static bool all = true;

        static (int line, int word) author = (1, 2);

        static string txt = ".txt";
        static string sup = "sup.txt";

        static Idea[] all_ideas = { };
        static Idea[] matched = { };

        static string tab = "   ";

        public static int col_ideas = 0;
        static int index = 0;
        static bool KeepGoing = true;

        public static string name = "";
        public static string description = "";
        public static string tag = "";
        // TODO список добавившихся
        // TODO функция добавиться

        static void GetIdeas()
        {
            if (!File.Exists(ideas_list))
            {
                File.Create(ideas_list);
            }

            string[] ideas = File.ReadAllLines(ideas_list);
            string author_line;
            string sups;
            all_ideas = new Idea[ideas.Length];

            for(int i = 0; i < ideas.Length; i++)
            {
                all_ideas[i] = new Idea();
                sups = ideas[i].Replace(txt, sup);

                all_ideas[i].SetPath(ideas[i], sups);
                all_ideas[i].name = File.ReadLines(all_ideas[i].path).First();
                author_line = File.ReadLines(all_ideas[i].path).Skip(author.line).First();
                all_ideas[i].author = author_line.Split(" ")[author.word];
                Userr.GetUsersFromFile(sups, out all_ideas[i].supported);
            }
        }

        public static void Exit()
        {
            Console.BackgroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Userr.Print("Выход");
            Console.ResetColor();
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Userr.Restart_Input();
                    return;
                }
            }
        }

        public static string ShowSupported(int col)
        {
            if (col == 0)
            {
                return "Идею еще никто не поддержал";
            }
            if (col == 1)
            {
                return "Всего идею поддержал 1 пользователь";
            }
            if (col < 5)
            {
                return $"Всего идею поддержало {col} пользователя";
            }
            return $"Всего идею поддержали {col} пользователей";
        }

        public static void SaveIdea(Idea idea, string name, string[] lines)
        {
            string file = idea.path;
            File.Create(file).Close();

            File.AppendAllText(file, name + '\n');
            File.AppendAllText(file, $"Автор - {Userr.name}\n");
            File.AppendAllLines(file, lines);

            File.Create(file.Replace(txt, sup)).Close();
        }

        public static void CheckPovtor(ref string file, string file_name)
        {
            col_ideas = 0;
            while (File.Exists(file))
            {
                col_ideas++;
                file = $"{folder}\\{file_name}{col_ideas}.txt";
            }
        }

        public static void AddIdea()
        {
            Console.Clear();
            Userr.Restart_Input();
            Idea idea = new Idea();
            Userr.Print("Введите название идеи");
            name = Userr.Read();

            string file_name = name.Replace(' ', '_');
            string file = $"{folder}\\{file_name}{col_ideas}.txt";
            
            CheckPovtor(ref file, file_name);

            idea.SetPath(file, file.Replace(txt, sup));
            idea.SetAuthor(Userr.name);
            idea.name = name;
            File.AppendAllLines(ideas_list, new string[] {file});


            Array.Resize(ref all_ideas, all_ideas.Length + 1);
            all_ideas[all_ideas.Length - 1] = idea;

            ReadIdea(out string[] lines);

            SaveIdea(idea, name, lines);

            Exit();
        }

        private static void ReadIdea(out string[] lines)
        {
            lines = new string[1];
            int i = 0;
            Userr.Print("Введите Вашу идею в несколько строк, в конце просто еще раз нажмите [Enter]");
            string x = " ";
            Console.CursorVisible = true;
            do
            {
                Console.Write('\t');
                x = Console.ReadLine();
                Userr.row++;
                lines[i] = x;
                i++;
                Array.Resize(ref lines, i + 1);
            } while (x != "");
            Console.CursorVisible = false;
        }

        private static void ShowTheIdea(Idea id)
        {
            string[] idea = File.ReadAllLines(id.path);

            Userr.Print(idea[0]);

            for (int i = 1; i < idea.Length; i++)
            {
                Console.WriteLine('\t' + idea[i]);
                Userr.row++;
            }

            Console.WriteLine('\t' + ShowSupported(id.supported.Length));
        }

        public static void ShowIdeas()
        {
            Console.Clear();
            GetIdeas();
            all = true;
            if (all_ideas.Length == 0)
            {
                string[] sorry = { $"К сожалению идей еще нет" };
                Userr.Print(sorry[0]);
                Exit();
            }
            else
            {
                index = 0;
                int ind = 2;
                int line = 0;
                while (KeepGoing)
                {
                    Console.Clear();
                    Userr.Restart_Input();
                    ShowTheIdea(all_ideas[index]);
                    MiniMenu(ref ind, ref line);
                }
                KeepGoing = true;
            }
        }

        private static int Lens(string[,] str, int wich)
        {
            int k = 0;
            for(int i = 0; i < str.GetLength(1); i++)
            {
                k += str[wich, i].Length;
            }
            return k;
        }

        private static void MenuLinePrint(string[,] menu, int wich, bool here, int index)
        {
            int col = Console.WindowWidth / 2 - Lens(menu, wich) / 2;
            int row = Console.WindowHeight / 2 + wich;

            if (Userr.row + 2 > row)
            {
                row = Userr.row + 2;
            }

            Console.SetCursorPosition(col, row);

            for (int i = 0; i < menu.GetLength(1); i++)
            {
                if (here && i == index)
                {
                    Console.BackgroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.Write(menu[wich, i]);
                Console.ResetColor();
                if(menu[wich, i].Length > 0)
                {
                    Console.Write(tab);
                }                
            }
            Userr.row++;
        }

        private static void SortFirstUp()
        {
            Array.Sort(all_ideas, (Idea a, Idea b) => b.supported.Length.CompareTo(a.supported.Length));
            all_ideas.Reverse();
            index = 0;
        }

        private static void SortFirstDown()
        {
            Array.Sort(all_ideas, (Idea a, Idea b) => a.supported.Length.CompareTo(b.supported.Length));
            index = 0;
        }

        private static string GetName()
        {
            Console.Clear();
            Userr.Restart_Input();
            Userr.Print("Введите название идеи, которую хотите найти: ");
            return Userr.Read();
        }

        private static void SearchByName(string name)
        {
            matched = new Idea[0];
            int i = -1;
            foreach (Idea idea in all_ideas)
            {
                if (idea.name == name)
                {
                    i++;
                    Array.Resize(ref matched, i + 1);
                    matched[i] = idea;
                }
            }
        }

        private static void Search()
        {
            SearchByName(GetName());
            if(matched.Length == 0)
            {
                No("Идей с таким названием не найдено");
            }
            else
            {
                all = false;
                index = 0;
                int ind = 2;
                int line = 0;
                while (KeepGoing)
                {
                    Console.Clear();
                    Userr.Restart_Input();
                    ShowTheIdea(matched[index]);
                    MiniMenu(ref ind, ref line);
                }
                KeepGoing = true;
            }
        }

        private static void MiniMenu(ref int ind, ref int line)
        {
            string[,] mm = { { "Предыдущая", "Выход", "Следующая" }, 
                             { "Удалить", "-", "Поддержать" }, 
                             { "Поиск по идеям", "Сначала популярные", "Сначала непопулярные" } };

            for (int i = 0; i < mm.GetLength(0); i++)
            {
                MenuLinePrint(mm, i, line == i, ind);
            }

            CheckArrows(ref line, ref ind, ref mm);
        }

        private static void CheckArrows(ref int line, ref int ind, ref string[,] mm)
        {
            System.Action[,] funcs = { { Previos, () => { KeepGoing = false; }, Next }, 
                                     { DeleteTheIdea, () => { }, Support }, 
                                     {Search, SortFirstUp, SortFirstDown } };

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.RightArrow:
                    if (ind == mm.GetLength(1) - 1) ind = -1;
                    ind++;
                    if (line == 1 && ind == 1) ind++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (ind == 0) ind = mm.GetLength(1);
                    ind--;
                    if (line == 1 && ind == 1) ind--;
                    break;
                case ConsoleKey.UpArrow:
                    if(line == 0) line = mm.GetLength(0);
                    line--;
                    break;
                case ConsoleKey.DownArrow:
                    if(line == mm.GetLength(0)-1) line = -1;
                    line++;
                    break;
                case ConsoleKey.Enter:
                    System.Action act = () => { };
                    act = funcs[line, ind];
                    act();
                    break;
            }
        }

        static void Next()
        {
            if (index == all_ideas.Length - 1 && all)
            {
                index = -1;
            }
            else if(index == matched.Length - 1 && !all)
            {
                index = -1;
            }
            index++;
        }

        static void Previos()
        {
            if (index == 0 && all)
            {
                index = all_ideas.Length;
            }
            else if(index == 0 && !all)
            {
                index = matched.Length;
            }
            index--;
        }

        static void DeleteTheIdea()
        {
            if (all_ideas[index].author == Userr.thisUser.login || Userr.thisUser.role == Userr.admin)
            {
                File.Delete(all_ideas[index].path);
                File.Delete(all_ideas[index].pathOfSup);

                for (int i = index; i < all_ideas.Length - 1; i++)
                {
                    all_ideas[i] = all_ideas[i + 1];
                }

                Array.Resize(ref all_ideas, all_ideas.Length - 1);
                index--;

                UpdateIdeasList();

                Success();
            }
            else
            {
                No("Вы не автор");
            }
        }

        private static void UpdateIdeasList()
        {
            File.WriteAllText(ideas_list, null);
            string name;
            foreach(Idea idea in all_ideas)
            {
                name = idea.path;
                name.Replace(folder + "\\", "").Replace(txt, "");
                File.AppendAllLines(ideas_list, new string[] { name });
            }

        }

        private static void No(string intro)
        {
            Console.Clear();
            string[] no = { intro };
            string[] exit = { "Выход" };

            no = no.Concat(exit).ToArray();

            Menuu.DrawMenu(no, intro: no.Length - exit.Length);
            if(Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return;
            }
        }

        private static void Success()
        {
            Console.Clear();
            string[] success = { "Успешно" };
            string[] exit = { "Выход" };

            success.Concat(exit).ToArray();

            Menuu.DrawMenu(success, intro: success.Length - exit.Length);
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                return;
            }
        }

        static void Support()
        {
            if(Userr.thisUser.role == -1)
            {
                No("Сначала войдите в аккаунт");
                return;
            }
            if(all_ideas[index].author != Userr.thisUser.login && !Userr.thisUser.IsThemHere(all_ideas[index].supported))
            {
                File.AppendAllText(all_ideas[index].pathOfSup, $"{Userr.thisUser.login} {Userr.thisUser.password} {Userr.thisUser.role}\n");
                Success();
            }
            else
            {
                if(all_ideas[index].author == Userr.thisUser.login)
                { 
                    No("Вы автор");
                }
                else
                {
                    No("Вы уже поддержали эту идею");
                }
            }

        }
    }
}