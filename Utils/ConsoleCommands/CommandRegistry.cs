using System;
using System.Collections.Generic;
using System.Linq;

namespace GnosiaArchipelagoRandomizer.Utils.ConsoleCommands
{
    public static class CommandRegistry
    {
        private static readonly Dictionary<string, ConsoleCommand> Commands = new();

        public static void Register(ConsoleCommand command)
        {
            Commands[command.Keyword.ToLower()] = command;
        }

        public static bool TryExecute(string rawInput, out CommandResult result)
        {
            result = null;

            if (!rawInput.StartsWith("/"))
            {
                return false;
            }

            var split = rawInput.Substring(1).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 0)
            {
                result = CommandResult.Error("Command was empty.");
                return true;
            }

            var keyword = split[0].ToLower();
            var args = split.Skip(1).ToArray();

            if (!Commands.TryGetValue(keyword, out var command))
            {
                result = CommandResult.Error($"Unknown command: /{keyword}");
                return true;
            }

            if (args.Length < command.MinArgs)
            {
                result = CommandResult.Error(
                    $"Command /{keyword} requires at least {command.MinArgs} parameter(s).\nUsage: {command.Usage}");
                return true;
            }

            if (args.Length > command.MaxArgs)
            {
                result = CommandResult.Error(
                    $"Command /{keyword} accepts at most {command.MaxArgs} parameter(s).\nUsage: {command.Usage}");
                return true;
            }

            try
            {
                result = command.Execute(args);
            }
            catch (Exception ex)
            {
                result = CommandResult.Error(
                    $"Error executing command /{keyword}: {ex.Message}");
            }

            return true;
        }

        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return Commands.Values;
        }
    }
}