namespace GnosiaArchipelagoRandomizer.Utils.ConsoleCommands
{
    public class CommandResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public static CommandResult Ok(string msg)
        {
            return new CommandResult
            {
                Success = true,
                Message = msg
            };
        }

        public static CommandResult Error(string msg)
        {
            return new CommandResult
            {
                Success = false,
                Message = msg
            };
        }
    }
}