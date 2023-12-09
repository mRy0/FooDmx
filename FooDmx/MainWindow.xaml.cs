using FooDmx.Outputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FooDmx
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        byte[] _addresses = new byte[512];
        int _startAddress = 1;

        private List<Outputs.IOutput> _outputPlugins =  new List<Outputs.IOutput>();

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private IOutput _output;


        public MainWindow()
        {
            _output = new Outputs.OpenDMX.OpenDMXOutput();

            _outputPlugins.Add(new Outputs.ArtNET.ArtNETOutput());

            InitializeComponent();
            SetChannel();

            fader0.ValueChanged += SetValue;
            fader1.ValueChanged += SetValue;
            fader2.ValueChanged += SetValue;
            fader3.ValueChanged += SetValue;
            fader4.ValueChanged += SetValue;
            fader5.ValueChanged += SetValue;
            fader6.ValueChanged += SetValue;
            fader7.ValueChanged += SetValue;
            fader8.ValueChanged += SetValue;
            fader9.ValueChanged += SetValue;
            fader10.ValueChanged += SetValue;
            fader11.ValueChanged += SetValue;
            fader12.ValueChanged += SetValue;
            fader13.ValueChanged += SetValue;
            fader14.ValueChanged += SetValue;
            fader15.ValueChanged += SetValue;
            _ = _output.RunAsync(_cancellationTokenSource.Token);
        }
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        private void btBackAddress_Click(object sender, RoutedEventArgs e)
        {
            _startAddress -= 16;
            if (_startAddress < 1)
                _startAddress = 1;

            SetChannel();
        }

        private void btNextAddress_Click(object sender, RoutedEventArgs e)
        {
            _startAddress += 16;
            if (_startAddress > (513 -16))
                _startAddress = 513-16;

            SetChannel();
        }
        private void SetValue(Models.Events.DmxChannelEvent dmxChannelEvent)
        {
            _addresses[dmxChannelEvent.ChannelId - 1] = dmxChannelEvent.Value;
            _output.SetAddresses(_addresses);
        }

        private void SetChannel()
        {
            fader0.Address = _startAddress + 0;
            fader1.Address = _startAddress + 1;
            fader2.Address = _startAddress + 2;
            fader3.Address = _startAddress + 3;
            fader4.Address = _startAddress + 4;
            fader5.Address = _startAddress + 5;
            fader6.Address = _startAddress + 6;
            fader7.Address = _startAddress + 7;
            fader8.Address = _startAddress + 8;
            fader9.Address = _startAddress + 9;
            fader10.Address = _startAddress + 10;
            fader11.Address = _startAddress + 11;
            fader12.Address = _startAddress + 12;
            fader13.Address = _startAddress + 13;
            fader14.Address = _startAddress + 14;
            fader15.Address = _startAddress + 15;

            fader0.Value = _addresses[_startAddress - 1];
            fader1.Value = _addresses[_startAddress + 0];
            fader2.Value = _addresses[_startAddress + 1];
            fader3.Value = _addresses[_startAddress + 2];
            fader4.Value = _addresses[_startAddress + 3];
            fader5.Value = _addresses[_startAddress + 4];
            fader6.Value = _addresses[_startAddress + 5];
            fader7.Value = _addresses[_startAddress + 6];
            fader8.Value = _addresses[_startAddress + 7];
            fader9.Value = _addresses[_startAddress + 8];
            fader10.Value = _addresses[_startAddress + 9];
            fader11.Value = _addresses[_startAddress + 10];
            fader12.Value = _addresses[_startAddress + 11];
            fader13.Value = _addresses[_startAddress + 12];
            fader14.Value = _addresses[_startAddress + 13];
            fader15.Value = _addresses[_startAddress + 14];
        }

        private void MenuItem_Options_Click(object sender, RoutedEventArgs e)
        {
            var optWindow = new Options();

            _outputPlugins.ForEach(plugin => { optWindow.SetOption(plugin.Name, plugin.GetOptionsPage()); });
            

            optWindow.Show();
        }
        private void MenuItem_Start_Click(object sender, RoutedEventArgs e)
        {
        }

    }
}
