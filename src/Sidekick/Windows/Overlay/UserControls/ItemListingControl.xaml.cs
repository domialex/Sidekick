using Sidekick.Helpers.POETradeAPI.Models;
using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sidekick.Windows.Overlay.UserControls
{
    public partial class ItemListingControl : UserControl
    {
        public ItemListingControl()
        {

        }

        public ItemListingControl(ListingResult listingResult)
        {
            InitializeComponent();

            accountNameTextbox.Text = listingResult.Listing.Account.Name;
            characterNameTextbox.Text = listingResult.Listing.Account.LastCharacterName;
            priceTextbox.Text = listingResult.Listing.Price?.Amount + " " + listingResult.Listing.Price?.Currency;
            itemLevelTextbox.Text = listingResult.Item.Ilvl.ToString();
            ageTextbox.Text = GetHumanReadableTimeSpan(listingResult.Listing.Indexed);
            if (listingResult.Item.Corrupted)
            {
                priceTextbox.Foreground = new SolidColorBrush(Color.FromRgb(210, 0, 0));
            }
        }

        private string GetHumanReadableTimeSpan(DateTime time)
        {
            var span = DateTime.Now - time.ToLocalTime();

            if (span.Days > 1) return $"{span.Days} days";
            if (span.Days == 1) return $"{span.Days} day";
            if (span.Hours > 1) return $"{span.Hours} hours";
            if (span.Hours == 1) return $"{span.Hours} hour";
            if (span.Minutes > 1) return $"{span.Minutes} minutes";
            if (span.Minutes == 1) return $"{span.Minutes} minute";
            if (span.Seconds > 10) return $"{span.Seconds} seconds";

            return "now";
        }
    }
}
