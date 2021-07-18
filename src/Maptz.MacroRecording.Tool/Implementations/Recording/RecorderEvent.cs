using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace Maptz.MacroRecording.Tool
{

    public class RecorderEvent
    {
        public double OffsetSeconds { get; set; }
        public RecorderEventType EventType { get; set; }
        public VKeys Key { get; set; }
        public int MouseX { get; set; }
        public int MouseY { get; set; }
        
        public override string ToString()
        {
            string extra;
            if (EventType == RecorderEventType.KeyDown || EventType == RecorderEventType.KeyUp)
            {
                extra = Key.ToString();
            }
            else
            {
                extra = $"{MouseX}, {MouseY}";
            }
            return $"{EventType} - {extra}";
        }
    }
}