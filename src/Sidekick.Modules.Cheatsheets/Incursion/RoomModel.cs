namespace Sidekick.Modules.Cheatsheets.Incursion
{
    public class RoomModel
    {
        public RoomModel(string contains, string modifiers)
        {
            Contains = contains;
            Modifiers = modifiers;
        }

        public RoomTierModel Level1 { get; set; }
        public RoomTierModel Level2 { get; set; }
        public RoomTierModel Level3 { get; set; }

        public string Contains { get; set; }
        public string Modifiers { get; set; }
        public string Tooltip { get; set; }
    }
}
