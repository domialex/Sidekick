using System.ComponentModel.DataAnnotations;

namespace Sidekick.Database.Windows
{
    public class Window
    {
        [Key]
        public string Id { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }
    }
}
