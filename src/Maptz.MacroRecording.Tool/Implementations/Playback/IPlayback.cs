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

    public interface IPlayback : IDisposable
    {
        Task Play(IEnumerable<RecorderEvent> events, double speed = 1.0, CancellationToken cancellationToken = default(CancellationToken));
    }
}