using FooDmx.Outputs;
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

namespace FooDmx
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {

        private IOutput _output;
        List<DmxRow> _dmxRows = new();

        public class DmxRow : INotifyPropertyChanged
        {
            private int[] _values = new int[32];

            public int StartAddress { get; set; }
            public int[] Values
            {
                get => _values;
                set
                {
                    _values = value;
                    OnPropertyChanged(nameof(Values));
                }
            }
            public int this[int index]
            {
                get => _values[index];
                set
                {
                    if (_values[index] != value)
                    {
                        _values[index] = value;
                        OnPropertyChanged($"Values[{index}]");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ListWindow(IOutput output)
        {
            _output = output;
            InitializeComponent();
            LoadDmxAddresses();

            _output.Updated += _output_Updated;
        }

        private void _output_Updated(byte[] addr)
        {
            for (int i = 0; i < 16; i++)
            {                
                var row = _dmxRows[i];

                //row.Values = addr.Skip((i * 32)).Take(32).Select(x => (int)x).ToArray();

                for (int j = 0; j < 32; j++)
                {
                    row.Values[j] = addr[i * 32 + j];
                }
                    
            }
            DmxDataGrid.DataContext = _dmxRows; 

        }

        public ListWindow()
        {
            InitializeComponent();
            LoadDmxAddresses();
        }
        private void LoadDmxAddresses()
        {
            var addr = _output.Addresses;

            for (int i = 0; i < 16; i++)
            {
                var row = new DmxRow { StartAddress = (i *32) + 1 };
                row.PropertyChanged += Row_PropertyChanged;
                for (int j = 0; j < 32; j++)
                {
                    row.Values[j] = addr[(i* 16) + j];
                }
                _dmxRows.Add(row);
            }

            DmxDataGrid.ItemsSource = _dmxRows;
        }

        private void Row_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var addr = new byte[513];

            foreach (var row in _dmxRows)
            {
                for (int j = 0;j < 32; j++)
                {
                    addr[row.StartAddress + j] = (byte)row.Values[j];
                }
            }
            _output.SetAddresses (addr);
        }


        private void ValidateInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void CorrectValue(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (int.TryParse(textBox.Text, out int value))
                {
                    if (value < 0)
                    {
                        textBox.Text = "0";
                    }
                    else if (value > 255)
                    {
                        textBox.Text = "255";
                    }

                }
                else
                {
                    textBox.Text = "0";
                }
            }

        }


        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Enter)
            {
                e.Handled = true;
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private void DmxDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                var uiElement = e.OriginalSource as UIElement;
                if (uiElement != null)
                {
                    uiElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                }
            }
        }
    }
}
