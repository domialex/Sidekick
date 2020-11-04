using System.ComponentModel.DataAnnotations;

namespace Sidekick.Persistence.Windows
{
    public class Window
    {
        [Key]
        public string Id { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }
    }
}
