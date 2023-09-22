# SteamShortcutManager
A basic C# class library for the purpose of making access of the shortcuts.vdf file used by Valve's Steam easier.  
  
Currently loses some information in the process, mainly flatpak and development information.  
Sadly, *nix systems are not supported fully yet. 
  
Feel free to critique or contribute improved code.  
  
Some todo's are scattered through the code, but here is the current to-do list:
* Automatic Path Detection for Linux in the Path&UserFinder
* Optimisation of
  * `GetName()` and `GetShortcutsPath()` in the Path&UserFinder
  * The stream reading process for opening the file.
* Adding Support for flatpaks and Devinfo
* Implementing the ability to use DateTime objects instead of int32 hex strings

## Code Examples
### Finding Steam Install Location
```csharp
PathAndUserFinder pathAndUserFinder = new PathAndUserFinder();
string steamPath = pathAndUserFinder.GetSteamPath();
string vdfPath = pathAndUserFinder.GetShortcutsPath(0);
```
### Reading Shortcuts VDF
```csharp
Reader r = new Reader();
List<Shortcut> b = r.GetShortcuts(vdfPath);
foreach (Shortcut o in b)
{
  o.PrintValues();
}
```
### Adding New Shortcut
```csharp
Reader r = new Reader();
List<Shortcut> b = r.GetShortcuts(vdfPath);
//creating example shortcut object
b.Add(new Shortcut("TEST","C://HOME","C://HOME","C://HOME","", "9C3C", new List<string>()));
```
### Writing to File
```csharp
Writer w = new Writer();
//outputs to the desktop
File.WriteAllText($@"C:\Users\{Environment.UserName}\Desktop\shortcuts-output.vdf", w.BuildShortcuts(b));
```

##### Thanks to [xblah](https://github.com/jean-knapp "xblah") for code contributions. 
