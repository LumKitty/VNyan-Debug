# VNyan node graph debug tool

![image](https://github.com/user-attachments/assets/5ad5e788-080d-4f12-8d21-83f0a63ba37c)

This tool aims to make debugging complex node graphs easier by providing a way to log messages in real time from within your node graphs.
Start it from the plugins menu, and then use the two provided functions:

```_lum_dbg_log``` - Log a message to the console. All inputs will be logged  
```_lum_dbg_err``` - The same thing, but in red  
```_lum_dbg_checkvars``` - Put this in your code if you want to log any changes to parameters

If you click the button a second time, a config UI will open, which allows you to specify a list of triggers. If you add these then you will get log entries every time that trigger is called. If you save it as DefaultTriggers.txt in your VNyan profile directory it will auto-load on startup

## Installation

Download from https://github.com/LumKitty/VNyan-Debug/releases  
Unzip the zipfile into VNyan\Items\Assemblies (no subdirectories)  
Yes the .exe is meant to be there. This thing works by spawning an external application and then sending messages to it. This is better than making a Unity UI for two reasons
1) More flexibility on where you put the window
2) Fuck making Unity UIs. If you can avoid doing this, you should

## https://twitch.tv/LumKitty
