using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace LinesAsDrawingBrush
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double ProbeHeight { get; set; }

        public double ProbeWidth { get; set; }

        public double LineDistance { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ProbeWidth = 100;
            ProbeHeight = 0;

            LineDistance = 10;

            HeightBox.Text = ProbeWidth.ToString();
            WidthBox.Text = ProbeHeight.ToString();
            RecreateDynamicLines(Canvas);
            CalculateViewport();
        }

        private void CalculateViewport()
        {
            var ratioX = LineDistance / ProbeWidth;
            var ratioY = LineDistance / ProbeHeight;
            DrawingBrush.Viewport = new Rect(0, 0, ratioX, ratioY);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            RecreateDynamicLines(Canvas);
            CalculateViewport();
        }

        private void RecreateDynamicLines(Panel canvas)
        {
            canvas.Children.Clear();

            var lineScale = RecalculateGridSize(ProbeWidth, ProbeHeight, canvas.ActualWidth, canvas.ActualHeight);
            if (!lineScale.Equals(Double.NaN) && !lineScale.Equals(0))
            {
                AddVerticalLines(canvas, lineScale);
                AddHorizontalLines(canvas, lineScale);
            }
        }

        private double RecalculateGridSize(double width, double height, double widthPixels, double heightPixels)
        {
            var minDim = Math.Min(width, height);
            if (minDim.Equals(0))
                minDim = 1;

            var ratio = LineDistance / minDim;
            var minDimPixels = Math.Min(widthPixels, heightPixels);
            var dimension = ratio * minDimPixels;

            return dimension;
        }

        private void AddHorizontalLines(Panel canvas, double lineScale)
        {
            var rows = Convert.ToInt32(canvas.ActualHeight / lineScale);
            var allLeftHorizontalSpace = canvas.ActualHeight - lineScale;
            for (var i = 1; i <= rows; i++)
            {
                var line = new Line
                {
                    Stroke = Brushes.Blue,
                    StrokeThickness = 1,
                    X1 = 0,
                    Y1 = allLeftHorizontalSpace,
                    X2 = canvas.ActualWidth,
                    Y2 = allLeftHorizontalSpace,
                };
                canvas.Children.Add(line);

                allLeftHorizontalSpace -= lineScale;
            }
        }

        private void AddVerticalLines(Panel canvas, double lineScale)
        {
            var cols = Convert.ToInt32(canvas.ActualWidth / lineScale);

            var allLeftSpace = lineScale;
            for (var i = 1; i <= cols; i++)
            {
                var line = new Line
                {
                    Stroke = Brushes.Blue,
                    StrokeThickness = 1,
                    X1 = allLeftSpace,
                    Y1 = 0,
                    X2 = allLeftSpace,
                    Y2 = canvas.ActualHeight,
                };
                canvas.Children.Add(line);

                allLeftSpace += lineScale;
            }
        }

        private void WidthBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (sender as TextBox);
            if (String.IsNullOrWhiteSpace(textBox.Text))
                return;

            ProbeHeight = Convert.ToDouble(textBox.Text);

            CalculateViewport();
            RecreateDynamicLines(Canvas);
        }

        private void HeightBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (sender as TextBox);
            if (String.IsNullOrWhiteSpace(textBox.Text))
                return;

            ProbeHeight = Convert.ToDouble(textBox.Text);

            CalculateViewport();
            RecreateDynamicLines(Canvas);
        }
    }
}
