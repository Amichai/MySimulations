using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelAndSimulations {
    public class CASystem {
        public CASystem(int numberOfStates, Layer start, Func<List<int>, int, int, int> transitionRule) {
            this.activeLayer = start;
            this.NumberOfStates = numberOfStates;
            this.TransitionRule = transitionRule;
        }
        public int NumberOfStates { get; set; }
        Layer activeLayer { get; set; }
        Func<List<int>, int, int, int> TransitionRule { get; set; }
        public Layer Iterate() {
            ///Generates a new Layer
            Layer newLayer = new Layer();
            for (int i = 0; i < activeLayer.Values.Count(); i++) {
                newLayer.Add(TransitionRule(activeLayer.Values, i, NumberOfStates));
            }
            activeLayer = newLayer;
            return activeLayer;
        }
    }
}
