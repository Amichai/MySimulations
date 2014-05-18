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
using ChartingHelper;

namespace ModelAndSimulations {
    /// <summary>
    /// Interaction logic for PreferentialAttachment.xaml
    /// </summary>
    public partial class PreferentialAttachment : UserControl, INotifyPropertyChanged {
        public PreferentialAttachment() {
            this.cities = new List<double>();
            this.DataContext = this;
            InitializeComponent();
            this.AlphaVal = ".1";
            this.Quantity = "10";
            this.TotalPopulation = 0;
        }

        List<double> cities;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public int SpawnQuantity { get; set; }

        private Brush validBrush = Brushes.LightGreen;
        private Brush invalidBrush = Brushes.Red;

        public string Quantity {
            get { return _Quantity; }
            set {
                if (_Quantity != value) {
                    _Quantity = value;
                    OnPropertyChanged(QuantityPropertyName);
                    try {
                        SpawnQuantity = int.Parse(value);
                        this.quantityTB.Background = validBrush;
                    } catch {
                        this.quantityTB.Background = invalidBrush;
                    }
                }
            }
        }
        private string _Quantity;
        public const string QuantityPropertyName = "Quantity";

        private static Random rand = new Random();
        public int TotalPopulation { get; set; }

        public double SpawnAlpha { get; set; }

        public string AlphaVal {
            get { return _AlphaVal; }
            set {
                if (_AlphaVal != value) {
                    _AlphaVal = value;
                    OnPropertyChanged(AlphaValPropertyName);
                    try {
                        this.SpawnAlpha = double.Parse(value);
                        this.spawnAlphaTB.Background = validBrush;
                    } catch {
                        this.spawnAlphaTB.Background = invalidBrush;
                    }
                }
            }
        }
        private string _AlphaVal;
        public const string AlphaValPropertyName = "AlphaVal";

        Histogram histogram;

        private void Button_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < SpawnQuantity; i++) {
                bool newCity = rand.NextDouble() <= SpawnAlpha;
                if (newCity) {
                    this.cities.Add(1);
                    TotalPopulation++;
                } else {
                    int cityVal = rand.Next(TotalPopulation);
                    int localCount = 0;
                    for (int j = 0; j < cities.Count(); j++) {
                        localCount += (int)Math.Round(cities[j]);
                        if (localCount > cityVal) {
                            cities[j]++;
                            TotalPopulation++;
                            break;
                        }
                    }
                }
                ///Check if we are going to spawn a new city,
                ///if not, pick the city to join
            }
            this.citySizes.Text = string.Concat(cities.Select(k => k + ","));
            this.histogram = new Histogram(cities, "cities");
            this.HistogramGrid.Children.Clear();
            this.HistogramGrid.Children.Add(this.histogram);
        }

        private void Clear_Click_1(object sender, RoutedEventArgs e) {
            this.cities.Clear();
            this.citySizes.Text = "";
            this.HistogramGrid.Children.Clear();
        }
    }
}
