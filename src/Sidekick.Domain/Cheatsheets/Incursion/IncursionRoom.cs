namespace Sidekick.Domain.Cheatsheets.Incursion
{
    public class IncursionRoom
    {
        public IncursionRoom(string contains = "", string modifiers = "")
        {
            Contains = contains;
            Modifiers = modifiers;
        }

        public IncursionRoomTier Level1 { get; set; }
        public IncursionRoomTier Level2 { get; set; }
        public IncursionRoomTier Level3 { get; set; }

        public string Contains { get; set; }
        public string ContainsTooltip { get; set; }

        public string Modifiers { get; set; }
    }
}
