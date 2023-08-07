using System;
using System.Security.Cryptography;
using Menu;
using Users;

namespace Ideas
{
    class Idea
    {
        public string author = "";
        public string path = "";
        public string pathOfSup = "";
        public User[] supported = { };

        public string name = "";

        public void SetPath(string file, string fileSup)
        {
            path = file;
            pathOfSup = fileSup;
        }

        public void SetAuthor(string they)
        {
            author = they;
        }
    }
}