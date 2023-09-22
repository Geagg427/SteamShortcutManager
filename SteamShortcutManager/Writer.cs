using System;
using System.Collections.Generic;
using System.Text;

namespace SteamShortcutManager
{ 
    public class Writer
    {
        //courtesy of jean again...
        public string BuildShortcuts(List<Shortcut> shortcuts)
        {
            string shortcutsString = "\u0000shortcuts\u0000";
            for (int i = 0; i < shortcuts.Count; i++)
            {
                shortcutsString += "\u0000" + i + "\u0000";
                shortcutsString += BuildShortcut(shortcuts[i]);
                shortcutsString += "\u0008";
            }
            shortcutsString += "\u0008\u0008";
            return shortcutsString;
        }

        private string BuildShortcut(Shortcut shortcut)
        {
            string shortcutString = "";
            //shortcutString += "\u0002appid\u0000" + shortcut.GetAppID() + "\u0000";
            shortcutString += "\u0001AppName\u0000" + shortcut.AppName + "\u0000";
            shortcutString += "\u0001Exe\u0000\"" + shortcut.Exe + "\"\u0000";
            shortcutString += "\u0001StartDir\u0000\"" + shortcut.StartDir + "\"\u0000";
            shortcutString += "\u0001icon\u0000" + shortcut.Icon + "\u0000";
            shortcutString += "\u0001ShortcutPath\u0000" + shortcut.ShortcutPath + "\u0000";
            shortcutString += "\u0001LaunchOptions\u0000" + shortcut.LaunchOptions + "\u0000";
            shortcutString += "\u0002IsHidden\u0000" + (shortcut.IsHidden ? "\u0001" : "\u0000") + "\u0000\u0000\u0000";
            shortcutString += "\u0002LastPlayTime\u0000" + shortcut.LastPlayTime; // + "\u0001Flat
            shortcutString += BuildTags(shortcut.Tags);
            return shortcutString;
        }
        private static string BuildTags(List<string> tags)
        {
            var tagString = "\u0000tags\u0000";
            if (tags != null)
            {
                for (var i = 0; i < tags.Count; ++i)
                {
                    tagString += "\u0001" + i + "\u0000" + tags[i] + "\u0000";
                }
            }
            tagString += "\u0008";
            return tagString;
        }
 
    }
}
