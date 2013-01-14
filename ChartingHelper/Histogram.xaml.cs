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
        }

        private void draw(double binSize) {
            var series = vals.Histogram(binSize);
            setSeries(series, (List<double>)series.Tag);
        }

        List<double> vals = null;

        private PlotModel _plot = new PlotModel();

        public PlotModel Plot {
            get { return _plot; }
            set { _plot = value;
            OnPropertyChanged("Plot");
            }
        }

        private Brush errorColor = Brushes.LightPink;
        private Brush workingColor = Brushes.LightGreen;
        double binSizeVal;

        private string _binSize;
        public string BinSize {
            get { return _binSize; }
            set {
                _binSize = value;
                if (!double.TryParse(this.binSize.Text, out binSizeVal)) {
                    this.binSize.Background = errorColor;
                } else {
                    this.binSize.Background = workingColor;
                }
                OnPropertyChanged("BinSize");
            }
        }

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
            if (!double.TryParse(this.binSize.Text, out binSizeVal)) return;
            draw(binSizeVal);

        }

        private void binSize_TextChanged(object sender, TextChangedEventArgs e) {
            if (!double.TryParse(this.binSize.Text, out binSizeVal)) {
                this.binSize.Background = errorColor;
            } else {
                this.binSize.Background = workingColor;
            }
        }   
    }
}
