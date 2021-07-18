using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
namespace Maptz.MacroRecording.Tool
{
    public class MacroSlot
    {
        public string Description { get; set; }
        public IEnumerable<RecorderEvent> Events { get; set; }
    }
}