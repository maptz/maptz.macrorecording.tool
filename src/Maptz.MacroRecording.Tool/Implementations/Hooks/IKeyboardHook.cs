using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
namespace Maptz.MacroRecording.Tool
{

    public interface IKeyboardHook
    {
        event KeyboardHookCallback KeyDown;
        event KeyboardHookCallback KeyUp;

        void Install();
        void Uninstall();
    }
}