using System;
using System.Collections.Generic;
using System.Linq;
using GnosiaArchipelagoRandomizer.Archipelago;
using GnosiaArchipelagoRandomizer.Patches.Optional;
using HarmonyLib;
using setting;
using UnityEngine;

namespace GnosiaArchipelagoRandomizer.Utils.ConsoleCommands
{
    public static class BuiltInCommands
    {
        private static readonly Dictionary<string, int> Characters = new()
        {
            { "player", 0 },
            { "gina", 1 },
            { "sq", 2 },
            { "raqio", 3 },
            { "stella", 4 },
            { "shigemichi", 5 },
            { "chipie", 6 },
            { "remnan", 7 },
            { "comet", 8 },
            { "yuriko", 9 },
            { "jonas", 10 },
            { "setsu", 11 },
            { "otome", 12 },
            { "sha-ming", 13 },
            { "kukrushka", 14 },
        };

        private static readonly Dictionary<string, Setting.Yakuwari> Roles = new()
        {
            { "engineer", Setting.Yakuwari.y_Uranai },
            { "doctor", Setting.Yakuwari.y_Reibai },
            { "ga", Setting.Yakuwari.y_Kari },
            { "gd", Setting.Yakuwari.y_Lover },
            { "crew", Setting.Yakuwari.y_Murabito },
            { "ac", Setting.Yakuwari.y_Kyojin },
            { "gnosia", Setting.Yakuwari.y_Jinro },
            { "bug", Setting.Yakuwari.y_Fox },
        };
        public static void RegisterAll()
        {
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "help",
                Description = "Lists all commands.",
                Usage = "/help",
                MinArgs = 0,
                MaxArgs = 0,

                Execute = args =>
                {
                    var commands = CommandRegistry.GetCommands();

                    var text = string.Join("\n",
                        commands.Select(c => $"{c.Usage} - {c.Description}"));

                    return CommandResult.Ok(text);
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "deathlink",
                Description = "Activates or deactivates deathlink.",
                Usage = "/deathlink <on|off>",
                MinArgs = 1,
                MaxArgs = 1,

                Execute = args =>
                {
                    var value = args[0].ToLower();

                    if (value != "on" && value != "off")
                    {
                        return CommandResult.Error("Parameter must be either 'on' or 'off'.");
                    }

                    bool enabled = value == "on";

                    if (Plugin.ArchipelagoClient.GetDeathLinkHandler().IsDeathLinkEnabled() && enabled)
                    {
                        return CommandResult.Error("DeathLink is already enabled.");
                    }

                    if (!Plugin.ArchipelagoClient.GetDeathLinkHandler().IsDeathLinkEnabled() && !enabled)
                    {
                        return CommandResult.Error("DeathLink is already disabled.");
                    }

                    Plugin.ArchipelagoClient.GetDeathLinkHandler().ToggleDeathLink();

                    return enabled ? CommandResult.Ok("DeathLink activated successfully.") : CommandResult.Ok("DeathLink deactivated successfully.");
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "debug",
                Description = "Activates or deactivates debug mode.",
                Usage = "/debug <on|off>",
                MinArgs = 1,
                MaxArgs = 1,

                Execute = args =>
                {
                    var value = args[0].ToLower();

                    if (value != "on" && value != "off")
                    {
                        return CommandResult.Error("Parameter must be either 'on' or 'off'.");
                    }

                    bool enabled = value == "on";

                    Plugin.debug_mode = enabled;

                    return enabled ? CommandResult.Ok("Debug Mode activated successfully.") : CommandResult.Ok("Debug Mode deactivated successfully.");
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "block_mkb",
                Description = "Turning this on blocks mouse inputs to the game unless something is clicked directly and tries to block keyboard inputs when typing in the console. Makes skipping text with mouse impossible while active.",
                Usage = "/block_mkb <on|off>",
                MinArgs = 1,
                MaxArgs = 1,

                Execute = args =>
                {
                    var value = args[0].ToLower();

                    if (value != "on" && value != "off")
                    {
                        return CommandResult.Error("Parameter must be either 'on' or 'off'.");
                    }

                    bool enabled = value == "on";

                    ArchipelagoConsole.blockMouseAndKeyboard = enabled;

                    return enabled ? CommandResult.Ok("Mouse & Keyboard block activated successfully.") : CommandResult.Ok("Mouse & Keyboard block deactivated successfully.");
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "try_force_event",
                Description = "Guarantees the event will trigger as long as the conditions to trigger it are satisfied",
                Usage = "/try_force_event <Event ID>",
                MinArgs = 1,
                MaxArgs = 1,

                Execute = args =>
                {
                    ulong id = Convert.ToUInt64(args[0]);

                    if (id >= 200)
                    {
                        return CommandResult.Error("Event IDs can only go up to 199");
                    }

                    gnosia.GameData gd = null;

                    try
                    {
                        gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                    }
                    catch
                    {
                        return CommandResult.Error("You must load a save file (and be in the Setup screen) to use this command");
                    }

                    if (gd.baseData.state != 0)
                    {
                        return CommandResult.Error("You must be in the Setup screen to use this command.");
                    }

                    for (int i = 0; i < gd.baseData.s_chara_resource.Length; i++)
                    {
                        gd.baseData.s_chara_resource[i] = 255;
                    }

                    gd.baseData.sce_flg |= (id << 56);

                    return CommandResult.Ok("Command executed successfully.");
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "force_role",
                Description = "forces a character to have a certain role next loop",
                Usage = "/force_role <Character> <Role>",
                MinArgs = 2,
                MaxArgs = 2,

                Execute = args =>
                {
                    var character = args[0].ToLower();
                    var role = args[1].ToLower();

                    if (!Characters.ContainsKey(character))
                    {
                        return CommandResult.Error($"Unknown Character '{character}'.");
                    }

                    if (!Roles.ContainsKey(role))
                    {
                        return CommandResult.Error($"Unknown Role '{role}'.");
                    }

                    gnosia.GameData gd = null;

                    try
                    {
                        gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                    }
                    catch
                    {
                        return CommandResult.Error("You must load a save file (and be in the Setup screen) to use this command");
                    }

                    if (gd.baseData.state != 0)
                    {
                        return CommandResult.Error("You must be in the Setup screen to use this command.");
                    }

                    gd.charaYakuList[Characters[character]] = Roles[role];

                    return CommandResult.Ok("Command executed successfully.");
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "force_chara",
                Description = "Forces a character to be present in the next loop.",
                Usage = "/force_chara <Characters...>",
                MinArgs = 1,
                MaxArgs = 14,

                Execute = args =>
                {
                    foreach (string character in args)
                    {
                        if (!Characters.ContainsKey(character.ToLower()))
                        {
                            return CommandResult.Error($"Unknown Character '{character}'.");
                        }
                    }

                    gnosia.GameData gd = null;

                    try
                    {
                        gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                    }
                    catch
                    {
                        return CommandResult.Error("You must load a save file (and be in the Setup screen) to use this command");
                    }

                    if (gd.baseData.state != 0)
                    {
                        return CommandResult.Error("You must be in the Setup screen to use this command.");
                    }

                    foreach (string character in args)
                    {
                        gd.charaUseList.Add(Characters[character.ToLower()]);
                    }

                    return CommandResult.Ok("Command executed successfully.");
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "reset_scenario",
                Description = "Resets all of an event's flags to their initial state",
                Usage = "/reset_scenario <Event ID>",
                MinArgs = 1,
                MaxArgs = 1,

                Execute = args =>
                {
                    ulong id = Convert.ToUInt64(args[0]);

                    if (id >= 200)
                    {
                        return CommandResult.Error("Event IDs can only go up to 199");
                    }

                    gnosia.GameData gd = null;

                    try
                    {
                        gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                    }
                    catch
                    {
                        return CommandResult.Error("You must load a save file to use this command");
                    }

                    gd.baseData.sce_ind_flg[id] = 0;

                    return CommandResult.Ok("Command executed successfully.");
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "get_stats",
                Description = "Prints the given character's current stats",
                Usage = "/get_stats <Character>",
                MinArgs = 1,
                MaxArgs = 1,

                Execute = args =>
                {
                    var character = args[0].ToLower();

                    if (!Characters.ContainsKey(character))
                    {
                        return CommandResult.Error($"Unknown Character '{character}'");
                    }

                    gnosia.GameData gd = null;

                    try
                    {
                        gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                    }
                    catch
                    {
                        return CommandResult.Error("You must load a save file to use this command");
                    }

                    string text = "";

                    foreach (Setting.E_abil abil in Enum.GetValues(typeof(Setting.E_abil)))
                    {
                        text += $"{Setting.AbilNames[(int)abil]}: {(float)Mathf.RoundToInt(gd.GetAbil(gd.personFromId[Characters[character]], abil, false) * 100) / 2f}\n";
                    }

                    return CommandResult.Ok(text);
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "rename",
                Description = "renames one of the characters of the game (this save file only)",
                Usage = "/rename <Character> <New Name>",
                MinArgs = 2,
                MaxArgs = 2,

                Execute = args =>
                {
                    var character = args[0].ToLower();
                    var newName = args[1];

                    if (!Characters.ContainsKey(character))
                    {
                        return CommandResult.Error($"Unknown Character '{character}'.");
                    }

                    gnosia.GameData gd = null;

                    try
                    {
                        gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                    }
                    catch
                    {
                        return CommandResult.Error("You must load a save file (and be in the Setup screen) to use this command");
                    }

                    if (gd.baseData.state != 0)
                    {
                        return CommandResult.Error("You must be in the Setup screen to use this command.");
                    }

                    //Get internal stuff
                    Type dataType = AccessTools.TypeByName("gnosia.Data");
                    Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);

                    object entry = chara.GetValue(Characters[character]);
                    AccessTools.Field(entry.GetType(), "name").SetValue(entry, newName);
                    chara.SetValue(entry, Characters[character]);

                    return CommandResult.Ok("Command executed successfully.");
                }
            });
            CommandRegistry.Register(new ConsoleCommand
            {
                Keyword = "goal_info",
                Description = "Lists all goal-related information.",
                Usage = "/goal_info",
                MinArgs = 0,
                MaxArgs = 0,

                Execute = args =>
                {
                    //Get options and Goal option
                    var options = ArchipelagoClient.ServerData.SlotData.Options;

                    if (options.Goal == null)
                    {
                        return CommandResult.Error("The 'Goal' yaml option wasn't found in SlotData");
                    }

                    if (options.RequiredNotePercent == null)
                    {
                        return CommandResult.Error("The 'Required Note Percent' yaml option wasn't found in SlotData");
                    }

                    ArchipelagoData.Goal? goal = options?.Goal;
                    float requiredNotePercent = (float)(options?.RequiredNotePercent ?? 80);

                    string goalName = "";
                    string goalData = "";
                    switch (goal)
                    {
                        case ArchipelagoData.Goal.NormalEnding:
                            goalName = "Normal Ending";
                            //Get gd
                            gnosia.GameData gd = GameObject.Find("Application/GameLogManager/SaveDataManager").GetComponent<gnosia.GameData>();
                            //Get internal stuff
                            Type dataType = AccessTools.TypeByName("gnosia.Data");
                            Array chara = (Array)AccessTools.Field(dataType, "Chara").GetValue(null);
                            //Calculate progress
                            int foundNotes = 0;
                            int totalNotes = 0;
                            for (int i = 1; i < 15; i++)
                            {
                                byte notes = MyUtils.GetCharaTotalNotes(chara, i);
                                totalNotes += notes;
                                for (int j = 0; j < notes; j++)
                                {
                                    if ((gd.baseData.s_chara_all_flg[i] & (1UL << j)) > 0UL)
                                    {
                                        foundNotes += 1;
                                    }
                                }
                            }
                            int requiredNotes = (int)(totalNotes * (requiredNotePercent / 100f));
                            goalData = $"Found Notes: {foundNotes}/{requiredNotes}/{totalNotes} (found/required/total)\n";
                            break;
                        case ArchipelagoData.Goal.RoleAchievements:
                            goalName = "Role Achievements";
                            var completedAchievementNames = HandleAchievementsPatch.completedAchievements
                                .Where(id => HandleAchievementsPatch.roleAchievementIdToName.ContainsKey(id))
                                .Select(id => HandleAchievementsPatch.roleAchievementIdToName[id])
                                .ToList();

                            HashSet<string> excludedAchievementNames = new HashSet<string>(
                                (options?.ExcludedAchievements ?? Enumerable.Empty<string>())
                                .Concat(options?.ExcludeLocations ?? Enumerable.Empty<string>())
                                .Select(name => name.Substring(0, name.Length - " Achievement".Length))
                            );

                            HashSet<string> neededAchievementNames = new HashSet<string>(
                                HandleAchievementsPatch.allRoleAchievements
                                .Where(id => HandleAchievementsPatch.roleAchievementIdToName.ContainsKey(id))
                                .Select(id => HandleAchievementsPatch.roleAchievementIdToName[id])
                                .Except(excludedAchievementNames)
                            );

                            goalData = "Achievements Needed to Goal: " +
                            string.Join(", ", neededAchievementNames) +
                            "\nCompleted Achievements: " +
                                (completedAchievementNames.Count > 0
                                    ? string.Join(", ", completedAchievementNames)
                                    : "None");
                            break;
                        case null:
                            return CommandResult.Error("Error: Goal option not found in SlotData");
                        default:
                            return CommandResult.Error("Error: Unknown Goal option");
                    }

                    //Actually list goal info
                    string text = $"Goal: {goalName}\n{goalData}";

                    return CommandResult.Ok(text);
                }
            });
        }
    }
}
