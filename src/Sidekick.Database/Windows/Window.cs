using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Sidekick.Database.Windows
{
    public class Window
    {
        internal static void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        [Key]
        public string Id { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
    }
}
