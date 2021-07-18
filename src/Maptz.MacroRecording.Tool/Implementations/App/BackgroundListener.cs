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

    public class BackgroundListener : IBackgroundListener
    {
        public event EventHandler<KeyEventArgs> KeyDown;
        public event EventHandler<KeyEventArgs> KeyUp;

        private IKeyboardHook _keyHook;

        public bool IsListening { get; private set; }

        public BackgroundListener(IKeyboardHook keyboardHook)
        {
            Debug.WriteLine("Background listener constructed");
            _keyHook = keyboardHook;

            _keyHook.KeyUp += e =>
            {
                if (!IsListening) return false;
                var args = new KeyEventArgs(e);
                KeyUp?.Invoke(this, args);
                return args.IsHandled;
            };
            _keyHook.KeyDown += e =>
            {
                if (!IsListening) return false;
                var args = new KeyEventArgs(e);
                KeyDown?.Invoke(this, args);
                return args.IsHandled;
            };
        }

        public void Dispose()
        {
            IsListening = false;
        }

        public void StartListening()
        {
            Debug.WriteLine("Background keyhook listening");
            IsListening = true;
        }
    }
}