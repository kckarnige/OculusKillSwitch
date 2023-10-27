
<h3 align="center"></h3>
<p align="center">
    <picture>
        <source media="(prefers-color-scheme: dark)" srcset="./icon-dark.png" width="256px">
        <img alt="iCon" src="./icon.png" width="256px">
    </picture> 
</p>
<h1 align="center">Oculus Kill Switch</h1>

<h3 align="center">Toggle Oculus Killer and play your Oculus games.</h3>

<br>

> _Keep in mind, this is my first time coding in C#, that being said, if there are any bugs, [submit an issue](https://github.com/kckarnige/OculusKillSwitch/issues), and feel free to make [pull requests](https://github.com/kckarnige/OculusKillSwitch/pulls)._

## Installation

1. Download *([x64](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-7.0.12-windows-x64-installer) / [x86](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-7.0.12-windows-x86-installer))* and install the .NET 7.0 Runtime.

2. Download *([x64](https://github.com/kckarnige/OculusKillSwitch/releases/latest/download/OculusKillSwitch.exe) / [x86](https://github.com/kckarnige/OculusKillSwitch/releases/latest/download/OculusKillSwitch-x86.exe))* Oculus Kill Switch, and move it to where your Oculus Dash is located, should be at `C:\Program Files\Oculus\Support\oculus-dash\dash\bin` by default.

3. Run it, it should ask to make a desktop shortcut for you and ask to download Oculus Killer (if you don't already have it installed).

4. That's it, if it closes, run it again and enjoy!

## Libraries Used

- [*Mayerch1/GithubUpdateCheck*](https://github.com/Mayerch1/GithubUpdateCheck) - Used for checking GitHub releases

- [*rickyah/ini-parser*](https://github.com/rickyah/ini-parser) - Used for parsing the INI file

- [*ookii-dialogs/ookii-dialogs-wpf*](https://github.com/ookii-dialogs/ookii-dialogs-wpf) - Used for more Windows dialog options