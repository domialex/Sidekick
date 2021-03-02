namespace Sidekick.Domain.Game.Items
{
    /// <summary>
    /// Stores data about each line in the parsing process
    /// </summary>
    public class ParsingLine
    {
        /// <summary>
        /// Stores data about each line in the parsing process
        /// </summary>
        /// <param name="text">The line of the item description</param>
        public ParsingLine(int index,string text)
        {
            Index = index;
            Text = text;
        }

        /// <summary>
        /// Indicates if this line has been successfully parsed
        /// </summary>
        public bool Parsed { get; set; } = false;

        /// <summary>
        /// The index of the line inside the ParsingBlock
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The line of the item description
        /// </summary>
        public string Text { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
