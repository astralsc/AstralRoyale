# <p align="center">AstralRoyale</p>
# <p align="center">Clash Royale server for 1.9.0 & 1.9.2</p>
[![clash royale](https://img.shields.io/badge/Clash%20Royale-1.9.2-brightred.svg?style=flat")](https://clash-royale.en.uptodown.com/android/download/1632865)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
![Build Status](https://action-badges.now.sh/retroroyale/ClashRoyale)

#### A .NET Core 2017 Clash Royale Server (v1.9.0 & v1.9.2)
##### Need help? Join our [Discord](https://discord.gg/mUredE6CTU)

## Features
```
1. More commands.
2. Searchable clans.
3. Maintenance system. (only enabled/disabled with the built in clan commands or the config.json file)
4. Clan Commands (including non admin/admin commands + added /admin & /ban command)
5. Update check if you're version is incorrect.
6. Extended the 2v2 button timestamp to 2038, I did this so you can see the 2v2 button)
7. Username, clan name/description/chat filtering system. (currently, the username only gets filtered when you change your existing username)
8. I forgot the other features.
```

## Battles
The server supports battles, for those a patched client is neccessary.

[See the wiki for a tutorials](https://github.com/fdz6/AstralRoyale/wiki/)

## How to start

#### Requirements:
  - [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
  - [UwAmp (includes phpMyAdmin & MySQL)](https://www.uwamp.com/en/?page=download)

```
I'm too lazy to upload all files here, so they are bundled in a .zip file that you'll need to extract to use this server.
If you're on Windows, the ClashRoyale folder has bat files to run the server.
If you're on Linux run the commands below.
```

###### Main Server:
```
cd /where/your/directory/you/put/ClashRoyale
dotnet publish "ClashRoyale.csproj" -c Release -o app
```
To configurate your server, such as the database you have to edit the ```config.json``` file.

#### Run the server:

###### Main Server:
```dotnet app/ClashRoyale.dll```

## Need help?
Contact me on Discord (fusiondevz) or open an issue.
