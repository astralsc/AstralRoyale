# ClashRoyale (2017)
[![clash royale](https://img.shields.io/badge/Clash%20Royale-1.9.2-brightred.svg?style=flat")](https://clash-royale.en.uptodown.com/android/download/1632865)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
![Build Status](https://action-badges.now.sh/retroroyale/ClashRoyale)

#### A .NET Core Clash Royale Server (v1.9.0 - v1.9.3)
##### Need help? Join our [Discord](https://discord.gg/8cHkNE6)

## Battles
The server supports battles, for those a patched client is neccessary.

[See the wiki for a tutorial](https://github.com/fdz6/ClashRoyale/wiki/Patch-for-battles)

## How to start

#### Requirements:
  - [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
  - [UwAmp (includes phpMyAdmin & MySQL):](https://www.uwamp.com/en/?page=download)

for Ubuntu use these commands to set it up:

###### Main Server:
```
mkdir ClashRoyale
git clone https://github.com/fdz6/AstralRoyale.git && cd AstralRoyale/ClashRoyale

dotnet publish "ClashRoyale.csproj" -c Release -o app
```
To configurate your server, such as the database you have to edit the ```config.json``` file.

#### Run the server:

###### Main Server:
```dotnet app/ClashRoyale.dll```

#### Update the server:
###### Main Server:
```git pull && dotnet publish "ClashRoyale.csproj" -c Release -o app && dotnet app/ClashRoyale.dll```

## Need help?
Contact me on Discord (fusiondevz) or open an issue.
