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
using System.ComponentModel;
using System.Collections.ObjectModel;
using ChartingHelper;
using OxyPlot;
using Xceed.Wpf.AvalonDock.Layout;
using OxyPlot.Series;

namespace ModelAndSimulations {
    /// <summary>
    /// Interaction logic for UrnModel.xaml
    /// </summary>
    public partial class UrnModel : UserControl, INotifyPropertyChanged {
        ObservableCollection<Urn> urns;
        public UrnModel() {
            InitializeComponent();
            urns = new ObservableCollection<Urn>();
            this.simulationsUI.ItemsSource = urns;
            this.DataContext = this;
            this.increment.Value = 1;
            this.quantity.Value = 10;
        }

        public int Increment {
            get {
                return (int)Math.Round(this.increment.Value);
            }
        }

        private void OnPropertyChanged(string name) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void Spawn_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < this.quantity.Value; i++) {
                var urn = new Urn(this.Increment, 1000);
                urn.SimulationComplete += new EventHandler(urn_SimulationComplete);
                urns.Insert(0, urn);
                urns.First().Start(100000);
            }
            OnPropertyChanged("NumberOfSimulations");
        }

        public List<double> ratios = new List<double>();

        private object syncRoot = new object();

        void urn_SimulationComplete(object sender, EventArgs e) {
            lock (syncRoot) {
                double newVal = ((Urn)sender).Ratio;
                ratios.Add(newVal);
                if (this.h != null) {
                    Dispatcher.Invoke((Action)(() => {
                        this.h.Add(newVal);
                    }));
                }
            }
        }

        /// There is a bug in the histogram class.

        public event PropertyChangedEventHandler PropertyChanged;

        private void Poll_Click(object sender, RoutedEventArgs e) {
            List<double> results = new List<double>();
            List<Chart> charts = new List<Chart>();
            List<LineSeries> series = new List<LineSeries>();
            for (int i = 0; i < urns.Count(); i++) {
                //charts.Add(urns[i].FirstN.LineGraph().Graph());
                series.Add(urns[i].FirstN.LineGraph());
            }
            //addCharts(charts, "convergence");
            addChart(series.Graph(), "poll");
        }

        private void addChart(UserControl ct, string title) {
            this.pane.Children.Insert(0, new LayoutDocument() { Content = ct, Title = title });
            this.pane.Children.First().IsSelected = true;
        }

        #region add charts dialog
        int currentIdx = 0;
        List<Chart> charts;
        string title;
        private void addAtIdx(int idx) {
            var ld = new LayoutDocument() { Content = charts[idx], Title = title + idx.ToString() };
            ld.Closed += new EventHandler(ld_Closed);
            this.pane.Children.Insert(0, ld);
            this.pane.Children.First().IsSelected = true;
        }
        private void addCharts(List<Chart> ct, string title) {
            currentIdx = 0;
            this.charts = ct;
            this.title = title;
            addAtIdx(currentIdx);
        }

        void ld_Closed(object sender, EventArgs e) {
            currentIdx++;
            if (currentIdx >= charts.Count()) return;
            addAtIdx(currentIdx);
        }
        #endregion
        Histogram h = null;

        private void Histogram_Click(object sender, RoutedEventArgs e) {
            h = new Histogram(ratios, "", .05);
            addChart(h, "Histogram");
        }

        public int NumberOfSimulations {
            get {
                return this.urns.Count();
            }
        }


        private void Clear_Click(object sender, RoutedEventArgs e) {
            this.urns.Clear();
            OnPropertyChanged("NumberOfSimulations");
        }

    }
}
