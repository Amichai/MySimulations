using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelAndSimulations {
    /// <summary>
    /// Interaction logic for ChaosGame.xaml
    /// </summary>
    public partial class ChaosGame : UserControl, INotifyPropertyChanged {
        public ChaosGame() {
            ///TODO: persist selection
            ///Fractional move should be a slider
            //Make the chaos game flip book - for each 
            InitializeComponent();
            this.Radius = 200;
            this.initialPoints = new List<Vector>();
            this.NumberOfVertices = 10;
            this.NumberOfIterations = 1000;
            this.FractionalMove = .63;
            this.reset();
        }

        private double _FractionalMove;
        public double FractionalMove {
            get { return _FractionalMove; }
            set {
                if (_FractionalMove != value) {
                    _FractionalMove = value;
                    OnPropertyChanged("FractionalMove");
                }
            }
        }
        public int TotalNumberOfPoints {
            get {
                if (this.canvas == null) {
                    return 0;
                }
                return this.canvas.Children.Count; 
            }
        }

        private int _NumberOfIterations;
        public int NumberOfIterations {
            get { return _NumberOfIterations; }
            set {
                if (_NumberOfIterations != value) {
                    _NumberOfIterations = value;
                    OnPropertyChanged("NumberOfIterations");
                }
            }
        }

        private int _NumberOfVertices;
        public int NumberOfVertices {
            get { return _NumberOfVertices; }
            set {
                if (_NumberOfVertices != value) {
                    _NumberOfVertices = value;
                    OnPropertyChanged("NumberOfVertices");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name) {
            var eh = PropertyChanged;
            if (eh != null) {
                eh(this, new PropertyChangedEventArgs(name));
            }
        }

        private static Random rand = new Random();

        private void nextPoint() {
            var targetPoint = this.initialPoints[rand.Next(this.initialPoints.Count())];
            var dx = targetPoint.X - this.currentPoint.X;
            var dy =  targetPoint.Y - this.currentPoint.Y;
            var x = dx * this.FractionalMove + this.currentPoint.X;
            var y = dy * this.FractionalMove + this.currentPoint.Y;
            var newPt = new Vector(x, y);
            //Debug.Print(string.Format("current pt: {0}, target pt: {1}, resultant pt: {2}",
            //    this.currentPoint, targetPoint, newPt));
            this.currentPoint = newPt;
            addPoint(x, y); 
        }

        private void addPoint(double x, double y) {
            var e = new Ellipse() {
                Width = 1,
                Height = 1,
                Fill = Brushes.Blue
            };
            this.canvas.Children.Add(e);
            Canvas.SetLeft(e, x + xOffset);
            Canvas.SetTop(e, y + yOffset);
        }

        private double _Radius;
        public double Radius {
            get { return _Radius; }
            set {
                if (_Radius != value) {
                    _Radius = value;
                    OnPropertyChanged("Radius");
                }
            }
        }

        private Vector currentPoint;

        private void Run_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < this.NumberOfIterations; i++) {
                this.nextPoint();
            }
            OnPropertyChanged("TotalNumberOfPoints");
        }

        private double xOffset {
            get {
                return this.Radius + 10;
            }
        }

        private double yOffset {
            get {
                return this.Radius + 10;
            }
        }

        private List<Vector> initialPoints;

        private void createInitialPoints() {
            this.initialPoints.Clear();
            var dTheta = (Math.PI * 2) / this.NumberOfVertices;
            double theta = 0;
            for (int i = 0; i < NumberOfVertices; i++)
			{
                double x = this.Radius * Math.Cos(theta);
                double y = this.Radius * Math.Sin(theta);

                this.addPoint(x, y);
                this.initialPoints.Add(new Vector(x, y));

                theta += dTheta;
			}
        }

        private void Reset_Click(object sender, RoutedEventArgs e) {
            reset();
        }

        private void reset() {
            this.canvas.Children.Clear();
            this.createInitialPoints();
            this.currentPoint = new Vector(rand.Next((int)this.Radius), 
                    rand.Next((int)this.Radius));
        }
    }
}
