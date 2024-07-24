using FooDmx.Outputs;
using NumericUpDownLib.Base;
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


        private byte[] _addresses = new byte[512];


        public ListWindow(IOutput output)
        {
            _output = output;
            _addresses = _output.Addresses; 
            InitializeComponent();
            CreateGrid();

        }

        private void CreateGrid()
        {
            for (int j = 0; j < 32; j++)
            {
                Griddy.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i< 32; i++)
            {
                Griddy.RowDefinitions.Add(new RowDefinition());
            }


            for (int i = 0; i < 32; i += 2)
            {

                for (int j = 0; j < 32; j++)
                {
                    var txt = new TextBlock();
                    txt.Text = ((i /2 ) * 32 +  j + 1).ToString();

                    Grid.SetRow(txt, i);
                    Grid.SetColumn(txt, j);

                    Griddy.Children.Add(txt);

                }

                for (int j = 0; j < 32; j++)
                {
                    var nbr = new NumericUpDownLib.NumericUpDown();
                    nbr.MaxValue = 255;
                    nbr.MinValue = 0;
                    nbr.Value = _addresses[(i / 2) * 32 + j];
                    nbr.IsIncDecButtonsVisible = false;
                    nbr.IsLargeStepEnabled = false;
                    nbr.IsMouseDragEnabled = false;
                    nbr.IsDisplayLengthFixed = true;
                    nbr.IsEnabled = true;
                    nbr.FormatString = "D";
                    nbr.StepSize = 1;

                    Grid.SetRow(nbr, i + 1);
                    Grid.SetColumn(nbr, j + 1);

                    Griddy.Children.Add(nbr);

                }
            }


        }


        public ListWindow()
        {
            InitializeComponent();
            CreateGrid();
        }


       
      
    }
}
