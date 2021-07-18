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
using System.Windows.Shapes;

namespace Maptz.MacroRecording.Tool
{

    /// <summary>
    /// Interaction logic for SaveItemWindow.xaml
    /// </summary>
    public partial class SaveItemWindow : Window
    {
        public SaveItemWindowModel Model { get; }

        public SaveItemWindow(IEnumerable<RecorderEvent> events)
        {
            Model = new SaveItemWindowModel(this, events);
            InitializeComponent();

            x_Grid.DataContext = Model;
        }
    }




}

