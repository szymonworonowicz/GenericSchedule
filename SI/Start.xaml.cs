using Newtonsoft.Json;
using SI.Controller;
using SI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

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
            GenericController gc;
            List<List<GenericItem>>[] schedules = new List<List<GenericItem>>[items.Count];

            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Group item = items[i] as Group;
                    gc = new GenericController(item, schedules);

                    schedules[i] = gc.Generate(300).ToList();

                    MainWindow window = new MainWindow(item, schedules[i]);

                    window.Show();

                }

                this.Close();
            }
        }
    }

}

