using System.Windows;
using System.Windows.Documents;

namespace Sidekick.Presentation.Wpf.Views.Prices.Helpers
{
    /// <summary>
    /// <para>TextPointerTranslator class.</para>
    /// <para>Notes:</para>
    /// <para>This class is used to translate TextPointers from and to text character offsets
    /// in FlowDocuments.</para>
    /// </summary>
    public class TextPointerTranslator
    {
        #region Construction
        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="document">The FlowDocument to translate TextPointers and character offsets.</param>
        public TextPointerTranslator(FlowDocument document)
        {
            this.Document = document;

            this.ResetToStart();
        }
        #endregion


        #region Fields
        /// <summary>
        /// Character index of start of current run
        /// </summary>
        int i_start;
        /// <summary>
        /// The navigator TextPointer used to search Document.
        /// </summary>
        TextPointer navigator;
        /// <summary>
        /// The Document's end-of-content TextPointer.
        /// </summary>
        TextPointer endOfDoc;

        #endregion


        #region Properties

        /// <summary>
        /// The FlowDocument being operated on.
        /// </summary>
        public FlowDocument Document { get; private set; }

        #endregion


        #region Public Methods

        /// <summary>
        /// <para>Gets a TextPointer to a text character offset in the FlowDocument.</para>
        /// <para>Notes:</para>
        /// <para>This method searches for next character-offset TextPointer in a forward direction.</para>
        /// <para>Search iterates through text blocks and determines if character offset is within current
        /// text block (instead of searching every character.)</para>
        /// <para>If CharOffset is beyond the Document, an end-of-document TextPointer is returned.</para>
        /// <para>Tabs in a Paragraph's Margin Left or TextIndent property are not counted
        /// as tab characters.</para>
        /// <para>Assumes newlines are "\r\n" characters.</para>
        /// </summary>
        /// <param name="CharOffset">The character offset in the text string.</param>
        /// <returns>The TextPointer if found, or an end-of-document TextPointer.</returns>
        public TextPointer GetTextPointer(int CharOffset)
        {
            return this.GetTextPointer(CharOffset, true);
        }

        /// <summary>
        /// <para>Gets a TextPointer to a text character offset in the FlowDocument.</para>
        /// <para>Notes:</para>
        /// <para>This method searches for next character-offset TextPointer in a forward direction.</para>
        /// <para>Search iterates through text blocks and determines if character offset is within current
        /// text block (instead of searching every character.)</para>
        /// <para>If CharOffset is beyond FlowDocument, an end-of-document TextPointer is returned.</para>
        /// <para>Tabs in a Paragraph's Margin Left or TextIndent property are not counted
        /// as tab characters.</para>
        /// <para>Assumes newlines are "\r\n" characters.</para>
        /// <para>Multiple calls can be made without reseting text position fields to speed up
        /// finding Textpointers.</para>
        /// <para>If one is operating in a forward order, one can use the obtained TextPointers to create
        /// TextRanges and call ApplyPropertyValue method without needing to reset text positions to start.</para>
        /// </summary>
        /// <param name="CharOffset">The text character offset in the FlowDocument.</param>
        /// <param name="ResetToStart">Whether or not to reset text positions to start of FlowDocument before 
        /// next search.</param>
        /// <returns>The TextPointer if found, or an end-of-document TextPointer.</returns>
        public TextPointer GetTextPointer(int CharOffset, bool ResetToStart)
        {
            #region Strategy
            // The goal is to count Text run lengths until CharOffset is within a Text run.
            // Characters also need to be added to the count for end of Paragraph and Linebreak 
            // (which in the text string is signified as "\r\n" characters) 
            //
            // The CharOffset is considered to be inside the run if the caret (textpointer) is positioned
            // before the first character of the run, or after the last character of the run (to
            // keep the textpointer within the run.)
            //
            // i_start: character index of start of current run
            // i_end: character index of end of current run 
            #endregion

            if (ResetToStart)
                this.ResetToStart();

            // loop until end of doc
            while (this.navigator.CompareTo(this.endOfDoc) < 0)
            {
                // switch pointer context
                switch (this.navigator.GetPointerContext(LogicalDirection.Forward))
                {
                    case TextPointerContext.ElementEnd:
                        {
                            DependencyObject d = this.navigator.GetAdjacentElement(LogicalDirection.Forward);
                            if (d is Paragraph || d is LineBreak)
                            {
                                // Add 2 for linebreak: "\r\n"
                                // Note: Text in a TextRange between to TextPointers inserts "\r\n" at 
                                // end of a Paragraph and end of LinkBreak 
                                this.i_start += 2;
                            }
                            break;
                        }
                    case TextPointerContext.Text:
                        {
                            // 0 1 2 3 4 5 6
                            // a b c d e X

                            // calc character index of end of current run
                            int i_end = this.i_start + this.navigator.GetTextRunLength(LogicalDirection.Forward);

                            // case: character offset is within current run
                            if (CharOffset <= i_end)
                            {
                                // calc offset within current run
                                int offset = CharOffset - this.i_start;

                                // return text pointer to character offset
                                return this.navigator.GetPositionAtOffset(offset, LogicalDirection.Forward);
                            }

                            // calc character index to next text run (if it exists)
                            this.i_start = i_end;
                            break;
                        }
                }
                // Advance the naviagtor to the next context position.
                this.navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
            }

            // case: counter index went past CharOffset and last paragraph and there is no more 
            // text runs to check for CharOffset: therefore, return end-of-doc text pointer
            if (this.i_start != 0 && CharOffset <= this.i_start)
            {
                return this.endOfDoc.GetNextInsertionPosition(LogicalDirection.Backward);
            }

            // default: not found
            return this.endOfDoc;
        }

        /// <summary>
        /// <para>Gets text character offset from a TextPointer in a FlowDocument.</para>
        /// <para>Notes:</para>
        /// <para>This method searches for the TextPointer in a forward direction.</para>
        /// <para>Search iterates through text blocks and determines if TextPointer is within current
        /// text block (instead of searching every character.)</para>
        /// <para>If TextPointer is beyond Document its text-length is returned.</para>
        /// <para>Tabs in a Paragraph's Margin Left or TextIndent property are not counted
        /// as tab characters.</para>
        /// <para>Assumes newlines are "\r\n" characters.</para>
        /// </summary>
        /// <param name="tp">The TextPointer to find the text character-offset to.</param>
        /// <returns>The text character-offset or Document text-length.</returns>
        public int GetCharacterOffset(TextPointer tp)
        {
            #region Strategy
            // The goal is to count Text run lengths until TextPointer is within a Text run.
            // Characters also need to be added to the count for end of Paragraph and Linebreak 
            // (which in the text string is signified as "\r\n" characters) 
            //
            // i_start: character index of start of current run
            #endregion

            this.ResetToStart();

            // loop until end of doc
            while (this.navigator.CompareTo(this.endOfDoc) < 0)
            {
                // switch pointer context
                switch (this.navigator.GetPointerContext(LogicalDirection.Forward))
                {
                    case TextPointerContext.ElementEnd:
                        {
                            DependencyObject d = this.navigator.GetAdjacentElement(LogicalDirection.Forward);
                            if (d is Paragraph || d is LineBreak)
                            {
                                #region Notes:
                                // When TextPointer is in an empty paragraph navigator won't get
                                // a Text context position. 
                                // It will get a Paragraph ElementStart, where navigator.CompareTo(tp) is -1
                                // Then it will get a Paragraph ElementEnd, where navigator.CompareTo(tp) is 1
                                // signifying it past the TextPointer.
                                // Same logic applies to LineBreak.
                                // It will get a LineBreak ElementStart, where navigator.CompareTo(tp) is 0
                                // Then it will get a LineBreak ElementEnd, where navigator.CompareTo(tp) is 1
                                // signifying it past the TextPointer.
                                // Therefore, when navigator has past TextPointer on ElementEnd for either Paragraph 
                                // or LineBreak, assign the current value of i_start.                     
                                #endregion

                                // case: TextPointer is in empty paragraph
                                if (this.navigator.CompareTo(tp) > 0)
                                {
                                    // return start of run index
                                    return i_start;
                                }

                                // Add 2 for linebreak: "\r\n"
                                // Note: Text in a TextRange between to TextPointers inserts "\r\n" at 
                                // end of a Paragraph and end of LinkBreak 
                                this.i_start += 2;
                            }
                            break; // switch
                        }
                    case TextPointerContext.Text:
                        {
                            // 0 1 2 3 4 5 6
                            // a b c d e X

                            // calc run length
                            int l_run = this.navigator.GetTextRunLength(LogicalDirection.Forward);
                            // calc TextPointer at end of current run
                            TextPointer tp_end = this.navigator.GetPositionAtOffset(l_run, LogicalDirection.Forward);

                            // get result comparing textpointers: -1: tp < tp_end, 0: tp = tp_end, 1: tp > tp_end
                            int result = tp.CompareTo(tp_end);
                            // case: text pointer is within current run
                            if (result <= 0)
                            {
                                // calc offset within current run
                                int o_run = this.navigator.GetOffsetToPosition(tp);

                                // calc character offset
                                int o_char = i_start + o_run;

                                // return text pointer to character offset
                                return o_char;
                            }

                            // calc character index to next text run (if it exists)
                            this.i_start += l_run;

                            break; // switch
                        }
                }
                // Advance the naviagtor to the next context position.
                this.navigator = navigator.GetNextContextPosition(LogicalDirection.Forward);
            }

            // default: not found return i_start: should only happen on empty doc
            return i_start;
        }


        #endregion


        #region Private Method

        /// <summary>
        /// <para>Resets the search position fields to start of document.</para>
        /// </summary>
        private void ResetToStart()
        {
            this.i_start = 0;
            this.endOfDoc = this.Document.ContentEnd;
            this.navigator = this.Document.ContentStart;
        }

        #endregion

    }
}
