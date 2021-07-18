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
    public enum RecorderEventType
    {
        KeyDown,
        KeyUp,
        DoubleClick,
        LeftButtonDown,
        LeftButtonUp,
        MiddleButtonDown,
        MiddleButtonUp,
        MouseMove,
        MouseWheel,
        RightButtonDown,
        RightButtonUp,
    }
}