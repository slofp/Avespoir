# Change Log

Changes to this project will be written here.
If it changes slightly, the first digit of the decimal point moves.

If it changes significantly or very much, the first digit will move.

***

## [Beta 4.2](https://github.com/Fairy-Phy/Avespoir/tree/Beta-4.2) - 2021-05-30

### Added

* Update Checker

### Fixed

* Restart Method

### Changed

* How to execute the command

  delegate to abstruct

* Unification of command prefixes

  All @ and > are now unified as $

* Help Description

  Instead of writing in the Help method, we decided to write in units of commands

* Leveling System

  Added weights to the experience formula

## [Beta 4.1](https://github.com/Fairy-Phy/Avespoir/tree/Beta-4.1) - 2021-04-30

### Fixed

* API Rate Limit in AwaitMessage

### Changed

* AwaitMessage Process

* LiteDB Connection Type(Shared -> Direct)

* DSharpPlus Update v4.0.1

* Event Argument(Update adds DiscordClient)

## [Beta 4.0](https://github.com/Fairy-Phy/Avespoir/tree/Beta-4.0) - 2021-03-12

### Added

* **Docker Support**

* Database Methods

* DBConfig Key(Filename)

* Auto-generate Config file if it does not exist

* Language variable
  * ConfigLevelSwitchFalse
  * ConfigLevelSwitchTrue
  * RoleLevelDenyText
  * CommandNotImpl (Future?)

* LevelSystem Switching

* Config Command "level" arg

* Future use class
  * CommandAbstruct
  * LanguageDictionary

### Fixed

* Help Config args not write "leaveban"

* Change Status Null Error

* Level Up Embed Author bug

### Changed

* **Main Database (MongoDB -> LiteDB)**

* Readme Usage

* How to get the version (string -> Assembly version to string)

* logout Command (Command name: logout -> exit)

### Removed

* WindowsRepo configuration

* DBConfig Keys
  * UseDatabase
  * Url
  * Port
  * Username
  * Mechanism
  * MainDatabase

## [Beta 3.1](https://github.com/Fairy-Phy/Avespoir/tree/Beta-3.1) - 2020-09-10

### Added

* Language (en-US)

* Botowner Command (UserData Reset)

### Fixed

* Level stack bug

* User delete command

## [Beta 3.0](https://github.com/Fairy-Phy/Avespoir/tree/Beta-3.0) - 2020-08-31

### Added

* Database Schemes
  * AllowUsers and Roles: GuildID
  * GuildConfig

* Multi Language Support (This version ja-JP only)

* Server Configs
  * Whitelist Enable/Disable
  * LeaveBan Enable/Disable
  * Custom Public Prefix
  * Custom Moderator Prefix
  * LogChannel
  * Language (This version ja-JP only)

* Public Command: Roll (Long type random)

### Changed

* Repository Change: Organizations -> Personal

* Level System: Exp to First Sender -> First Sender and End Sender

* Database Process: Target Proparty -> GuildID and Target Proparty

* Database Scheme: LogChannel -> GuildConfig

* Level System DatabaseMethods -> Unified Database DatabaseMethods

### Removed

* Lunetrip Project(Experimental) (so hard system...)

## [Beta 2.1](https://github.com/Fairy-Phy/Avespoir/tree/Beta-2.1) - 2020-07-09

### Fixed

* Level System Bug

### Added

* Lunetrip Project(Experimental)

* Public Commands
  * Script Line(Experimental)
  * Script File(Experimental)

### Changed

* Level System Calculation(ExpScale 1.0x -> 0.5x)
* Level System Check NextMessage System -> Message Stack System

## [Beta 2.0](https://github.com/Fairy-Phy/Avespoir/tree/Beta-2.0) - 2020-05-19

### Fixed

* One-time Authentication bug
* DateTime Expection(?)

### Added

* Leveling System

* Public Commands
  * Create Invite url
  * User Status

### Changed

* self made logger -> log4net

## [Beta 1.2](https://github.com/Fairy-Phy/Avespoir/tree/Beta-1.2) - 2020-05-19

### Added

* One-time Authentication

* Moderator Commands
  * DBLogChannelAdd
  * DBLogChannelChange

### Removed

* Botowner Command: DBChannelAdd

## [Beta 1.1.1](https://github.com/Fairy-Phy/Avespoir/tree/Beta-1.1.1) - 2020-03-26

### Fixed

* Moderator Command Allow Botowner

## [Beta 1.1](https://github.com/Fairy-Phy/Avespoir/tree/Beta-1.1) - 2020-03-25

### Added

* Repository only build constitution
* Moderator command
  * DBUserChangeRole Command
* Bot status display allow users count

## [Beta 1.0](https://github.com/Fairy-Phy/Avespoir/tree/Beta-1.0) - 2020-03-24

Start Beta version!

### Added

* Avespoir installer(Incomplete)

### Changed

* Test Code -> Production specification

## [Alpha 4.0 (Final Update)](https://github.com/Fairy-Phy/Avespoir/tree/Alpha-4.0) - 2020-03-17

Alpha version final update.

### Added

* LogChannels schema
* Schemas BsonElement
* Role level
* Moderator command role check
* Botowner command id check
* Botownerid config
* Public Commands
  * Help command
  * Version command
* Moderator commands
  * DBUserAdd command
  * DBUserDelete command
  * DBUserList command
  * DBRoleAdd command
  * DBRoleDelete command
  * DBRoleList command
* Botowner commands
  * DBLogChannelAdd command
  * Restart command
  * Logout command
* ClientErroredEvent
* GuildMemberAddEvent
* GuildMemberRemoveEvent

### Changed

* Advance database connection
* Client constructer start bot -> method start
* ClientLog.StartClientLogEvents moved Client.Main method
* Async ReadyEvent and MessageEvent

### Removed

* Ready event get database data
* Client.Main args
* Test command(send check)

## [Alpha 3.0](https://github.com/Fairy-Phy/Avespoir/tree/Alpha-3.0) - 2020-03-11

### Added

* Command Attribute
* Moderator test Command
* Botowner test Command
* Original command execution program
* Array remove
* Random Code Generator

### Changed

* MessageLog DM support
* ReadyEvent Bot status

### Removed

* DSharpPlus.CommandNext

## [Alpha 2.2(3.2.2 commit)](https://github.com/Fairy-Phy/Avespoir/tree/Alpha-2.2) - 2020-03-03

The version name in the commit is incorrect.

### Added

* Use Database config
* Database login check
* Retry Database login
* Help Command(incomplete)
* GuildMemberAddEvent
* GuildMemberRemoveEvent
* ReadyEvent

### Changed

* Message.MessageEvent -> Events.MessageEvent
* Avoid deadlock(?)

## [Alpha 2.0](https://github.com/Fairy-Phy/Avespoir/tree/Alpha-2.0) - 2020-03-02

### Added

* MongoDB access
* #region description
* get MongoDB config by json file
* AllowUsers, Roles Schemas
* Exceptions
* Export Log file
* Find Command
* Emoji Command
* Emoji converter(:emoji: -> \<:emoji: emoji id>)

### Changed

* Newtonsoft.Json -> System.Text.Json
* new MainBot() Tasking
* Command class as partial class

### Fixed

* LoggerProperties.Username NullReferenceException issue

### Deprecated

* DSharpPlus.CommandNextModule RegisterCommands

## [Alpha 1.1](https://github.com/Fairy-Phy/Avespoir/tree/Alpha-1.1) - 2020-02-23

### Added

* AllowUnsafe -> true (Not Use...?)
* Moderator Command class
* Ping Command
* Message data info export console log

### Changed

* MessageCreatedEvent Code
* CommandModuleNext method move CommandRegister class
* MessageCreated -> MessageEvent

## [Alpha 1.0](https://github.com/Fairy-Phy/Avespoir/tree/Alpha-1.0) - 2020-02-21

### Added

* Connect discord Bot
* Config Setting by json file
* Console Log

## Project Start - 2020-02-20

### Added

* Repository created