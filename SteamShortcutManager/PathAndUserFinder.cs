using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ConsoleApp1
{
    public class PathAndUserFinder
    {
        //gets the steam installation path - doesn't work on linux
        public string GetSteamPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //64x
                var k = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam").GetValue("SteamPath");
                if (k != null) { return k.ToString(); }

                //32x (86x)
                k = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Valve\\Steam").GetValue("InstallPath");
                if (k != null) { return k.ToString(); }

                //failsafe
                Console.WriteLine("Automatic Steam Path Location Failed, Please Put URI to Steam Folder Here (do not include the steam executable):");
                return Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Sorry Linux users, I'll need you to pass your steam path in. Enter the path to the directory containing the steam executable. (if you have the capability I would appreciate it if you could modify the source code to allow for automatic install detection on linux)");
                return Console.ReadLine();
            } 
        }

        //gets a username for a given user folder - not efficiently
        public string GetName(string PathUserID) //todo: Optimise GetName()
        {
            //there is theoretically a method of getting this information from the registry but i can only find it for the currently signed in user
            FileStream fs = File.OpenRead(PathUserID + @"\config\localconfig.vdf");
            byte[] Bytes = new byte[11];
            while (System.Text.Encoding.UTF8.GetString(Bytes) != "PersonaName")
            {
                fs.Read(Bytes, 0, 11);
                fs.Seek(fs.Position - 10, SeekOrigin.Begin);
            }

            fs.Seek(fs.Position + 14, SeekOrigin.Begin);

            string Name = "";
            byte tempByte = (byte)fs.ReadByte();
            while (tempByte != '"')
            {
                Name += (char)tempByte;
                tempByte = (byte)fs.ReadByte();
            }
            return Name;
        }

        public string GetShortcutsPath(int? userNum = null) //todo: Rewrite GetShortcutsPath() user choice Section and validation
        {
            string usersPath = GetSteamPath() + @"\userdata\";
            #region get UserIds List
            List<string> UserIds = new List<string>(Directory.GetDirectories(usersPath));
            for (int i = 0; i < UserIds.Count; i++)
            {
                if (UserIds[i].Substring(UserIds[i].Length - 2) == "ac")
                {
                    UserIds.Remove(UserIds[i]); //removing the non account metadata file from the user list
                }
            }
            #endregion

            string returnString = ChooseUsers(userNum, usersPath, UserIds) + @"\config\shortcuts.vdf";
            returnString = returnString.Replace(@"\\", "\\");
            returnString = returnString.Replace("/", @"\");
            Console.WriteLine(returnString);
            return ValidateFile(returnString);
        }

        //helper functions
        private static string ValidateFile(string returnString)
        {
            #region Validate File
            byte[] buffer = new byte[18];
            try
            {
                File.OpenRead(returnString).Read(buffer, 0, 18);
            }
            catch
            {
                Console.WriteLine("Error reading file - It is likely the file does not exist");
            }
            if ((System.Text.Encoding.UTF8.GetString(buffer) == "\0shortcuts\0\00\0\u0001app")
                || System.Text.Encoding.UTF8.GetString(buffer) == "\0shortcuts\0\00\0\u0002app")
            {
                return returnString;
            }
            else
            {
                Console.WriteLine("Steam Path Has Not Resolved Successfully, Please try again.");
                throw new NotImplementedException();
            }
            #endregion
        }
        private string ChooseUsers(int? userNum, string usersPath, List<string> UserIds)
        {
            if (UserIds.Count > 1)
            {
                if (userNum == null) //if an arguement is not passed
                {
                    Console.WriteLine("Multiple Users Detected, Enter number corresponding to your steam id, enter any letter to choose the first option");
                    for (int i = 0; i < UserIds.Count; i++)
                    {
                        Console.WriteLine($"[{i}]: {UserIds[i]} - Name is likely {GetName(UserIds[i])}");
                    }


                    string inputNumber = Console.ReadLine();
                    int.TryParse(inputNumber, out int AccountIndex);

                    if (AccountIndex < UserIds.Capacity)
                    {
                        return UserIds[AccountIndex];
                    }
                    else
                    {
                        Console.WriteLine("Invalid input given, please try again.");
                        return ChooseUsers(null, usersPath, UserIds);
                    }
                }
                else
                {
                    if (userNum < UserIds.Capacity)
                    {
                        return UserIds[userNum ?? 0];
                    }
                    else
                    {
                        Console.WriteLine("Argument provided User Index was larger than the number of users: Manually Select please. ");
                        return ChooseUsers(null, usersPath, UserIds);
                    }
                }
            }
            //hack; possible error here; untested
            return Directory.GetDirectories(usersPath)[0]; //usersPath + Directory.GetDirectories(usersPath)[0];
        }


        
    }
}
