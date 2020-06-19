# [![](https://i.imgur.com/1B5jR3D.png)](#) Sidekick [![](https://img.shields.io/github/v/release/domialex/sidekick?style=flat-square)](https://github.com/domialex/Sidekick/releases/latest/download/Sidekick.exe)
A Path of Exile helper that shows item prices using the official Path of Exile Trade API.

## Description
If you used [POE-TradeMacro](https://github.com/PoE-TradeMacro/POE-TradeMacro), it's the same idea.

Sidekick should be able to price check almost every item, the idea is to eventually be able to modify your search on-the-fly and configure which attributes are selected by default depending on the item type.

## Installation
1. [Download Sidekick](https://github.com/domialex/Sidekick/releases/latest/download/Sidekick.exe)
2. Install **[.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-desktop-3.1.4-windows-x64-installer)** if needed
3. Run Sidekick.exe

## Usage
1. Add an exception for **Sidekick.exe** in your anti-virus, some people have reported that it can block Sidekick in some cases.
2. Run **Sidekick.exe**.
3. Select your league in the settings if needed.
4. Put **Path of Exile** in **Windowed** or **Windowed Fullscreen** mode for best results.
5. In **Path of Exile**, hover an item and press **Ctrl+D** (default binding).

## Features
### Price check - Ctrl+D
Sidekick allows you to price check items using the official Path of Exile trade market. You can compare and preview items by clicking on any result. For rare items, a price prediction from poeprices.info is shown.
For unique items, prices from poe.ninja are used.
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/price-check.png)

### League Overviews - F6
Access quick cheatsheets about the various leagues.

#### Betrayal
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/overlay-betrayal.png)

#### Blight
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/overlay-blight.png)

#### Delve
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/overlay-delve.png)

#### Incursion
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/overlay-incursion.png)

#### Metamorph
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/overlay-metamorph.png)

### Settings - Ctrl+O
All keybinds are customizable. The settings are also found by right-clicking the Sidekick icon in your Windows notification area.
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/settings-general.png)
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/settings-keybindings.png)

### Go to Hideout - F5
Quickly go to your hideout. Writes the following chat command: `/hideout`.

### Leave Party - F4
Quickly leave a party. You must have set your character name in the settings first. Writes the following chat command: `/kick {settings.Character_Name}`

### Reply to Latest Whisper - Ctrl+Shift+R
Reply to the last whisper received. Starts writing the following chat command: `@{characterName}`

### Exit to Character Selection - Ctrl+Shift+X
Exit to the character selection screen. Writes the following chat command: `/exit`

### Tab Scrolling - Ctrl+Scroll-wheel
Switch tabs in your stash using your scroll-wheel, you can also bind keys instead.

### Open Wiki - Alt+W
Open the wiki for the item currently under your mouse.

### Find Items - Ctrl+F
Search an item in your stash or passive tree.

### Check For Dangerous Map Mods - Ctrl+X
Check if a map has any dangerous mods. Mods are configurable.
![image](https://raw.githubusercontent.com/domialex/Sidekick/master/docs/assets/images/map-dangerous.png)

### Open Search - Alt+Q
Open the official trade website using the item under your mouse.

## Development
We accept most PR and ideas. If you want a feature included, create an issue and we will discuss it.

We are also available on [Discord](https://discord.gg/H4bg4GQ).

[![](https://img.shields.io/discord/664252463188279300?color=%23738AD6&label=Discord&style=flat-square)](https://discord.gg/H4bg4GQ)


## Thanks
- [Contributors](https://github.com/domialex/Sidekick/graphs/contributors)
- [POE-TradeMacro](https://github.com/PoE-TradeMacro/POE-TradeMacro) - Original idea
- [WindowsHook](https://github.com/topstarai/WindowsHook) - Keyboard and mouse hooks
- [AdonisUI](https://benruehl.github.io/adonis-ui/) - UI
- [poe.ninja](https://poe.ninja/)
- [Poe Price Info](https://www.poeprices.info/)
