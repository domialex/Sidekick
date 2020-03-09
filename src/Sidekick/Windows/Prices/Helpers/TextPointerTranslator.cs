using System;
using System.Windows.Documents;

namespace Sidekick.Windows.Prices.Helpers
{
    public static class FlowHelper
    {
        /// <summary>
        /// <para>Gets a TextRange from a FlowDocument by text character offset and length.</para>
        /// <para>Notes:</para>
        /// <para>Uses TextPointerTranslator to find TextPointers from character offset and length.</para>
        /// <para>Returns TextRange if successful, null if not.</para>
        /// </summary>
        /// <param name="document">The FlowDocument to find the TextRange in.</param>
        /// <param name="CharOffset">The text character offset.</param>
        /// <param name="Length">The text length of the TextRange.</param>
        /// <returns></returns>
        public static TextRange GetTextRange(this FlowDocument document, int CharOffset, int Length)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            TextPointerTranslator sel = new TextPointerTranslator(document);

            TextPointer start = sel.GetTextPointer(CharOffset, false);
            TextPointer end = sel.GetTextPointer(CharOffset + Length, false);

            TextRange range = null;
            if (start != null && end != null)
            {
                range = new TextRange(start, end);
            }
            return range;
        }
    }
}
