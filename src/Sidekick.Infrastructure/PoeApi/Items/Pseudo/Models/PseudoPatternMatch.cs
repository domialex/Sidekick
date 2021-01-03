namespace Sidekick.Infrastructure.PoeApi.Items.Pseudo.Models
{
    public class PseudoPatternMatch
    {
        public PseudoPatternMatch(string id, string type, string text)
        {
            Id = id;
            Type = type;
            Text = text;
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{Text} - {Id} ({Type})";
        }
    }
}
