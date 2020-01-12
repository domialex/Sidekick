using Sidekick.Helpers;
using Sidekick.Helpers.POEPriceInfoAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sidekick.Windows.PricePrediction
{
    /// <summary>
    /// Interaction logic for PricePredictionView.xaml
    /// </summary>
    public partial class PricePredictionView : Window
    {
        public PricePredictionView(int width, int height)
        {
            Width = width;
            Height = height;
            InitializeComponent();
            Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        public bool IsDisplayed => Visibility == Visibility.Visible;

        public void SetResult(PriceInfo info)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetResultCallback(SetResult), new object[] { info });
            }
            else
            {
                if(!IsDisplayed)
                {
                    return;
                }

                textBoxMin.Text = Math.Round(info.Min, 2, MidpointRounding.AwayFromZero).ToString();
                textBoxMax.Text = Math.Round(info.Max, 2, MidpointRounding.AwayFromZero).ToString();
                labelCurrency.Content = info.Currency;
                labelPredictionScore.Content = info.PredictionConfidenceScore.ToString("00");
                textBoxItem.Document.Blocks.Clear();
                var lines = info.ItemText.Split(ItemParser.NEWLINE_SEPERATOR, StringSplitOptions.RemoveEmptyEntries);

                foreach(var line in lines)
                {
                    textBoxItem.Document.Blocks.Add(new Paragraph(new Run(line)));
                }
            }
        }
        delegate void SetResultCallback(PriceInfo info);

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
                DataContext = null;
                Visibility = Visibility.Hidden;
            }
        }
        delegate void HideWindowAndClearDataCallback();
    }
}
