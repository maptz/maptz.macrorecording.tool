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

    public interface IRecorder : IDisposable
    {
        void Start();
        IEnumerable<RecorderEvent> StopAsync();
    }
}