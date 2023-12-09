using System;
using System.Collections.Generic;
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

namespace FooDmx
{
    /// <summary>
    /// Interaktionslogik für Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        private Dictionary<string, UserControl> _optionWindows = new Dictionary<string, UserControl>();

        public Options()
        {
            InitializeComponent();

      
        }


        public void SetOption(string name, UserControl page)
        {
            _optionWindows.Add(name, page);


            var menu = new TreeViewItem() { Header = name };

            menu.Selected += Menu_Selected;

            trvOptions.Items.Add(menu);


        }

        private void Menu_Selected(object sender, RoutedEventArgs e)
        {
            var menu = sender as TreeViewItem;
            if (menu == null) return;

            var name = menu.Header as string;
            var page = _optionWindows[name];

            gpName.Header = name;
            contentControl.Content = page;
        }

    }
}
