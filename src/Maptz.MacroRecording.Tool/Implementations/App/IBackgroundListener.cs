using System;
namespace Maptz.MacroRecording.Tool
{

    public interface IBackgroundListener : IDisposable
    {
        event EventHandler<KeyEventArgs> KeyDown;
        event EventHandler<KeyEventArgs> KeyUp;

        void StartListening();
    }
}