using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Text;

namespace SteamShortcutManager
{
    public class Reader
    {
        public List<Shortcut> GetShortcuts(string path)
        {
            List<Shortcut> Shortcuts = new List<Shortcut>();

            Shortcut shortcut = new Shortcut();

            //code here stolen & modified from https://github.com/jean-knapp at https://developer.valvesoftware.com/wiki/Steam_Library_Shortcuts on 1/09/23, 
            //todo: add flatpak support 
            
            #region streamreader version
            StreamReader stream = new StreamReader(path);

            string word = "";
            string key = "";
            bool readingTags = false;
            int tagId = -1;

            while (!stream.EndOfStream)
            {
                char c = Convert.ToChar(stream.Read());

                if (c == '\u0000')
                {
                    if (word.EndsWith("\u0001AppName"))//|| word.EndsWith("\u0001appname"))
                    {
                        if (shortcut != null)
                            Shortcuts.Add(shortcut);
                        // New shortcut
                        shortcut = new Shortcut();
                        key = "\u0001appname";
                    }
                    else if (
                        word == "\u0001Exe" ||
                        word == "\u0001StartDir" ||
                        word == "\u0001icon" ||
                        word == "\u0001ShortcutPath" ||
                        word == "\u0001LaunchOptions" ||
                        word == "\u0002hidden" ||
                        word == "\u0002LastPlayTime"
                        )
                    {
                        key = word;
                    }
                    else if (word == "tags") { readingTags = true; }
                   
                    else if (key != "")
                    {
                        switch (key)
                        {
                            case "\u0001appname":
                                shortcut.AppName = word;
                                break;
                            case "\u0001Exe":
                                shortcut.Exe = word.Trim('"');
                                break;
                            case "\u0001StartDir":
                                shortcut.StartDir = word.Trim('"');
                                break;
                            case "\u0001icon":
                                shortcut.Icon = word;
                                break;
                            case "\u0001ShortcutPath":
                                shortcut.ShortcutPath = word;
                                break;
                            case "\u0001LaunchOptions":
                                shortcut.LaunchOptions = word;
                                break;
                            case "\u0002LastPlayTime":
                                shortcut.LastPlayTime = word.Remove(word.IndexOf("\u0001Flat")); //prevents reading "/u0001flatpakid" into the last played time); 
                                break;
                            case "\u0002IsHidden":
                                shortcut.IsHidden = (word == "\u0001");
                                break;
                            default:
                                break;
                        }
                        key = "";
                    }
                    else if (readingTags)
                    {
                        if (word.StartsWith("\u0001"))
                        {
                            tagId = int.Parse(word.Substring("\u0001".Length));
                        }
                        else if (tagId >= 0)
                        {
                            shortcut.Tags.Add(word);
                            tagId = -1;
                        }
                        else
                        {
                            readingTags = false;
                        }
                    }
                    word = "";
                }
                else { word += c; }
            }





            #endregion



            //todo: optimise stream reader to not increment by character, but increment until it hits a specific character that is the end of a string ( \u0000? )



            Shortcuts.RemoveAt(0);
            return Shortcuts; //Uncertain of cause,but a random null entry is at the beginning
        }

        public static DateTime ConvertHexDateToDateTime(string HexDate)
        {
            byte[] b = Encoding.ASCII.GetBytes(HexDate);
            return DateTimeOffset.FromUnixTimeSeconds(BitConverter.ToInt32(b, 0)).DateTime;
        }


      
    }
    
}
