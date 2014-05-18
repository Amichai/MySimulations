using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ChaosGame {
    class FrameGenerator {
        public FrameGenerator(int numberOfVertices, double fractionalMove, int radius) {
            this.radius = radius;
            this.numberOfVertices = numberOfVertices;
            this.fractionalMove = fractionalMove;
            this.reset();
        }

        private int numberOfVertices;
        private double fractionalMove;

        private int radius;
        private int size {
            get {
                return radius * 2;
            }
        }

        private Frame frame;

        private List<Vec2> initialPoints;

        private void createInitialPoints() {
            this.initialPoints = new List<Vec2>();
            var dTheta = (Math.PI * 2) / this.numberOfVertices;
            double theta = 0;
            for (int i = 0; i < this.numberOfVertices; i++) {
                int x = (int)Math.Round(this.radius * Math.Cos(theta));
                int y = (int)Math.Round(this.radius * Math.Sin(theta));

                var v = new Vec2(x, y);
                this.initialPoints.Add(v);

                theta += dTheta;
            }
        }

        private void addPoint(Vec2 v) {
            this.frame.Set(v);
        }

        private void nextPoint() {
            var targetPoint = this.initialPoints[rand.Next(this.initialPoints.Count())];
            var dx = targetPoint.X - this.currentPoint.X;
            var dy = targetPoint.Y - this.currentPoint.Y;
            var x = dx * this.fractionalMove + this.currentPoint.X;
            var y = dy * this.fractionalMove + this.currentPoint.Y;
            var newPt = new Vec2(x, y);
            //Debug.Print(string.Format("current pt: {0}, target pt: {1}, resultant pt: {2}",
            //    this.currentPoint, targetPoint, newPt));
            this.currentPoint = newPt;
            addPoint(newPt);
        }

        private static Random rand = new Random();
        private Vec2 currentPoint;

        private void reset() {
            this.frame = new Frame(this.size);
            this.createInitialPoints();
            this.currentPoint = new Vec2(rand.Next((int)this.radius),
                    rand.Next((int)this.radius));
        }


        public void SaveImage(string filepath) {
            var img = this.frame.ToBitmap();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            Guid photoID = System.Guid.NewGuid();

            encoder.Frames.Add(BitmapFrame.Create(img));

            using (var filestream = new FileStream(filepath, FileMode.Create))
                encoder.Save(filestream);
        }

        internal void Run(int iter) {
            for (int i = 0; i < iter; i++) {
                this.nextPoint();
            }
        }
    }

    struct Vec2 {
        public Vec2(double x, double y) : this() {
            this.X = (int)Math.Round(x);
            this.Y = (int)Math.Round(y);            
        }

        public Vec2(int x, int y)
            : this() {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
