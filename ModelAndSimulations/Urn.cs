using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace ModelAndSimulations {
    public class Urn : Notifier {
        public int Quantity1 { get; set; }
        public int Quantity2 { get; set; }
        BackgroundWorker bw;
        static int seedVal = 0;
        public Urn(int increment, int initialValuesToSave = 100) {
            this.initialValuesToSave = initialValuesToSave;
            this.Iter = 0;
            this.FirstN = new List<double>();
            this.Quantity1 = 1;
            this.Quantity2 = 1;
            this.Increment = increment;
            this.bw = new BackgroundWorker();
            rand = new Random(seedVal++);
            this.bw.DoWork += new DoWorkEventHandler(bw_DoWork);
        }

        Random rand;

        private int initialValuesToSave;

        public int Iter { get; set; }

        public int Total {
            get {
                return Quantity1 + Quantity2;
            }
        }

        enum type { type1, type2 };

        private type pickABall() {
            int r = rand.Next(0, Total);
            if (r < Quantity1) {
                return type.type1;
            } else {
                return type.type2;
            }
        }

        public List<double> FirstN;

        public int Increment { get; set; }

        public void Iterate() {
            Iter++;
            var ball = pickABall();
            if (ball == type.type1) {
                this.Quantity1+= Increment;
            } else {
                this.Quantity2+= Increment;
            }
            if (FirstN.Count() < this.initialValuesToSave) {
                FirstN.Add(Ratio);
            }
            OnPropertyChanged("Ratio");
            OnPropertyChanged("Iter");
        }

        public double Ratio {
            get {
                return (double)Quantity1/ (Quantity2 + Quantity1);
            }
        }

        public void Start() {
            bw.RunWorkerAsync();   
        }

        private int maxIter = -1;
            
        public void Start(int iterations) {
            this.maxIter = iterations;
            bw.RunWorkerAsync();
        }

        public event EventHandler SimulationComplete;

        void bw_DoWork(object sender, DoWorkEventArgs e) {
            int i=0; 
            while(true){
                Iterate();
                i++;
                if (i >= maxIter && maxIter != -1) {
                    SimulationComplete(this, new EventArgs());
                    return;
                }
            }
        }
    }
}
