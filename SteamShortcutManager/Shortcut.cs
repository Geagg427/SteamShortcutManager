using System;
using System.Collections.Generic;
using System.Text;

namespace SteamShortcutManager
{
    public class Shortcut
    {
        #region variables              
        public string Appid { get; }
        public string AppName { get; set; }
        public string Exe { get; set; }
        public string StartDir { get; set; }
        public string ShortcutPath { get; set; }
        public string LaunchOptions { get; set; }
        public bool IsHidden { get; set; } //isHidden seems to be deprecated - may just be a tag similar to favourites and custom ones
        public bool? AllowDesktopConfig { get; set; }
        public bool? AllowOverlay { get; set; }
        public bool? OpenVr { get; set; }
        public bool? DevKit { get; set; }
        public string DevkitID { get; set; }
        public string LastPlayTime { get; set; }
        public List<string> Tags { get; set; } //tags are just the collection indicators? 
        public string Icon { get; set; }
        #endregion

        public Shortcut(string _AppName, string _Exe, string _StartDir, string _ShortcutPath, string _LaunchOptions,string _LastPlayTime, List<string> _tags)
        {
            AppName = _AppName;
            Exe = _Exe;
            StartDir = _StartDir;
            ShortcutPath = _ShortcutPath;
            LaunchOptions = _LaunchOptions;
            LastPlayTime = _LastPlayTime;
            Tags = _tags;
        }

        public Shortcut() { }


        public void PrintValues()
        {
           
            Console.WriteLine(
                       $"  {AppName}\n" +
                       $"  Exe: {Exe ?? "not yet implemented"}\n" +
                       $"  StartDir: {StartDir ?? "not yet implemented"}\n" +
                       $"  Shortcut Path:{ShortcutPath ?? "not yet implemented"}\n" +
                       $"  Launch Options: {LaunchOptions ?? "not yet implemented"}\n" +
                       $"  is hidden?: not yet implemented\n" +
                       $"  Last Play Time: {Reader.ConvertHexDateToDateTime(LastPlayTime)}\n" + //+ " or in computer Terms: " + Encoding.ASCII.GetBytes(LastPlayTime)[2].ToString("X") + Encoding.ASCII.GetBytes(LastPlayTime)[3].ToString("X")}\n" +
                       $"  Icon: {Icon} \n\n");
        }
    }





}
