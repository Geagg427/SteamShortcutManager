using SteamShortcutManager;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExampleConsoleApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //Getting Steam Path & User Shortcuts.VDF File
            PathAndUserFinder pathAndUserFinder = new PathAndUserFinder();
            string vdfPath = pathAndUserFinder.GetShortcutsPath(0);

            //Reading
            Reader r = new Reader();
            List<Shortcut> b = r.GetShortcuts(vdfPath);
            foreach (Shortcut o in b)
            {
                o.PrintValues();
            }

            //adding new shortcuts
            b.Add(new Shortcut("TEST","C://HOME","C://HOME","C://HOME","", "9C3C", new List<string>()));
            
           
            //Writing To File
            Writer w = new Writer();
            File.WriteAllText($@"C:\Users\{Environment.UserName}\Desktop\shortcuts-output.vdf", w.BuildShortcuts(b));
     

            Console.ReadKey();
        }
        
    }
}
