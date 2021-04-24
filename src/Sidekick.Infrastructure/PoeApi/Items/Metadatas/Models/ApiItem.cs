namespace Sidekick.Infrastructure.PoeApi.Items.Metadatas.Models
{
    public class ApiItem
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public ApiItemFlags Flags { get; set; } = new ApiItemFlags();
    }
}
