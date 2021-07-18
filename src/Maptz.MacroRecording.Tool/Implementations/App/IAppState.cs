using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
namespace Maptz.MacroRecording.Tool
{

    public interface IAppState : INotifyPropertyChanged
    {
        AppMode Mode { get; set; }
    }
}