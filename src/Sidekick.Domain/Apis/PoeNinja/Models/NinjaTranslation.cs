namespace Sidekick.Domain.Apis.PoeNinja.Models
{
    /// <summary>
    /// Contains translation data from the Poe ninja api
    /// </summary>
    public class NinjaTranslation
    {
        /// <summary>
        /// The english value
        /// </summary>
        public string English { get; set; }

        /// <summary>
        /// The value of the translation
        /// </summary>
        public string Translation { get; set; }
    }
}
