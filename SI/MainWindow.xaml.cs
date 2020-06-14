using SI.Controller;
using SI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GenericController Generic;

        public MainWindow()
        {
            InitializeComponent();

            Generic = new GenericController();
            var list = Generic.Generate(10).ToList();

                      

            Mon.ItemsSource = list[0];
            Tue.ItemsSource = list[1];
            Wed.ItemsSource = list[2];
            Thu.ItemsSource = list[3];
            Fri.ItemsSource = list[4];
            

        }
    }
}
