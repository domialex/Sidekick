# [![](https://i.imgur.com/1B5jR3D.png)](#) Sidekick [![](https://img.shields.io/github/v/release/domialex/sidekick?style=flat-square)](https://github.com/domialex/Sidekick/releases/latest/download/Sidekick.exe)
A Path of Exile helper that shows item prices using the official Path of Exile Trade API.

## Description
If you used [POE-TradeMacro](https://github.com/PoE-TradeMacro/POE-TradeMacro), it's the same idea.

Sidekick should be able to price check almost every item, the idea is to eventually be able to modify your search on-the-fly and configure which attributes are selected by default depending on the item type.

## Missing from this version
Unfortunately, one of the most asked for feature was not ready in time; The advanced search and item filters. Development is under way, but there is still a lot of issues and work to be done before we can release it.

As soon as it is ready, we will make a new release, it shouldn't be long.

## Beta?
Yes, this is still a beta since some core features are missing. We are thinking of removing the beta label once the advanced search / item filters are completed.

## Warning
This is a **BETA**, it will probably break in some cases, if it does, don't hesitate to [create an issue](https://github.com/domialex/Sidekick/issues).

## Installation
1. [Download Sidekick](https://github.com/domialex/Sidekick/releases/latest/download/Sidekick.exe)
2. Run Sidekick.exe.

## Usage
1. Run **Sidekick.exe**.
2. Select your league in the settings if needed.
3. Put **Path of Exile** in **Windowed** or **Windowed Fullscreen** mode for best results.
4. In **Path of Exile**, hover an item and press **Ctrl+D** (default binding).
5. To close the overlay, press **Space** (default binding).

## Features
### Check Prices
Default binding: Ctrl+D
Sidekick allows you to check prices of items from the official path of exile trade market. It is also possible to preview items by clicking on any result. For rare items, we get a price prediction from poeprices.info.
For unique items, we get a price from poe.ninja.
![image](https://user-images.githubusercontent.com/5131398/76627770-fce5f600-6511-11ea-886e-ee824a3720d6.png)

### League Overviews
Default binding: F6
Access quick cheatsheets about the various leagues.

#### Betrayal
![image](https://user-images.githubusercontent.com/5131398/76628398-091e8300-6513-11ea-916a-02d296b98248.png)

#### Blight
![image](https://user-images.githubusercontent.com/5131398/76628415-0facfa80-6513-11ea-9959-16fbe443a6c9.png)

#### Delve
![image](https://user-images.githubusercontent.com/5131398/76628428-163b7200-6513-11ea-966f-7242406246bd.png)

#### Incursion
![image](https://user-images.githubusercontent.com/5131398/76628445-1cc9e980-6513-11ea-9746-3aa3bba0ac16.png)

#### Metamorph
![image](https://user-images.githubusercontent.com/5131398/76628461-22bfca80-6513-11ea-81bf-5219fd06f803.png)

### Settings
All keybinds are customizable in the settings. The settings are found by right click the Sidekick icon in your Windows notification area.
![image](https://user-images.githubusercontent.com/5131398/76629436-a4fcbe80-6514-11ea-8063-fb00b2a6aa6e.png)
![image](https://user-images.githubusercontent.com/5131398/76629450-a9c17280-6514-11ea-8c9f-17220c9139e0.png)

### Go to Hideout
Default binding: F5
Allows you to quickly go to your hideout. Writes the following chat command: /hideout .

### Leave Party
Default binding: F4
Allows you to quickly leave a party. You must have set your character name in the settings first. Writes the following chat command: /kick {settings.Character_Name}

### Reply to Latest Whisper
Default binding: Ctrl+Shift+R
Allows you to reply to the last whisper you received quickly. Starts writing the following chat command: @{characterName} 

### Exit to Character Selection
Default binding: Ctrl+Shift+X
Quickly exits to the character selection screen. Writes the following chat command: /exit

### Tab Scrolling
Using Ctrl+Scroll will allow you to quickly switch tabs when in your stash. We also have unassigned keybinds to switch to the left or right tabs.

### Open Wiki
Default binding: Alt+W
Opens the wiki for the item you have your mouse over.

### Find Items
Default binding: Ctrl+F
Quickly search from an item into various screens (stash, passive skill tree).

### Open Search
Default binding: Alt+Q

## Development
We accept most PR and ideas. If you want a feature included, create an issue and we will discuss it. We are also available on [Discord](https://discord.gg/H4bg4GQ)

## Thanks
- [Contributors](https://github.com/domialex/Sidekick/graphs/contributors)
- [POE-TradeMacro](https://github.com/PoE-TradeMacro/POE-TradeMacro) - Original idea
- [WindowsHook](https://github.com/topstarai/WindowsHook) - Keyboard and mouse hooks
- [AdonisUI](https://benruehl.github.io/adonis-ui/) - UI
- [poe.ninja](https://poe.ninja/)
- [Poe Price Info](https://www.poeprices.info/)
