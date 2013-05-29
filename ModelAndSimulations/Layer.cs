using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ModelAndSimulations {
    public class Layer {
        public static Layer Generate(int numColors, int length, Func<int> generator){
            var l = new Layer();
            for (int i = 0; i < length; i++) {
                var c = generator() % numColors;
                l.Add(c);
            }
            return l;
        }

        public Layer() {
            this.Values = new List<int>();
        }

        private static List<Brush> converter = new List<Brush>() { Brushes.Black, Brushes.White, 
            Brushes.Red, Brushes.Green, Brushes.Blue, 
            Brushes.Orange, Brushes.Yellow, Brushes.Violet };

        public List<Brush> Colors {
            get {
                return Values.Select(i => converter[i]).ToList();
            }
        }
        public List<int> Values { get; set; }

        internal void Add(int val) {
            this.Values.Add(val);
        }
    }
}
