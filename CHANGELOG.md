# Change Log

Changes to this project will be written here.
If it changes slightly, the first digit of the decimal point moves.

If it changes significantly or very much, the first digit will move.

***

## [Beta 1.1.1](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Beta-1.1.1) - 2020-03-26

### Fixed

* Moderator Command Allow Botowner

## [Beta 1.1](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Beta-1.1) - 2020-03-25

### Added

* Repository only build constitution
* Moderator command
  * DBUserChangeRole Command
* Bot status display allow users count

## [Beta 1.0](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Beta-1.0) - 2020-03-24

Start Beta version!

### Added

* Avespoir installer(Incomplete)

### Changed

* Test Code -> Production specification

## [Alpha 4.0 (Final Update)](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Alpha-4.0) - 2020-03-17

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

## [Alpha 3.0](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Alpha-3.0) - 2020-03-11

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

## [Alpha 2.2(3.2.2 commit)](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Alpha-2.2) - 2020-03-03

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

## [Alpha 2.0](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Alpha-2.0) - 2020-03-02

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

## [Alpha 1.1](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Alpha-1.1) - 2020-02-23

### Added

* AllowUnsafe -> true (Not Use...?)
* Moderator Command class
* Ping Command
* Message data info export console log

### Changed

* MessageCreatedEvent Code
* CommandModuleNext method move CommandRegister class
* MessageCreated -> MessageEvent

## [Alpha 1.0](https://gitlab.com/Avespoir_Project/Avespoir/-/tree/Alpha-1.0) - 2020-02-21

### Added

* Connect discord Bot
* Config Setting by json file
* Console Log

## Project Start - 2020-02-20

### Added

* Repository created