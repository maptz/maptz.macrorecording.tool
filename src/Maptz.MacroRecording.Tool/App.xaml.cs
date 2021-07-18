using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Maptz.MacroRecording.Tool
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            //Only shut down when we ask it to.
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IRecorder2, Recorder2>();
            serviceCollection.AddSingleton<IMouseHook, MouseHook>();
            serviceCollection.AddSingleton<IKeyboardHook, KeyboardHook>();
            serviceCollection.AddTransient<IBackgroundListener, BackgroundListener>();
            serviceCollection.AddSingleton<AppEngine>();
            serviceCollection.AddTransient<ShowItemsWindow>();
            serviceCollection.AddTransient<IAppState>(sp => sp.GetRequiredService<AppEngine>().AppState);
            serviceCollection.AddTransient<IMacroSlotStore, MacroSlotStore>();
            serviceCollection.Configure<MacroSlotStoreSettings>(settings =>
            {

            });
            serviceCollection.AddTransient<IPlayback, Playback>();
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var beginEngines = true;
            if (beginEngines)
            {
                var appEngine = ServiceProvider.GetRequiredService<AppEngine>();
                Debug.WriteLine("Starting hooking mechanism");
                var kh = ServiceProvider.GetRequiredService<IKeyboardHook>();
                var mh = ServiceProvider.GetRequiredService<IMouseHook>();
                kh.Install();
                mh.Install();
                Debug.WriteLine("Started hooking mechanism");

                ////Initialization process goes here. 
                appEngine.SetMode(AppMode.Idle);

                appEngine.Initialize();
                //var mw = new MainWindow();
                //mw.Show();
            }

        }

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceProvider.Dispose();
            base.OnExit(e);
        }
    }
}
