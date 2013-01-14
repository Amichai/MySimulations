using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OxyPlot;
using System.Windows;
using System.Windows.Controls;

namespace ChartingHelper {
    public static class Util {
        public static LineSeries LineGraph(this List<double> data) {
            var b = new LineSeries() { CanTrackerInterpolatePoints = false, StrokeThickness = 1 };
            b.Points = (List<IDataPoint>)data.Select((i, j) => (IDataPoint)(new DataPoint(j, i))).ToList();
            return b;
        }

        public static void ShowUserControl(this UserControl control) {
            Window window = new Window {
                Title = "Chart",
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Content = control
            };

            window.ShowDialog();
        }

        public static double StandardDev(this IEnumerable<double> range) {
            double ret = 0;
            if (range.Count() > 0) {
                //Compute the Average      
                double avg = range.Average();
                //Perform the Sum of (value-avg)_2_2      
                double sum = range.Sum(d => Math.Pow(d - avg, 2));
                //Put it all together      
                ret = Math.Sqrt((sum) / (range.Count() - 1));
            }
            return ret;
        }

        public static ScatterSeries GraphRankOrder(this IEnumerable<double> pts, string name, 
            bool abs = true, bool logy = false) {
            List<double> pts2;
            if (abs) {
                pts2 = pts.OrderByDescending(i => Math.Abs(i)).ToList();
            } else {
                pts2 = pts.OrderByDescending(i => i).ToList();
            }
            ScatterSeries ls = new ScatterSeries() { Title = name };
            List<IDataPoint> data = new List<IDataPoint>();
            for (int i = 0; i < pts2.Count(); i++) {
                double val = pts2[i];
                if (abs) {
                    val = Math.Abs(val);
                }
                if (logy) {
                    val = Math.Log(val);
                }
                data.Add(new DataPoint(i, val));
            }
            ls.MarkerSize = 1;
            ls.MarkerFill = OxyColors.Red;
            ls.Points = data;
            return ls;
        }

        public static Chart Graph(this LineSeries series, bool dateTimeAxis = false) {
            Chart chart = new Chart(dateTimeAxis);
            chart.AddSeries(series);
            return chart;
        }

        public static Chart Graph(this ScatterSeries series) {
            Chart chart = new Chart(false);
            chart.AddSeries(series);
            return chart;
        }

        public static Chart Graph(this List<RectangleBarSeries> series) {
            Chart chart = new Chart(false);
            foreach (var a in series) {
                chart.AddSeries(a);
            }
            return chart;
        }

        public static Chart Graph(this List<ScatterSeries> series, bool dateTimeAxis = false) {
            Chart chart = new Chart(dateTimeAxis);
            foreach (var a in series) {
                chart.AddSeries(a);
            }
            return chart;         
        }

        public static Chart Graph(this List<LineSeries> series, bool dateTimeAxis = true) {
            Chart chart = new Chart(dateTimeAxis);
            foreach (var a in series) {
                chart.AddSeries(a);
            }
            return chart;
        }

        public static Chart Graph(this LineSeries series) {
            Chart chart = new Chart(false);
            chart.AddSeries(series);
            return chart;
        }

        public static Chart Graph(this ColumnSeries series) {
            Chart chart = new Chart(false);
            chart.AddSeries(series, (List<double>)series.Tag);
            return chart;
        }

        public static Chart Graph(this RectangleBarSeries series) {
            Chart chart = new Chart(false);
            chart.AddSeries(series);
            return chart;
        }

        public static ColumnSeries Histogram(this List<double> vals, double binSize) {
            Dictionary<int, int> idxQuantity = new Dictionary<int, int>();
            foreach (var a in vals) {
                int binIdx = (int)Math.Floor(a / binSize);
                if (!idxQuantity.ContainsKey(binIdx)) {
                    idxQuantity[binIdx] = 1;
                } else {
                    idxQuantity[binIdx]++;
                }
            }

            var columnSeries = new ColumnSeries();

            var b = idxQuantity.OrderBy(i => i.Key);
            columnSeries.Tag = b.Select(i => (i.Key * binSize)).ToList();
            for(int i=0;i < idxQuantity.Count();i++){
                columnSeries.Items.Add(new ColumnItem() { 
                    CategoryIndex = i,
                    Value = b.ElementAt(i).Value,
                    Color = OxyColors.Green,
                });
            }
            return columnSeries;
        }
    }
}
    