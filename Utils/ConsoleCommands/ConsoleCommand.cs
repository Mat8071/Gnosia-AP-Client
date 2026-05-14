using System;
using System.Collections.Generic;

namespace GnosiaArchipelagoRandomizer.Utils.ConsoleCommands
{
    public class ConsoleCommand
    {
        public string Keyword { get; set; }

        public string Description { get; set; }

        public string Usage { get; set; }

        public int MinArgs { get; set; }

        public int MaxArgs { get; set; }

        public Func<string[], CommandResult> Execute { get; set; }
    }
}