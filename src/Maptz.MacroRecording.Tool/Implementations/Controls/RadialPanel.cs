using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WindowsInput;
namespace Maptz.MacroRecording.Tool
{
    public class RadialPanel : Panel
    {

        public static readonly DependencyProperty StartAngleProperty =
         DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(RadialPanel), new PropertyMetadata(0.0, new PropertyChangedCallback(OnStartAnglePropertyChanged)));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        private static void OnStartAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as RadialPanel).OnStartAnglePropertyChanged(e);

        private void OnStartAnglePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            InvalidateArrange();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // Measure each children and give as much room as they want 
            foreach (UIElement elem in Children)
            {
                //Give Infinite size as the avaiable size for all the children
                elem.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            }
            var baseVal = base.MeasureOverride(availableSize);
            return baseVal;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            //Arrange all children based on the geometric equations for the circle.
            if (Children.Count == 0)
                return finalSize;

            //Start from 12 o'clock position
            double _angle = (90-StartAngle) * Math.PI / 180;
            //Degrees converted to Radian by multiplying with PI/180
            double _incrementalAngularSpace = (360.0 / Children.Count) * (Math.PI / 180);
            //An approximate radii based on the avialable size , obviusly a better approach is needed here.
            double radiusX = finalSize.Width / 2.4;
            double radiusY = finalSize.Height / 2.4;
            foreach (UIElement elem in Children)
            {
                //Calculate the point on the circle for the element
                Point childPoint = new Point(Math.Cos(_angle) * radiusX, -Math.Sin(_angle) * radiusY);
                //Offsetting the point to the Avalable rectangular area which is FinalSize.
                Point actualChildPoint = new Point(finalSize.Width / 2 + childPoint.X - elem.DesiredSize.Width / 2, finalSize.Height / 2 + childPoint.Y - elem.DesiredSize.Height / 2);
                //Call Arrange method on the child element by giving the calculated point as the placementPoint.
                elem.Arrange(new Rect(actualChildPoint.X, actualChildPoint.Y, elem.DesiredSize.Width, elem.DesiredSize.Height));
                //Calculate the new _angle for the next element
                _angle -= _incrementalAngularSpace;
            }
            return finalSize;
        }
    }
}