namespace GregsStack.InputSimulatorStandard.Tests.UnicodeText
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using HtmlAgilityPack;

    using Xunit;
    using Xunit.Abstractions;

    public class UnicodeTextTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public UnicodeTextTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public static IEnumerable<object[]> UnicodeTestCases = new[]
                       {
                           new []{new UnicodeRange("Basic Latin", 0x0020, 0x007F)},
                           new []{new UnicodeRange("Block Elements", 0x2580, 0x259F)},
                           new []{new UnicodeRange("Latin-1 Supplement", 0x00A0, 0x00FF)},
                           new []{new UnicodeRange("Geometric Shapes", 0x25A0, 0x25FF)},
                           new []{new UnicodeRange("Latin Extended-A", 0x0100, 0x017F)},
                           new []{new UnicodeRange("Miscellaneous Symbols", 0x2600, 0x26FF)},
                           new []{new UnicodeRange("Latin Extended-B", 0x0180, 0x024F)},
                           new []{new UnicodeRange("Dingbats", 0x2700, 0x27BF)},
                           new []{new UnicodeRange("IPA Extensions", 0x0250, 0x02AF)},
                           new []{new UnicodeRange("Miscellaneous Mathematical Symbols-A", 0x27C0, 0x27EF)},
                           new []{new UnicodeRange("Spacing Modifier Letters", 0x02B0, 0x02FF)},
                           new []{new UnicodeRange("Supplemental Arrows-A", 0x27F0, 0x27FF)},
                           new []{new UnicodeRange("Combining Diacritical Marks", 0x0300, 0x036F)},
                           new []{new UnicodeRange("Braille Patterns", 0x2800, 0x28FF)},
                           new []{new UnicodeRange("Greek and Coptic", 0x0370, 0x03FF)},
                           new []{new UnicodeRange("Supplemental Arrows-B", 0x2900, 0x297F)},
                           new []{new UnicodeRange("Cyrillic", 0x0400, 0x04FF)},
                           new []{new UnicodeRange("Miscellaneous Mathematical Symbols-B", 0x2980, 0x29FF)},
                           new []{new UnicodeRange("Cyrillic Supplementary", 0x0500, 0x052F)},
                           new []{new UnicodeRange("Supplemental Mathematical Operators", 0x2A00, 0x2AFF)},
                           new []{new UnicodeRange("Armenian", 0x0530, 0x058F)},
                           new []{new UnicodeRange("Miscellaneous Symbols and Arrows", 0x2B00, 0x2BFF)},
                           new []{new UnicodeRange("Hebrew", 0x0590, 0x05FF)},
                           new []{new UnicodeRange("CJK Radicals Supplement", 0x2E80, 0x2EFF)},
                           new []{new UnicodeRange("Arabic", 0x0600, 0x06FF)},
                           new []{new UnicodeRange("Kangxi Radicals", 0x2F00, 0x2FDF)},
                           new []{new UnicodeRange("Syriac", 0x0700, 0x074F)},
                           new []{new UnicodeRange("Ideographic Description Characters", 0x2FF0, 0x2FFF)},
                           new []{new UnicodeRange("Thaana", 0x0780, 0x07BF)},
                           new []{new UnicodeRange("CJK Symbols and Punctuation", 0x3000, 0x303F)},
                           new []{new UnicodeRange("Devanagari", 0x0900, 0x097F)},
                           new []{new UnicodeRange("Hiragana", 0x3040, 0x309F)},
                           new []{new UnicodeRange("Bengali", 0x0980, 0x09FF)},
                           new []{new UnicodeRange("Katakana", 0x30A0, 0x30FF)},
                           new []{new UnicodeRange("Gurmukhi", 0x0A00, 0x0A7F)},
                           new []{new UnicodeRange("Bopomofo", 0x3100, 0x312F)},
                           new []{new UnicodeRange("Gujarati", 0x0A80, 0x0AFF)},
                           new []{new UnicodeRange("Hangul Compatibility Jamo", 0x3130, 0x318F)},
                           new []{new UnicodeRange("Oriya", 0x0B00, 0x0B7F)},
                           new []{new UnicodeRange("Kanbun", 0x3190, 0x319F)},
                           new []{new UnicodeRange("Tamil", 0x0B80, 0x0BFF)},
                           new []{new UnicodeRange("Bopomofo Extended", 0x31A0, 0x31BF)},
                           new []{new UnicodeRange("Telugu", 0x0C00, 0x0C7F)},
                           new []{new UnicodeRange("Katakana Phonetic Extensions", 0x31F0, 0x31FF)},
                           new []{new UnicodeRange("Kannada", 0x0C80, 0x0CFF)},
                           new []{new UnicodeRange("Enclosed CJK Letters and Months", 0x3200, 0x32FF)},
                           new []{new UnicodeRange("Malayalam", 0x0D00, 0x0D7F)},
                           new []{new UnicodeRange("CJK Compatibility", 0x3300, 0x33FF)},
                           new []{new UnicodeRange("Sinhala", 0x0D80, 0x0DFF)},
                           new []{new UnicodeRange("CJK Unified Ideographs Extension A", 0x3400, 0x4DBF)},
                           new []{new UnicodeRange("Thai", 0x0E00, 0x0E7F)},
                           new []{new UnicodeRange("Yijing Hexagram Symbols", 0x4DC0, 0x4DFF)},
                           new []{new UnicodeRange("Lao", 0x0E80, 0x0EFF)},
                           new []{new UnicodeRange("CJK Unified Ideographs", 0x4E00, 0x9FFF)},
                           new []{new UnicodeRange("Tibetan", 0x0F00, 0x0FFF)},
                           new []{new UnicodeRange("Yi Syllables", 0xA000, 0xA48F)},
                           new []{new UnicodeRange("Myanmar", 0x1000, 0x109F)},
                           new []{new UnicodeRange("Yi Radicals", 0xA490, 0xA4CF)},
                           new []{new UnicodeRange("Georgian", 0x10A0, 0x10FF)},
                           new []{new UnicodeRange("Hangul Syllables", 0xAC00, 0xD7AF)},
                           new []{new UnicodeRange("Hangul Jamo", 0x1100, 0x11FF)},
                           new []{new UnicodeRange("High Surrogates", 0xD800, 0xDB7F)},
                           new []{new UnicodeRange("Ethiopic", 0x1200, 0x137F)},
                           new []{new UnicodeRange("High Private Use Surrogates", 0xDB80, 0xDBFF)},
                           new []{new UnicodeRange("Cherokee", 0x13A0, 0x13FF)},
                           new []{new UnicodeRange("Low Surrogates", 0xDC00, 0xDFFF)},
                           new []{new UnicodeRange("Unified Canadian Aboriginal Syllabics", 0x1400, 0x167F)},
                           new []{new UnicodeRange("Private Use Area", 0xE000, 0xF8FF)},
                           new []{new UnicodeRange("Ogham", 0x1680, 0x169F)},
                           new []{new UnicodeRange("CJK Compatibility Ideographs", 0xF900, 0xFAFF)},
                           new []{new UnicodeRange("Runic", 0x16A0, 0x16FF)},
                           new []{new UnicodeRange("Alphabetic Presentation Forms", 0xFB00, 0xFB4F)},
                           new []{new UnicodeRange("Tagalog", 0x1700, 0x171F)},
                           new []{new UnicodeRange("Arabic Presentation Forms-A", 0xFB50, 0xFDFF)},
                           new []{new UnicodeRange("Hanunoo", 0x1720, 0x173F)},
                           new []{new UnicodeRange("Variation Selectors", 0xFE00, 0xFE0F)},
                           new []{new UnicodeRange("Buhid", 0x1740, 0x175F)},
                           new []{new UnicodeRange("Combining Half Marks", 0xFE20, 0xFE2F)},
                           new []{new UnicodeRange("Tagbanwa", 0x1760, 0x177F)},
                           new []{new UnicodeRange("CJK Compatibility Forms", 0xFE30, 0xFE4F)},
                           new []{new UnicodeRange("Khmer", 0x1780, 0x17FF)},
                           new []{new UnicodeRange("Small Form Variants", 0xFE50, 0xFE6F)},
                           new []{new UnicodeRange("Mongolian", 0x1800, 0x18AF)},
                           new []{new UnicodeRange("Arabic Presentation Forms-B", 0xFE70, 0xFEFF)},
                           new []{new UnicodeRange("Limbu", 0x1900, 0x194F)},
                           new []{new UnicodeRange("Halfwidth and Fullwidth Forms", 0xFF00, 0xFFEF)},
                           new []{new UnicodeRange("Tai Le", 0x1950, 0x197F)},
                           new []{new UnicodeRange("Specials", 0xFFF0, 0xFFFF)},
                           new []{new UnicodeRange("Khmer Symbols", 0x19E0, 0x19FF)},
                           new []{new UnicodeRange("Linear B Syllabary", 0x10000, 0x1007F)},
                           new []{new UnicodeRange("Phonetic Extensions", 0x1D00, 0x1D7F)},
                           new []{new UnicodeRange("Linear B Ideograms", 0x10080, 0x100FF)},
                           new []{new UnicodeRange("Latin Extended Additional", 0x1E00, 0x1EFF)},
                           new []{new UnicodeRange("Aegean Numbers", 0x10100, 0x1013F)},
                           new []{new UnicodeRange("Greek Extended", 0x1F00, 0x1FFF)},
                           new []{new UnicodeRange("Old Italic", 0x10300, 0x1032F)},
                           new []{new UnicodeRange("General Punctuation", 0x2000, 0x206F)},
                           new []{new UnicodeRange("Gothic", 0x10330, 0x1034F)},
                           new []{new UnicodeRange("Superscripts and Subscripts", 0x2070, 0x209F)},
                           new []{new UnicodeRange("Ugaritic", 0x10380, 0x1039F)}
                       };

        [Theory(Skip = "Run only interactive")]
        [MemberData(nameof(UnicodeTestCases))]
        public void TestUnicodeRanges(UnicodeRange range)
        {
            // ReSharper disable AccessToDisposedClosure
            using (var form = new UnicodeTestForm
            {
                Expected = range.Characters
            })
            {
                var ready = false;
                var formTask = Task.Factory.StartNew(
                    () =>
                        {
                            form.Shown += (x, y) => ready = true;
                            form.ShowDialog();
                        }, TaskCreationOptions.LongRunning);

                var simTask = Task.Factory.StartNew(
                    () =>
                        {
                            while (!ready)
                            {
                                Thread.Sleep(250);
                            }

                            var sim = new InputSimulator();
                            sim.Keyboard.TextEntry(range.Characters);
                            while (form.Received != form.Expected)
                            {
                                Thread.Sleep(500);
                            }

                            form.Close();
                        }, TaskCreationOptions.LongRunning);

                Task.WaitAll(new[] { formTask, simTask }, TimeSpan.FromSeconds(60));
                Assert.Equal(form.Expected, form.Received);
            }
            // ReSharper restore AccessToDisposedClosure
        }

        [Fact(Skip = "Generate Unicode test data manually")]
        public void GetCharacterRanges()
        {
            using (var client = new WebClient())
            {
                var html = client.DownloadString("http://jrgraphix.net/r/Unicode/");
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                foreach (var link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    var a = link.GetAttributeValue("href", "unknown");
                    if (!a.Contains("Unicode"))
                    {
                        continue;
                    }

                    a = "0x" + a.Replace("/r/Unicode/", "").Replace("-", ", 0x");
                    this.testOutputHelper.WriteLine($"new []{{new UnicodeRange(\"{link.InnerText}\", {a})}},");
                }
            }
        }
    }
}
