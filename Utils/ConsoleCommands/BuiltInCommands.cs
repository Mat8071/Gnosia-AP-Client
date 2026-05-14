using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GnosiaArchipelagoRandomizer.Archipelago;
using gnosia;
using UnityEngine;
using setting;

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

                    return enabled? CommandResult.Ok("DeathLink activated successfully.") : CommandResult.Ok("DeathLink deactivated successfully.");
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
        }
    }
}
