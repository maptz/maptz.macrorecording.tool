using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Maptz.MacroRecording.Tool
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ShowItemsWindow : Window
    {
        public ShowItemsWindow()
        {
            InitializeComponent();
            ShowActivated = false; //Prevent activation when it comes up
            Topmost = true;
            Loaded += (s, e) =>
            {
                IsOpen = true;
                RepositionWindow();
            };
        }

        public bool IsOpen { get; set; }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosed(e);
            //=> Application.Current.Windows.Cast<Window>().Any(x => x == this);
            IsOpen = false;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Topmost = true;
        }
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            Topmost = true;
        }

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


        internal void CloseIfOpen()
        {
            if (IsOpen)
            {
                Close();
            }
        }
    }
}
