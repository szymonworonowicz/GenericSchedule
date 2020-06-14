using Newtonsoft.Json;
using SI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SI
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public List<Group> groups { get; set; }
        public List<string> StringGroups { get; set; }
        public Start()
        {

            InitializeComponent();


            using (StreamReader GroupReader = File.OpenText("../../../Data/Groups.json"))
            {
                var groupsText = GroupReader.ReadToEnd();
                groups = JsonConvert.DeserializeObject<List<Group>>(groupsText);
            }
            Groups.ItemsSource = groups;

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var items = Groups.SelectedItems;
            
            if (items != null)
            {
                Group item = items[0] as Group;
                var Window = new MainWindow(item.CountofPerson);
                Window.Show();
                this.Close();
            }
        }
    }

}

