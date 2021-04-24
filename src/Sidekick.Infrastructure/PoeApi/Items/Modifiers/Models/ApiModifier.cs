namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models
{
    public class ApiModifier
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }

        public ApiModifierOptions Option { get; set; }
    }
}
