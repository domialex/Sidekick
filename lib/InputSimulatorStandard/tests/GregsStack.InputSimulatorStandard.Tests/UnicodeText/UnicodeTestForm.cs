namespace GregsStack.InputSimulatorStandard.Tests.UnicodeText
{
    using System;
    using System.Windows.Forms;

    public partial class UnicodeTestForm : Form
    {
        public UnicodeTestForm()
        {
            this.InitializeComponent();
        }

        public string Expected
        {
            get => this.ExpectedTextBox.Text;
            set => this.ExpectedTextBox.Text = value;
        }

        public string Received => this.RecievedTextBox.Text;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.RecievedTextBox.Focus();
        }
    }
}
