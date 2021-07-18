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
    public class SaveItemWindowModel : NotifyPropertyChangedBase
    {
        public SaveItemWindowModel(SaveItemWindow window, IEnumerable<RecorderEvent> events)
        {
            AvailableSlots = Enumerable.Range(0, 10).ToArray();
            Window = window;
            Events = events;
            CloseCommand = new CommandWrapper(() => { Window.DialogResult = true; Window.Close(); });
            CancelCommand = new CommandWrapper(() => { Window.DialogResult = false; Window.Close(); });
        }

        public SaveItemWindow Window { get; }
        public IEnumerable<RecorderEvent> Events { get; }

        public int[] AvailableSlots { get; }
        
        private int _currentSlot = 0;
        public int CurrentSlot
        {
            get => _currentSlot;
            set { if (_currentSlot != value) { _currentSlot = value; NotifyPropertyChanged(); } }
        }

        public ICommand CloseCommand { get; }
        public ICommand CancelCommand { get; }
    }
}