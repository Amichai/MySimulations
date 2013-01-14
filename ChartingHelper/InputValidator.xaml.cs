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

namespace ChartingHelper {
    /// <summary>
    /// Interaction logic for InputValidator.xaml
    /// </summary>
    public partial class InputValidator : UserControl {
        public InputValidator() {
            InitializeComponent();
            this.text.Text = "0";
        }

        
        public Type type { get; set; }
        //public string type { get; set; }
        public int TYPE { get; set; }
        private int myVar;

        public int MyProperty {
            get { return myVar; }
            set { myVar = value; }
        }
        

        private double val = 0;
        private Brush errorColor = Brushes.LightPink;
        private Brush workingColor = Brushes.LightGreen;

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (!double.TryParse(this.text.Text, out val)) {
                this.text.Background = errorColor;
            } else {
                this.text.Background = workingColor;
            }
        }

        public double Value {
            get {
                return val;
            }
            set{
                val = value;
                this.text.Text = val.ToString();
            }

        }

        private void PlusOne_Click(object sender, RoutedEventArgs e) {
            Value += 1;
        }

        private void MinusOne_Click(object sender, RoutedEventArgs e) {
            Value -= 1;
        }

    }
}
