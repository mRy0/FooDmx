using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaktionslogik für DmxFader.xaml
    /// </summary>
    public partial class DmxFader : UserControl
    {
        public byte Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                UpdateFader();
            }
        }
        public int Address 
        { 
            set
            {
                _address = value;
                this.lblAddress.Content = $"{Address}";
            }
            get { return _address; }
        }
        private byte _value = 0;
        private int _address = 0;

        private bool _up = true;
        private System.Timers.Timer _autoTimer;

        public Action<Models.Events.DmxChannelEvent>? ValueChanged { get; set; }


        public DmxFader()
        {
            _autoTimer = new System.Timers.Timer(250);
            _autoTimer.Elapsed += _autoTimer_Elapsed;
            InitializeComponent();
            this.lblAddress.Content = $"{Address}";
            UpdateFader();
        }

        private void _autoTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (_up && _value == 255)
                _up = false;
            else if(!_up && _value == 0)
                _up = true;

            if (_up)
                _value++;
            else
                _value--;

            InvokedUpdate();
        }

        private void InvokedUpdate()
        {
            if(this.Dispatcher.Thread.Equals(Thread.CurrentThread))
            {
                UpdateFader();
                SendEvents();
            }
            else
            {
                this.Dispatcher.Invoke(InvokedUpdate);
            }
        }

        private void UpdateFader()
        {
            sldFader.Value = _value;
            tbInput.Text = _value.ToString();
        }

        private void sldFader_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _value = (byte) e.NewValue;
            tbInput.Text = _value.ToString();
            SendEvents();
        }

        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(byte.TryParse(tbInput.Text, out byte bValue))
            {
                _value = bValue;
                sldFader.Value = bValue;
                SendEvents();
            }
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }


        private void SendEvents()
        {
            ValueChanged?.Invoke(new Models.Events.DmxChannelEvent()
            {
                ChannelId = (ushort)Address,
                Value = _value,
            });
        }

        private void btAuto_Click(object sender, RoutedEventArgs e)
        {
            if (_autoTimer.Enabled)
            {
                _autoTimer.Stop();
                btAuto.Background = new SolidColorBrush(SystemColors.ControlBrush.Color);
            }
            else
            {
                _up = (_value < 127);
                _autoTimer.Start();
                btAuto.Background = new SolidColorBrush(Colors.Green);
            }

        }

        private void tbSpeed_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(tbSpeed.Text, out int iSpeed))
            {
                if (_autoTimer.Enabled)
                {
                    _autoTimer.Stop();
                    _autoTimer.Interval = iSpeed;
                    _autoTimer.Start();
                }
                else
                {
                    _autoTimer.Interval = iSpeed;
                }

            }
        }
    }
}
