using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsInput;
namespace Maptz.MacroRecording.Tool
{
    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(VKeys vKeys)
        {
            Key = vKeys;
        }

        public VKeys Key { get; }
        public bool IsHandled { get; set; }
    }
}