using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Maptz.MacroRecording.Tool
{
    /// <summary>
    /// Interaction logic for StatusWindow.xaml
    /// </summary>
    public partial class StatusWindow : Window
    {
        public StatusWindow(StatusWindowModel model)
        {
            Model = model;
            InitializeComponent();

            x_Grid.DataContext = model;
            model.PropertyChanged += (s, e) =>
            {
                //Debug.WriteLine("SW Model PropertyChanged: " + e.PropertyName + model.ToString());
                //Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    InvalidateVisual();
                //}), null);
            };
            this.Topmost = true;

            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }

        public StatusWindowModel Model { get; }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.Topmost = true;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            this.Topmost = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.IsPlaying = !Model.IsPlaying;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Model.IsRecording = !Model.IsRecording;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}
