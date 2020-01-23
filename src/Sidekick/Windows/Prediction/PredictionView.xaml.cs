using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using Sidekick.Business.Apis.PoePriceInfo.Models;

namespace Sidekick.Windows.Prediction
{
    /// <summary>
    /// Interaction logic for PredictionView.xaml
    /// </summary>
    public partial class PredictionView : Window
    {
        private PriceInfoResult priceInfo;
        public readonly string[] PROPERTY_SEPERATOR = new string[] { "--------" };
        public readonly string[] NEWLINE_SEPERATOR = new string[] { Environment.NewLine };

        public PredictionView(int width, int height)
        {
            Width = width;
            Height = height;
            InitializeComponent();
            Hide();
        }

        public bool IsDisplayed => Visibility == Visibility.Visible;

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        public void SetPriceInfoResult(PriceInfoResult info)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetPriceInfoResultCallback(SetPriceInfoResult), new object[] { info });
            }
            else
            {
                if (!IsDisplayed)
                {
                    return;
                }

                this.priceInfo = info;
                var itemParagraph = new Paragraph();
                //var lines = info.ItemText.Split(NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);

                //foreach(var line in lines)
                //{
                //    itemParagraph.Inlines.Add(new Run(line + "\n"));
                //}

                //textBoxItemStats.Document = new FlowDocument(itemParagraph);
                var priceText = Math.Round(info.Min.Value, 2) + " ~ " + Math.Round(info.Max.Value, 2) + " " + info.Currency;
                labelPriceRange.Content = priceText;
                var predictionText = Math.Round(info.ConfidenceScore, 2) + " %";
                labelConfidence.Content = predictionText;
                var warningParagraph = new Paragraph();
                warningParagraph.Inlines.Add(info.WarningMessage);
                textBoxWarningMessage.Document = new FlowDocument(warningParagraph);
            }
        }

        delegate void SetPriceInfoResultCallback(PriceInfoResult info);

        public void SetWindowPosition(int x, int y)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetWindowPositionCallback(SetWindowPosition), new object[] { x, y });
            }
            else
            {
                Left = x;
                Top = y;
            }
        }

        delegate void SetWindowPositionCallback(int x, int y);

        public void ShowWindow()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ShowWindowCallback(ShowWindow));
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }

        delegate void ShowWindowCallback();

        public void HideWindowAndClearData()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new HideWindowAndClearDataCallback(HideWindowAndClearData));
            }
            else
            {
                priceInfo = null;
                textBoxItemStats.Document.Blocks.Clear();
                textBoxWarningMessage.Document.Blocks.Clear();
                labelConfidence.Content = "";
                labelPriceRange.Content = "";
                Visibility = Visibility.Hidden;
            }
        }

        delegate void HideWindowAndClearDataCallback();
    }
}
