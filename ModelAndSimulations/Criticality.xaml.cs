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
using ChartingHelper;
using System.Diagnostics;
using AvalonDock.Layout;
using OxyPlot;

namespace ModelAndSimulations {
    /// <summary>
    /// Interaction logic for Criticality.xaml
    /// </summary>
    public partial class Criticality : UserControl {
        public Criticality() {
            InitializeComponent();
            this.count.Value = 10000;
            //spawn(1000);
        }

        private void spawn(int steps, int startingVal = 1) {
            Random rand = new Random();
            double position = 1;
            List<double> vals = new List<double>();
            List<double> vals2 = new List<double>();
            for (int i = 0; i < steps; i++) {
                double step = rand.NextDouble();
                if (rand.Next(2) == 1) {
                    step *= -1;
                }
                position += step;
                double toAdd = 1.0 / position;
                vals.Add(toAdd);
                vals2.Add(position);
            }
            string t1 = "Inverse";
            string t2 = "Random Walk";

            var g1 = vals.LineGraph();
            g1.Title = t1;
            var g2 = vals2.LineGraph();
            g2.Title = t2;
            List<LineSeries> series = new List<LineSeries> { g1, g2 };

            addChart(series.Graph(false), "Line Graph");
            addChart(new Histogram(vals, t1), t1);
            addChart(new Histogram(vals2, t2), t2);
        }

        private void addChart(UserControl ct, string title) {
            this.pane.Children.Add(new LayoutDocument() { Content = ct, Title = title });
            this.pane.Children.First().IsSelected = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            spawn((int)this.count.Value);
        }
    }
}
