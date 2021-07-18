using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
namespace Maptz.MacroRecording.Tool
{
    public interface IRecorder2
    {
        bool IsRecording { get; }
        void Record(Action<IEnumerable<RecorderEvent>> onCompleted);
    }

    public class Recorder2 : IRecorder2
    {
        public List<RecorderEvent> Events { get; } = new List<RecorderEvent>();

        public Recorder2(IKeyboardHook keyboardHook, IMouseHook mouseHook)
        {
            Debug.WriteLine("Recorder construted");
            KeyboardHook = keyboardHook;
            MouseHook = mouseHook;

            KeyboardHook.KeyDown += e =>
            {
                if (!IsRecording) return false;

                if (e == VKeys.ESCAPE)
                {
                    Debug.WriteLine("Escape intercepted");
                    CancellationTokenSource.Cancel();
                }
                else
                {
                    AddKeyEvent(RecorderEventType.KeyDown, e);
                }
                return false;
            };

            KeyboardHook.KeyUp += e =>
            {
                if (!IsRecording) return false;
                AddKeyEvent(RecorderEventType.KeyUp, e);
                return false;
            };

            MouseHook.DoubleClick += e =>
            {
                if (!IsRecording) return;
                AddMouseEvent(RecorderEventType.DoubleClick, e.pt.x, e.pt.y);
            };

            MouseHook.LeftButtonDown += e =>
            {
                if (!IsRecording) return;
                AddMouseEvent(RecorderEventType.LeftButtonDown, e.pt.x, e.pt.y);
            };
            MouseHook.LeftButtonUp += e =>
            {
                if (!IsRecording) return;
                AddMouseEvent(RecorderEventType.LeftButtonUp, e.pt.x, e.pt.y);
            };
            MouseHook.MiddleButtonDown += e =>
            {
                if (!IsRecording) return;
                AddMouseEvent(RecorderEventType.MiddleButtonDown, e.pt.x, e.pt.y);
            };
            MouseHook.MiddleButtonUp += e =>
            {
                if (!IsRecording) return;
                AddMouseEvent(RecorderEventType.MiddleButtonUp, e.pt.x, e.pt.y);
            };
            MouseHook.MouseMove += e =>
            {
                if (!IsRecording) return;
                
                AddMouseEvent(RecorderEventType.MouseMove, e.pt.x, e.pt.y);
            };
            MouseHook.MouseWheel += e =>
            {
                if (!IsRecording) return;
                //AddEvent<System.Drawing.Point>(RecorderEventType.MouseWheel, new System.Drawing.Point(e.pt.x, e.pt.y));
            };
            MouseHook.RightButtonDown += e =>
            {
                if (!IsRecording) return;
                AddMouseEvent(RecorderEventType.RightButtonDown, e.pt.x, e.pt.y);
            };
            MouseHook.RightButtonUp += e =>
            {
                if (!IsRecording) return;
                AddMouseEvent(RecorderEventType.RightButtonUp, e.pt.x, e.pt.y);
            };
        }
        private void AddKeyEvent(RecorderEventType eventType, VKeys keys)
        {
            if (CancellationTokenSource == null || CancellationTokenSource.IsCancellationRequested) return;
            if (!IsRecording) return;

            var offsetSeconds = (DateTime.UtcNow - RecordingStartTime).TotalSeconds;
            var even = new RecorderEvent { EventType = eventType, OffsetSeconds = offsetSeconds, Key = keys };            
            Events.Add(even);
        }

        private void AddMouseEvent(RecorderEventType eventType, int x, int y)
        {
            if (CancellationTokenSource == null || CancellationTokenSource.IsCancellationRequested) return;
            if (!IsRecording) return;

            var offsetSeconds = (DateTime.UtcNow - RecordingStartTime).TotalSeconds;
            var even = new RecorderEvent { EventType = eventType, OffsetSeconds = offsetSeconds, MouseX = x, MouseY = y };
            Events.Add(even);
        }


        
        private DateTime RecordingStartTime { get; set; }
        private object _lock = new object();

        public void Record(Action<IEnumerable<RecorderEvent>> onCompleted)
        {
            if (IsRecording) throw new InvalidOperationException();
            Events.Clear();
            RecordingStartTime = DateTime.UtcNow;
            CancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = CancellationTokenSource.Token;
            IsRecording = true;

            Debug.WriteLine("Recorder2 recording");
            var t = new Thread(new ThreadStart(() =>
            {
                do
                {
                    Thread.Sleep(2);
                } while (!CancellationTokenSource.IsCancellationRequested);
                IsRecording = false;
                try
                {
                    Debug.WriteLine("Cancellation request fulfilled. Calling onCompleted");
                    onCompleted(Events);
                    Debug.WriteLine("onCompleted called");
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("onCompleted error " + ex.ToString());
                }
            }));
            t.Start();
           
        }


        public bool IsRecording { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; }
        public IKeyboardHook KeyboardHook { get; }
        public IMouseHook MouseHook { get; }

        public void Dispose()
        {
            
        }
    }
}