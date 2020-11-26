namespace Sidekick.Presentation.Wpf.Views.MapInfo
{
    public class DangerousMapModModel
    {
        public DangerousMapModModel(string modifier = "", string color = "")
        {
            Modifier = modifier;
            Color = color;
        }

        public string Modifier { get; set; }
        public string Color { get; set; }
    }
}
