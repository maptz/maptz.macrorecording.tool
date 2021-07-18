using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
namespace Maptz.MacroRecording.Tool
{
    public class MacroSlotStore : IMacroSlotStore
    {
        public MacroSlotStore(IOptions<MacroSlotStoreSettings> settings)
        {
            Settings = settings.Value;
        }

        public MacroSlotStoreSettings Settings { get; }

        public MacroSlotCollection Load()
        {
            try
            {
                if (File.Exists(Settings.FilePath))
                {
                    var contents = File.ReadAllText(Settings.FilePath);
                    var result = JsonConvert.DeserializeObject<MacroSlotCollection>(contents);
                    return result;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Failed to read slots." + ex.ToString());

            }            
            return new MacroSlotCollection();
        }

        public void Save(MacroSlotCollection slotCollection)
        {
            var outputFileInfo = new FileInfo(Settings.FilePath);
            if (!outputFileInfo.Directory.Exists) outputFileInfo.Directory.Create();
            var json = JsonConvert.SerializeObject(slotCollection);
             File.WriteAllText(outputFileInfo.FullName, json);
        }
    }
}