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
            List<List<List<GenericItem>>> list = Generic.Generate(10).ToList();

            UniformGrid Grid = new UniformGrid();
            

            Mon0.ItemsSource = list[0];
            Mon1.ItemsSource = list[1];
            Mon2.ItemsSource = list[2];
            Mon3.ItemsSource = list[3];
            Mon4.ItemsSource = list[4];
            Mon5.ItemsSource = list[5];
            Mon6.ItemsSource = list[6];
            Mon7.ItemsSource = list[7];
            Mon8.ItemsSource = list[8];
            Mon9.ItemsSource = list[9];

        }
    }
}
