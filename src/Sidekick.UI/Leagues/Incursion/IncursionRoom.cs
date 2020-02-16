namespace Sidekick.UI.Leagues.Incursion
{
    public class IncursionRoom
    {
        public IncursionRoom(string contains = "", string modifiers = "")
        {
            Contains = contains;
            Modifiers = modifiers;
        }

        public IncursionRoomTier Tier1 { get; set; }
        public IncursionRoomTier Tier2 { get; set; }
        public IncursionRoomTier Tier3 { get; set; }

        public string Contains { get; set; }
        public string ContainsTooltip { get; set; }

        public string Modifiers { get; set; }
    }
}
