using Maptz.MacroRecording.Tool.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
namespace Maptz.MacroRecording.Tool.Win32
{

    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public uint mouseData;
        public uint flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
}