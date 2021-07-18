using Maptz.MacroRecording.Tool.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
namespace Maptz.MacroRecording.Tool.Win32
{
    public enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208
    }
}