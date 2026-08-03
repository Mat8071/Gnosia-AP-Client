# Gnosia Archipelago Randomizer
This is a mod that aims to make the SinglePlayer social deduction game "[Gnosia](https://store.steampowered.com/app/1608290/GNOSIA/)" compatible with the [Archipelago Multi-Game Randomizer](https://archipelago.gg/).
## Goals
- Normal Ending

(More goals will be added in the future)
## Items
These are the items you may receive from other players in the MultiWorld or find randomly in your own world:
- Character Notes & Player Skills
- Roles
- Progressive Crew Max / Characters (depending on settings)
- Filler: EXP bonus at the end of the loop
## Locations
These are the objectives you need to complete to find a MultiWorld item:
- Meeting a character for the first time
- Learning information about a character (Where normally you'd get a character note)
- Seeing an event that normally gives a skill
- Having a character explain a role to you during the tutorial loops
- Surviving the loop where your role is secretly set to Bug (this does not require having the Bug Role)
## Other Features
- DeathLink

(More features may be added in the future)
## Changes From Base Game
On top of the obvious randomizer changes, there are a couple of additional modifications that were made to make the mod more enjoyable to play:
- When character unlocks are randomized, it's possible to have a loop without Setsu (this never happens in vanilla)
- Reaching the Normal Ending now requires only a percentage of the total notes, defined in the yaml settings for your slot (You can still set it to 100% if you want)

More QOL changes will be added in later versions (Making events more common is one of the planned changes)
## Future Plans
- Less wait between events (at least as an option) & Better Event Search
- Character Stats & Personality randomizer
- (Maybe) Character skills and skill requirements randomizer
- More items and locations
- More Goals
- Traps! (Give me suggestions for interesting traps)
- Better UI / tracker (??? Very far future)
## How to install
- First of all, install the [AP World](https://github.com/Mat8071/Gnosia-Archipelago-Randomizer/releases) and make a yaml using the [Options Creator](https://archipelago.gg/tutorial/Archipelago/other_en#options-creator) or a similar tool.
- Then, install [BepInEx](https://github.com/BepInEx/BepInEx/releases) in your game's installation folder.
- Run the game once with BepInEx installed.
- Download the [Client](https://github.com/Mat8071/Gnosia-AP-Client/releases).
- Extract the .zip into the BepInEx/plugins folder. Make sure there's only one /GnosiaArchipelagoRandomizer/ folder, not two. (the final structure should look like this: GNOSIA/BepInEx/plugins/GnosiaArchipelagoRandomizer/GnosiaArchipelagoRandomizer.dll).

After this, you just need to generate a world/multiworld, host it and connect to it with the client.
Know that the mod creates a new save folder for each seed it connects to. If you plan on playing many runs, you may want to clean the save folder from time to time.
## Questions and Troubleshooting
If you have any problems or questions on how something works, you may find an answer here
### How is save data handled? Should I backup my saves before trying the mod?
Upon connecting, the mod should create a new save folder based on the seed of the room you're connecting to. To access your vanilla saves, just remove the mod. (If you care about your saves a lot, a backup wouldn't hurt though)
### Can I load previous saves or start a new save file?
Yes. Absolutely. If anything breaks as a result of this, please report it as a bug.
### How do I know an event is in logic?
Unfortunately, this mod does not feature an in-game tracker (yet). In the meantime, you can use [Universal Tracker](https://archipelago.gg/tutorial/Archipelago/other_en#universal-tracker) to check logic and track your progress. Also, keep in mind some locations may be completable out of logic under certain circumstances (but it's never required).

In these cases the location will usually be marked as reachable with "glitched" logic in UT (this is just the term UT uses. "Glitched" logic does not actually require the use of glitches for this game).
### I'm in "go mode". What do I need to do to goal?
Here's what you need to do to goal with the currently implemented goals:
#### Normal Ending
To get the Normal Ending, you need to start a loop with zero Gnosia (and obviously have the right amount of notes defined by your yaml options).

To be able to set gnosia to zero, you need to unlock the ability to do so like in vanilla, by completing "The Final Problem" Event and subsequently winning a loop with Setsu. If you are in go mode, you can use `/get_logical_path The Final Problem` to know exactly which events you need to complete and in which order, in case you haven't done that yet.
### An event is in logic but it hasn't happened for many loops
This is not necessarily a logic error. It may be that the event requires specific character/role combinations or that the characters needed were involved in too many events recently.
Before you report this as a logic bug, try doing the following:
- open the in-game console and type `/try_force_event 0` (Even better if you know the event id, and use that instead of `0`. You can find a list of event ids [here](Docs/EventIDs.txt)). This resets each character to be very likely to show up in any event
- If the event requires a certain character/role combo, type `/force_role {Character} {Role}`. Eg: `/force_role Shigemichi Engineer`. Guardian Angel, Guard Duty and AC Follower are abbreviated as GA, GD and AC.
- If the event requires some characters to be present, type `/force_chara {Character1} {Character2} {...}` (even if you already forced their role)

If the event still does not trigger after a couple of loops, please report this as a logic bug.
### When I try to use the console, the game advances on its own
This is a known issue. You can "fix" it by typing `/block_mkb on` in the console. This will, however, make it impossible to advance events by clicking with the mouse. To deactivate this, type `/block_mkb off` in the console.
### What items should I hint for if I'm stuck?
On top of the obvious items (Roles, and Characters / Progressive Crew Max), there are a few items that are more important than they might look at first:
1. Bug Role (Around half of the events are secretly locked by unlocking it and it's hard-required for both respeccing and reaching the game's ending)
2. Guardian Angel Role (Required for one of the Raqio quizzes, which are part of the longest event chain, resulting in gaining access to A World Without Gnosia and the Normal Ending. Also, without it it's more difficult for Engineers and Doctors to reveal their role)
3. Setsu Note 2 (Required, with the Bug Role, to unlock Event Search if you want to use it)
### The game froze and I can't do anything!
If this happens, please report the bug so I can fix it. In the meantime, you can probably close the game, reopen and reconnect and load a previous file.
#### If the freeze only happens during a particular event:
Avoid the event that froze the game and if necessary, use `/send_location` in the host console (outside the game) to complete locations inside it.
If you can't avoid the event that froze the game, try restarting from setup.
#### If the freeze happened after receiving a DeathLink:
If the game does not freeze often and you care about deathlink you can try keeping it on. Otherwise, you can disable deathlink by typing `/deathlink off` in the in-game console. Even if you don't care about the freezes, please report the bug anyway!
### An event doesn't appear anymore after I received a DeathLink during it!
First of all, try the steps for "An event is in logic but hasn't happened for many loops" as that may fix the problem.
If that doesn't fix it, report the bug, get the ID of the event that doesn't appear anymore [here](Docs/EventIDs.txt) and type in the console `/reset_scenario {ID}`.
### Can I toggle DeathLink mid-run?
Yes. Just open the in-game console and type `/deathlink on` to activate it and `/deathlink off` to deactivate it.
