using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChaosGame {
    class Frame {
        public Frame(int size) {
            size++;
            this.r = size / 2;
            this.size = size;
            this.frame = new byte[size][];
            for (int i = 0; i < size; i++) {
                frame[i] = new byte[size];
            }
        }

        private int r;
        private int size;
        private byte[][] frame;

        internal void Set(Vec2 v) {
            this.frame[v.X + r][v.Y + r] = 255;
        }

        private byte[] toByteArray() {
            var l = this.frame.Length;
            byte[] output = new byte[l * l];
            for (int i = 0; i < l; i++) {
                Buffer.BlockCopy(this.frame[i], 0, output, i * l, l);
            }
            return output;
        }

        public BitmapSource ToBitmap() {
            var buffer = toByteArray();
            var dpiX = 96d;
            var dpiY = 96d;
            var pixelFormat = PixelFormats.Gray8; // grayscale bitmap
            var bytesPerPixel = (pixelFormat.BitsPerPixel + 7) / 8; // == 1 in this example
            var stride = bytesPerPixel * this.size; // == width in this example
            var bitmap = BitmapSource.Create(this.size, this.size, dpiX, dpiY,
                                             pixelFormat, null, buffer, stride);

            return bitmap as BitmapSource;
        }
    }
}
