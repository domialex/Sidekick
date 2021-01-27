namespace GregsStack.InputSimulatorStandard.Tests.UnicodeText
{
    using System.Text;

    public class UnicodeRange
    {
        public string Name { get; set; }

        public int Low { get; set; }

        public int High { get; set; }

        public string Characters
        {
            get
            {
                var i = this.Low;
                var sb = new StringBuilder(this.High - this.Low + 10);
                while (i <= this.High)
                {
                    sb.Append(char.ConvertFromUtf32(i));
                    i++;
                }

                return sb.ToString();
            }
        }

        public UnicodeRange(string name, int low, int high)
        {
            this.Name = name;
            this.Low = low;
            this.High = high;
        }
    }
}
