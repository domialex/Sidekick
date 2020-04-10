using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bindables;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Views.Prices
{
    /// <summary>
    /// Interaction logic for Sockets.xaml
    /// </summary>
    [DependencyProperty]
    public partial class Sockets : UserControl
    {
        [DependencyProperty(OnPropertyChanged = nameof(OnPropertyChanged))]
        public Item Item { get; set; }

        public Sockets()
        {
            InitializeComponent();
            Container.DataContext = this;
        }

        public List<List<string>> SocketGroups { get; set; }

        public static void OnPropertyChanged(
        DependencyObject dependencyObject,
        DependencyPropertyChangedEventArgs eventArgs)
        {
            var self = (Sockets)dependencyObject;

            var sockets = new List<List<string>>();

            if (self.Item != null && self.Item.Sockets != null)
            {
                foreach (var socket in self.Item.Sockets
                    .OrderBy(x => x.Group)
                    .GroupBy(x => x.Group)
                    .ToList())
                {
                    sockets.Add(socket
                        .Select(x => x.Colour switch
                        {
                            SocketColour.Blue => "#2E86C1",
                            SocketColour.Green => "#28B463",
                            SocketColour.Red => "#C0392B",
                            SocketColour.White => "#FBFCFC",
                            SocketColour.Abyss => "#839192",
                            _ => throw new Exception("Invalid socket"),
                        })
                        .ToList());
                }
            }

            self.SocketGroups = sockets;
        }

    }
}
