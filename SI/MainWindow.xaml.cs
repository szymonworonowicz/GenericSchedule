using SI.Models;
using System.Collections.Generic;
using System.Windows;

namespace SI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(Group group, List<List<GenericItem>> list)
        {
            InitializeComponent();
            this.Title = $"Plan klasy {group.Id}";

            Mon.ItemsSource = list[0];
            Tue.ItemsSource = list[1];
            Wed.ItemsSource = list[2];
            Thu.ItemsSource = list[3];
            Fri.ItemsSource = list[4];


        }
    }
}
