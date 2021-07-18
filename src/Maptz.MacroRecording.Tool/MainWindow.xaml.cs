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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* #region Private Fields */

        /* #endregion Private Fields */
        /* #region Private Methods */
        private void RepositionWindow()
        {
            var transform = PresentationSource.FromVisual(this).CompositionTarget.TransformFromDevice;
            var mousePos = transform.Transform(MouseEx.GetMousePosition());
            RepositionWindow(mousePos);
        }
        private void RepositionWindow(Point mousePosition)
        {
            Debug.WriteLine($"Positioning mouse at {mousePosition}");
            Left = mousePosition.X - Width;
            Top = mousePosition.Y - Height;
        }
        private void ToggleVisibility()
        {
            Debug.WriteLine("Toggling visibility");

        }
        private void UpdateModel()
        {
            Model.DateTime = DateTime.Now.ToString("yyyy/MM/dd");
            //Debug.WriteLine(Model.DateTime);
        }
        /* #endregion Private Methods */
        /* #region Protected Methods */
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            this.Width = 400;
            this.Height = 400;
            //this.Topmost = true;
            this.Top = 40;
            this.Left = 40;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);


        }
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);

            //this.Topmost = true;
            this.Activate();
        }
        /* #endregion Protected Methods */
        /* #region Public Properties */
        public MainWindowModel Model { get; }
        /* #endregion Public Properties */

        /* #region Public Constructors */
        public MainWindow()
        {
            InitializeComponent();


            Model = new MainWindowModel();
            x_Grid.DataContext = Model;

            WindowStyle = System.Windows.WindowStyle.None;

            //Start the timer. 
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {
                UpdateModel();
            };
            UpdateModel();
            timer.Start();

            this.PreventMove();
        }
        /* #endregion Public Constructors */
    }
}
