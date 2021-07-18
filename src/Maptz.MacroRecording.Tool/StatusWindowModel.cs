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

    public class StatusWindowModel : NotifyPropertyChangedBase
    {
        private bool _isRecording;
        public bool IsRecording
        {
            get => _isRecording;
            set { if (value != _isRecording) { _isRecording = value; NotifyPropertyChanged(); } }
        }

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set { if (value != _isPlaying) { _isPlaying = value; NotifyPropertyChanged(); } }
        }

        public override string ToString()
        {
            return $"IsRecording: {IsRecording}, IsPlaying: {IsPlaying}.";
        }
    }
}