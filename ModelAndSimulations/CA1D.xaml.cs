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
        int[] transitionRuleArray = new int[8] { 0, 1, 1, 1, 1, 0, 0, 0 };

        public int RowLength {
            get { return _RowLength; }
            set {
                if (_RowLength != value) {
                    _RowLength = value;
                    OnPropertyChanged(RowLengthPropertyName);
                }
            }
        }
        private int _RowLength;
        public const string RowLengthPropertyName = "RowLength";

        public int RectWidth {
            get { return _RectWidth; }
            set {
                if (_RectWidth != value) {
                    _RectWidth = value;
                    OnPropertyChanged(RectWidthPropertyName);
                }
            }
        }
        private int _RectWidth;
        public const string RectWidthPropertyName = "RectWidth";

        private int transitionRule(List<int> vals, int idx, int numberOfStates) {
            ///3 inspection pixels, 8 input types
            var layerLength = vals.Count;
            int minus1 = idx - 1 < 0 ? idx - 1 + vals.Count() : idx - 1;
            int plus1 = idx + 1 >= vals.Count() ? idx + 1 - vals.Count() : idx + 1;
            inspectionVals[0] = vals[minus1];
            inspectionVals[1] = vals[idx];
            inspectionVals[2] = vals[plus1];
            if (compare(0, 0, 0)) {
                return transitionRuleArray[0];
            } else if (compare(0, 0, 1)) {
                return transitionRuleArray[1];
            } else if (compare(0, 1, 0)) {
                return transitionRuleArray[2];
            } else if (compare(0, 1, 1)) {
                return transitionRuleArray[3];
            } else if (compare(1, 0, 0)) {
                return transitionRuleArray[4];
            } else if (compare(1, 0, 1)) {
                return transitionRuleArray[5];
            } else if (compare(1, 1, 0)) {
                return transitionRuleArray[6];
            } else if (compare(1, 1, 1)) {
                return transitionRuleArray[7];
            }
            throw new Exception();
        }

        const int NUMBER_OF_STATES = 2;

        public CA1D() {
            InitializeComponent();
            this.RectWidth = 5;
            this.RowSize = 50;
            this.Layers = new List<Layer>();
            this.DataContext = this;
            this.RandomConfig = true;
            for (int i = 0; i < 1; i++) {
                this.Layers.Add(Layer.Generate(NUMBER_OF_STATES, RowSize, j => rand.Next(5)));
            }
            system = new CASystem(NUMBER_OF_STATES, this.Layers.First(), transitionRule);
            this.LayersControl.ItemsSource = this.Layers;
            this.iterate_bw = new BackgroundWorker();
            this.iterate_bw.DoWork += new DoWorkEventHandler(iterate_bw_DoWork);
        }

        void iterate_bw_DoWork(object sender, DoWorkEventArgs e) {
            iterate();
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

        BackgroundWorker iterate_bw;

        private void iterate() {
            int times = 0;

            Dispatcher.Invoke((Action)(() => {
                transitionRule();
                try {
                    times = int.Parse(this.times.Text);
                } catch {
                    times = 1;
                }
            }));

            for (int i = 0; i < times; i++) {
                this.Layers.Add(system.Iterate());
            }

            Dispatcher.Invoke((Action)(() => {
                updateLayersUI();
            }));
        }

        private void Iterate_Click(object sender, RoutedEventArgs e) {
            if (iterate_bw.IsBusy) {
                return;
            }
            iterate_bw.RunWorkerAsync();
        }

        private void updateLayersUI() {
            this.LayersControl.ItemsSource = null;
            this.LayersControl.ItemsSource = this.Layers;
        }

        public bool RandomConfig {
            get { return _RandomConfig; }
            set {
                if (_RandomConfig != value) {
                    _RandomConfig = value;
                    OnPropertyChanged(RandomConfigPropertyName);
                }
            }
        }
        private bool _RandomConfig;
        public const string RandomConfigPropertyName = "RandomConfig";

        private void transitionRule() {
            this.transitionRuleArray[0] = int.Parse(this._000.Text);
            this.transitionRuleArray[1] = int.Parse(this._001.Text);
            this.transitionRuleArray[2] = int.Parse(this._010.Text);
            this.transitionRuleArray[3] = int.Parse(this._011.Text);
            this.transitionRuleArray[4] = int.Parse(this._100.Text);
            this.transitionRuleArray[5] = int.Parse(this._101.Text);
            this.transitionRuleArray[6] = int.Parse(this._110.Text);
            this.transitionRuleArray[7] = int.Parse(this._111.Text);
        }

        private void restart() {
            this.Layers.Clear();
            if (this.RandomConfig) {
                this.Layers.Add(Layer.Generate(NUMBER_OF_STATES, RowSize, i => rand.Next(5)));
            } else {
                this.Layers.Add(Layer.Generate(NUMBER_OF_STATES, RowSize, i => {
                    if (RowSize / 2 == i) {
                        return 0;
                    } else {
                        return 1;
                    }
                }));
            }

            system = new CASystem(NUMBER_OF_STATES, this.Layers.First(), transitionRule);
            updateLayersUI();
        }

        private void Restart_Click(object sender, RoutedEventArgs e) {
            restart();
        }

        Brush validBrush = Brushes.LightGreen;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            try {
                this.RectWidth = int.Parse((sender as TextBox).Text);
                (sender as TextBox).Background = validBrush;
            } catch {
                (sender as TextBox).Background = Brushes.Red;
            }
        }

        public int RowSize {
            get { return _RowSize; }
            set {
                if (_RowSize != value) {
                    _RowSize = value;
                    OnPropertyChanged(RowSizePropertyName);
                }
            }
        }
        private int _RowSize;
        public const string RowSizePropertyName = "RowSize";

        private void RowSize_TextChanged(object sender, TextChangedEventArgs e) {
            try {
                this.RowSize = int.Parse((sender as TextBox).Text);
                (sender as TextBox).Background = validBrush;
            } catch {
                (sender as TextBox).Background = Brushes.Red;
            }
        }
    }
}
