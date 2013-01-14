using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using System.ComponentModel;

namespace ChartingHelper {
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : UserControl, INotifyPropertyChanged {
        public Histogram(List<double> vals, double binSize) {
            this.vals = vals;
            InitializeComponent();
            this.DataContext = this;
            draw(binSize);
            this.NumberOfDataPoints = vals.Count();
        }

        private int _numberOfDataPoints;

        public int NumberOfDataPoints {
            get { return _numberOfDataPoints; }
            set {
                _numberOfDataPoints = value;
                OnPropertyChanged("NumberOfDataPoints");
            }
        }


        private void draw(double binSize) {
            this.binSize.Value = binSize;
            var series = vals.Histogram(binSize);
            setSeries(series, (List<double>)series.Tag);
        }

        List<double> vals = null;

        private PlotModel _plot = new PlotModel();

        public PlotModel Plot {
            get { return _plot; }
            set {
                _plot = value;
                OnPropertyChanged("Plot");
            }
        }

        private Brush errorColor = Brushes.LightPink;
        private Brush workingColor = Brushes.LightGreen;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void setSeries(ColumnSeries b, List<double> xAxis) {
            Plot = new PlotModel();
            var ca = new CategoryAxis() {
                LabelField = "label",
            };
            Plot.Axes.Add(ca);
            ((CategoryAxis)Plot.Axes[0]).Labels = (IList<string>)xAxis.Select(i => i.ToString()).ToList();
            Plot.Series.Clear();
            Plot.Series.Add(b);
            OnPropertyChanged("Plot");
        }

        private void redraw_Click(object sender, RoutedEventArgs e) {
            draw(this.binSize.Value);
        }
    }
}
