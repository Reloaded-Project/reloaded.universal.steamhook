<div align="center">
	<h1>Reloaded II: Universal Steam Hook</h1>
	<img src="https://i.imgur.com/BjPn7rU.png" width="150" align="center" />
	<br/> <br/>
	<strong>Writing steam_appid.txt by hand?<br/></strong>
    <p>That's boring! Who would want to do that?</p>
<b>Id: reloaded.universal.steamhook</b>
</div>

# Prerequisites
The CRI FS Hook uses the [Hooks Shared Library](https://github.com/Sewer56/Reloaded.SharedLib.Hooks).
Please download and extract that mod first.

# About This Project

The following project is a [Reloaded II](https://github.com/Reloaded-Project/Reloaded-II) Mod Loader mod with one simple goal: help the Steam API initialize without having to restart the application via Steam.

Normally if you run a game from the filesystem and not via Steam it will tell you either that the Steam client is missing or automatically restart itself via Steam. As this interferes with Reloaded II's `Launch Application` method of injection, we want to avoid that.

This happens because the Steam API needs to somehow determine the App ID of the game, which is not always supplied by developers in their code.

# What This Project Does

The main function of this project is to simply drop a file named `steam_appid.txt` to the game directory before any Steam code is executed. This file informs Steam the App ID of the current application, allowing the API to initialize without having to launch the application via Steam.

The ID is determined by scanning the metadata of every single steam application in `steamapps` e.g. `appmanifest_213610.vdf` for every single library folder found in `\steamapps\libraryfolders.vdf`. Once metadata is found with an install location matching the current application, the App Id is extracted and written to the text file.

Originally this was done by hand but the code has since been replaced with the [Steam Apps Management API](https://github.com/Indieteur/Steam-Apps-Management-API) third party library.

In addition, just in case as contingency, this mod hooks the `SteamAPI_IsSteamRunning` and `SteamAPI_RestartAppIfNecessary` functions to return values that would make the game believe the API initialized correctly and a restart is not necessary.

# What This Project does NOT do.
- Circumvent DRM. (Not Interested!)
