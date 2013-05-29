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

namespace ModelAndSimulations {
    /// <summary>
    /// Interaction logic for CA1D.xaml
    /// </summary>
    public partial class CA1D : UserControl, INotifyPropertyChanged {
        Random rand = new Random();
        CASystem system { get; set; }
        private int[] inspectionVals = new int[3];
        private bool compare(int a, int b, int c) {
            return inspectionVals[0] == a && inspectionVals[1] == b && inspectionVals[2] == c;
        }
        private int transitionRule(List<int> vals, int idx, int numberOfStates) {
            ///3 inspection pixels, 8 input types
            var layerLength = vals.Count;
            int minus1 = idx - 1 < 0 ? idx - 1 + vals.Count() : idx - 1;
            int plus1 = idx + 1 >= vals.Count() ? idx + 1 - vals.Count() : idx + 1;
            inspectionVals[0] = vals[minus1];
            inspectionVals[1] = vals[idx];
            inspectionVals[2] = vals[plus1];
            if (compare(0, 0, 0)) {
                return 0;    
            } else if (compare(0, 0, 1)) {
                return 1;
            } else if (compare(0, 1, 0)) {
                return 1;
            } else if (compare(0, 1, 1)) {
                return 1;
            } else if (compare(1, 0, 0)) {
                return 1;
            } else if (compare(1, 0, 1)) {
                return 0;
            } else if (compare(1, 1, 0)) {
                return 0;
            } else if (compare(1, 1, 1)) {
                return 0;
            }
            throw new Exception();
        }

        const int NUMBER_OF_STATES = 2;

        public CA1D() {
            InitializeComponent();
            this.Layers = new List<Layer>();

            for (int i = 0; i < 1; i++) {
                this.Layers.Add(Layer.Generate(NUMBER_OF_STATES, 50, () => rand.Next(5)));
            }
            system = new CASystem(NUMBER_OF_STATES, this.Layers.First(), transitionRule);
            this.LayersControl.ItemsSource = this.Layers;
        }         
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public List<Layer> Layers {
            get { return _Layers; }
            set {
                if (_Layers != value) {
                    _Layers = value;
                    OnPropertyChanged(LayersPropertyName);
                }
            }
        }
        private List<Layer> _Layers;
        public const string LayersPropertyName = "Layers";

        private void Iterate_Click(object sender, RoutedEventArgs e) {
            this.Layers.Add(system.Iterate());
            this.LayersControl.ItemsSource = null;
            this.LayersControl.ItemsSource = this.Layers;
        }
    }
}
