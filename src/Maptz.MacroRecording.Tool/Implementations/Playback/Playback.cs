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

    public class Playback : IPlayback
    {
        public void Dispose()
        {

        }

        private void ExtractEventsToPlay(List<RecorderEvent> queue, List<RecorderEvent> eventsToPlay, double elapsedTimeSeconds)
        {
            var indicesToTake = new List<int>();
            for (int i = 0; i < queue.Count; i++)
            {
                var even = queue[i];
                if (even.OffsetSeconds <= elapsedTimeSeconds)
                {
                    if (indicesToTake.Any()) indicesToTake.Insert(0, i);
                    else indicesToTake.Add(i);
                }
            }
            foreach (var i in indicesToTake)
            {
                var even = queue[i];
                queue.RemoveAt(i);
                eventsToPlay.Insert(0, even);
            }
        }

        public async Task Play(IEnumerable<RecorderEvent> events, double speed = 1.0, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                var mousePosition = MouseEx.GetMousePosition();

                var queue = events.ToList();
                var startTime = DateTime.UtcNow;
                while (queue.Any())
                {
                    var elapsedTimeSeconds = (DateTime.UtcNow - startTime).TotalSeconds;
                    //Simulate a speed up effect. 
                    elapsedTimeSeconds = elapsedTimeSeconds * speed;
                    var eventsToPlay = new List<RecorderEvent>();
                    ExtractEventsToPlay(queue, eventsToPlay, elapsedTimeSeconds);
                    foreach (var even in eventsToPlay)
                    {
                        PlayEvent(even);
                    }

                    Thread.Sleep(2);
                }

                //Reset mouse to the original position. 
                var position = CalculateISMPosition((int)mousePosition.X, (int)mousePosition.Y);
                var inputSimulator = new InputSimulator();
                inputSimulator.Mouse.MoveMouseTo(position.x, position.y);
            }); ;

            //    var ts = new ThreadStart(new Action(() =>
            //{
            //}));
            //var t = new Thread(ts);

            //t.Start();
            
            //while(t.Is)

        }

        private (double x, double y) CalculateISMPosition(int mx, int my)
        {
            ////        X, Y - point in dc 1600, 900 dc size
            //Each monitor is 65535 units wide and high. 
            //Each monitor is 65535 units wide and high. 
            //TODO This is aurrently a hardocded calculation!
            var x = (2 * 65535.0) * ((double)mx) / (3840.0 * 2.0);
            var y = 65535.0 * ((double)my) / 2160.0;
            return (x: x, y: y);
        }

        private void PlayEvent(RecorderEvent even)
        {
            var inputSimulator = new InputSimulator();
            var et = even.EventType;
            switch (et)
            {
                case RecorderEventType.DoubleClick:
                    inputSimulator.Mouse.LeftButtonDoubleClick();
                    break;
                case RecorderEventType.KeyDown:
                    inputSimulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)even.Key);
                    break;
                case RecorderEventType.KeyUp:
                    inputSimulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode)even.Key);
                    break;
                case RecorderEventType.LeftButtonDown:
                    //Debug.WriteLine($"Mouse down");
                    inputSimulator.Mouse.LeftButtonDown();
                    break;
                case RecorderEventType.LeftButtonUp:
                    //Debug.WriteLine($"Mouse up");
                    inputSimulator.Mouse.LeftButtonUp();
                    break;
                case RecorderEventType.MiddleButtonUp:
                case RecorderEventType.MiddleButtonDown:
                    //Swallow
                    //Swallow
                    break;
                case RecorderEventType.MouseMove:
                    
                    var position = CalculateISMPosition(even.MouseX, even.MouseY);
                    inputSimulator.Mouse.MoveMouseTo(position.x, position.y);
                    break;
                case RecorderEventType.MouseWheel:
                    //Swallow
                    break;
                case RecorderEventType.RightButtonDown:
                    inputSimulator.Mouse.RightButtonDown();
                    break;
                case RecorderEventType.RightButtonUp:
                    inputSimulator.Mouse.RightButtonUp();
                    break;
                default:
                    throw new NotSupportedException();

            }
        }
    }
}