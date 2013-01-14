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
using System.Threading;
using System.Collections.ObjectModel;
using ChartingHelper;
using AvalonDock.Layout;

namespace ModelAndSimulations {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        ObservableCollection<Urn> urns;
        public MainWindow() {
            InitializeComponent();
            urns = new ObservableCollection<Urn>();
            this.simulationsUI.ItemsSource = urns;
            this.DataContext = this;
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
                var urn = new Urn(this.Increment, 0);
                urn.SimulationComplete += new EventHandler(urn_SimulationComplete);
                urns.Insert(0, urn);
                urns.First().Start(100000);
            }
        }

        public List<double> ratios = new List<double>();

        void urn_SimulationComplete(object sender, EventArgs e) {
            ratios.Add(((Urn)sender).Ratio);
        }

        private void Poll() {
            List<double> results = new List<double>();
            foreach (var a in urns) {
                results.Add(a.Ratio);
            }
            results.LineGraph().Graph().ShowUserControl();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Poll_Click(object sender, RoutedEventArgs e) {
            List<double> results = new List<double>();
            List<Chart> charts = new List<Chart>();
            for(int i=0; i< urns.Count();i++){
                charts.Add(urns[i].FirstN.LineGraph().Graph());
            }
            addCharts(charts, "convergence");
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

        private void Histogram_Click(object sender, RoutedEventArgs e) {
            addChart(new Histogram(ratios, .05), "Histogram");
        }

        private void Clear_Click(object sender, RoutedEventArgs e) {
            this.urns.Clear();
        }
    }
}
