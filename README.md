# <p align="center">AstralRoyale</p>
# <p align="center">Clash Royale server for 1.9.0 - 1.9.2</p>
[![clash royale](https://img.shields.io/badge/Clash%20Royale-1.9.2-brightred.svg?style=flat")](https://clash-royale.en.uptodown.com/android/download/1632865)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
![Build Status](https://action-badges.now.sh/retroroyale/ClashRoyale)

#### A .NET Core 2017 Clash Royale Server (v1.9.0 - v1.9.2)
##### Need help? Join our [Discord](https://discord.gg/mUredE6CTU)
#### Want to help us? Fork this project and you could try add stuff!

#### SERVER DOWNLOAD: https://github.com/fdz6/AstralRoyale/releases/tag/Server
#### CLIENTS DOWNLOAD: https://github.com/fdz6/AstralRoyale/releases/tag/Clients
#### PORT FORWARD APP DOWNLOAD: https://github.com/fdz6/AstralRoyale/releases/tag/PortForward

## Update Logs (9/5/2025 - 8:19 PM):
```
1. Final fix to the profile arena.
2. Implemented a feature where if you have an outdated server version then you will be warned about having an outdated server which allows you to update it.
3. Fixed some bugs.
4. Invite Friends, Cancel Tournament Matchmake, Claim Achievements structure systems (not completed). 
```

## Features
```
1. More commands.
2. Searchable clans.
3. Maintenance system. (only enabled/disabled with the built in clan commands or the config.json file)
4. Clan Commands (including non admin/admin commands + added /admin & /ban command)
5. Update check if the app's version is incorrect.
6. Extended the 2v2 button timestamp to 2038, I did this so you can see the 2v2 button)
7. Username, clan name/description/chat filtering system.
8. Battle System
9. Friendly Battle
10. Shop
11. Upgrading
12. Arena map fixes.
```

## Partial Features
```
1. Sending Clan Mail
2. Achievements
```

## Incomplete Features
```
1. Classic Challenge & Grand Challenge
2. Reporting Users
3. Buying cards with gems
4. Battle Logs
```

## Battles
The server supports battles, for those a patched client is neccessary.

[See the wiki for a tutorials](https://github.com/fdz6/AstralRoyale/wiki/)

## How to start

#### Requirements:
  - [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
  - [UwAmp (includes phpMyAdmin & MySQL)](https://www.uwamp.com/en/?page=download)

```
If you're on Windows, the ClashRoyale folder has bat files to run the server.
If you're on Linux run the commands below.
```

###### Main Server:
```
cd /where/your/directory/you/put/ClashRoyale
dotnet publish "ClashRoyale.csproj" -c Release -o app
```

#### Run the server:

```
Run the port forward app and then set both local and external port to 9339.
Run UwAmp and start the server, open https://localhost/mysql then enter username and password as "root".
Make a new Database named "ardb" and create it then after click "ardb" and go to ClashRoyale folder then open GameAssets then drag "database.sql" into the Database page and it should import the sql file.
```

###### Main Server:
```dotnet app/ClashRoyale.dll```

```
When your server says that the configuration file has been added.
Find the config.json file (located in ClashRoyale folder) and open it.
Change the password to "root".
Change MinTrophies to 15 and change MaxTrophies to 50. You can change it to whatever you want.
Change DefaultGold, DefaultGems to any value but I recommend setting it to "100000000" if you want to progress faster.
Change DefaultLevel, PLEASE CHANGE THIS VALUE TO 1 - 13. DO NOT HAVE "DefaultLevel" SET TO 0 OR ELSE IT CRASHES THE CR APP.
Optional Step: You can change update_url to the download page of your website.

NOTE: THE APK & IPA STEPS ARE IN THE WIKI PAGE!
```

## Need help?
Contact me on Discord (fusiondevz) or open an issue.
