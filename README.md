# SteamShortcutManager
A basic C# class library for the purpose of making access of the shortcuts.vdf file used by Valve's Steam easier. 
Feel free to critique or improve code, some todo's are scattered through the code, but here is the current to-do list:
* Automatic Path Detection for Linux in the Path&UserFinder
* Optimisation of
  * `GetName()` and `GetShortcutsPath()` in the Path&UserFinder
  * The stream reading process for opening the file.
* Adding Support for flatpaks
## Thanks to [xbla](https://github.com/jean-knapp "xblah") for code contributions. 
