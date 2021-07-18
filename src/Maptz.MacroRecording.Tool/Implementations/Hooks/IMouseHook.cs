using Maptz.MacroRecording.Tool.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
namespace Maptz.MacroRecording.Tool
{



    public interface IMouseHook
    {
        event MouseHookCallback LeftButtonDown;
        event MouseHookCallback LeftButtonUp;
        event MouseHookCallback RightButtonDown;
        event MouseHookCallback RightButtonUp;
        event MouseHookCallback MouseMove;
        event MouseHookCallback MouseWheel;
        event MouseHookCallback DoubleClick;
        event MouseHookCallback MiddleButtonDown;
        event MouseHookCallback MiddleButtonUp;
        void Install();
        void Uninstall();
    }
}