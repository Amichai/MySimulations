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

namespace ChartingHelper {
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Chart : UserControl {
        PlotModel plot = new PlotModel();
        public Chart(PlotModel p){
            InitializeComponent();
            this.Root.Model = p;
        }

        public Chart(bool dateTimeAxis = true) {
            InitializeComponent();
            Random rand = new Random();
            if (dateTimeAxis) {
                plot.Axes.Add(new DateTimeAxis());
            }
            this.Root.Model = plot;
        }

        public void AddSeries(RectangleBarSeries b) {
            plot.Series.Add(b);
            this.Root.Model = plot;
        }

        public void AddSeries(ColumnSeries b, List<double> xAxis) {
            var ca = new CategoryAxis() { LabelField = "label", 
                Labels = (IList<string>)xAxis.Select(i => i.ToString()).ToList() };
            plot.Axes.Add(ca);
            plot.Series.Add(b);
            this.Root.Model = plot;
        }


        public void AddSeries(CandleStickSeries s) {
            s.Color = colors[plot.Series.Count() % colors.Count()];
            plot.Series.Add(s);
            this.Root.Model = plot;
        }

        public void AddSeries(ScatterSeries s) {
            s.MarkerFill = colors[plot.Series.Count() % colors.Count()];
            
            plot.Series.Add(s);
            this.Root.Model = plot;
        }

        public void AddSeries(LineSeries s) {
            s.Color = colors[plot.Series.Count() % colors.Count()];
            plot.Series.Add(s);
            this.Root.Model = plot;
        }


        List<OxyColor> colors = new List<OxyColor> { OxyColors.Black, OxyColors.Red, OxyColors.Blue, OxyColors.Green, OxyColors.Purple, OxyColors.Black };

        private void UserControl_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Escape:
                    ///Close this window
                    throw new NotImplementedException();
            }
        }
    }
}
