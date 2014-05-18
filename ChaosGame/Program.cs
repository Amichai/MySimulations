using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosGame {
    class Program {
        static void Main(string[] args) {
            var pts = 20;
            var frac = .65;
            var size = 2000;
            var iter = 100000000;
                var fg = new FrameGenerator(pts, frac, size);
                fg.Run(iter);
                var filename = string.Format("{0}_{1}_{2}_{3}.jpg", pts,
                    string.Concat(frac.ToString().Skip(2)), size, iter);
                fg.SaveImage(filename);
            //    frac += .01;
            //for (int i = 0; i < 30; i++) {
            //}
            //Process.Start(Path.Combine(Directory.GetCurrentDirectory(), filename));
        }

    }
}
