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
using System.Windows.Input;

namespace Maptz.MacroRecording.Tool
{
    public class AppEngine
    {
        public AppEngine(IServiceProvider serviceProvider)
        {
            AppState = new AppState();
            ServiceProvider = serviceProvider;
            AppState.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IAppState.Mode))
                {

                }
            };
            ShowItemsWindow = serviceProvider.GetRequiredService<ShowItemsWindow>();
            Recorder = ServiceProvider.GetRequiredService<IRecorder2>();
            MacroSlotStore = ServiceProvider.GetRequiredService<IMacroSlotStore>();
            Playback = ServiceProvider.GetRequiredService<IPlayback>();
            BackgroundListener = ServiceProvider.GetRequiredService<IBackgroundListener>();
            StatusWindowModel = new StatusWindowModel();
            StatusWindow = new StatusWindow(StatusWindowModel);
            
            BackgroundListener.KeyUp += (s, e) =>
             {
                 //Process the keyon this thread, so IsHandled is set. 
                 ProcessKey(e, true);
             };
            BackgroundListener.KeyDown += (s, e) =>
            {
                //Process the keyon this thread, so IsHandled is set. 
                ProcessKey(e, false);
                //Have to handle both key down and key up. We'll react to the keyup, but swallow key down if it's relevant.
            };


        }

        public void Initialize()
        {
            Debug.WriteLine("Starting background listener");
            BackgroundListener.StartListening();
            Debug.WriteLine("Loading slots");
            Slots = MacroSlotStore.Load();
            StatusWindow.Hide();
        }

        private async Task ProcessKey(KeyEventArgs e, bool isKeyUp)
        {
            void setMode(AppMode mode)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    SetMode(mode);
                }, System.Windows.Threading.DispatcherPriority.Normal);
            }
            var isControlDown = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            if (isControlDown && e.Key == VKeys.OEM_3)
            {
                if (AppState.Mode == AppMode.Idle)
                {
                    if (isKeyUp) setMode(AppMode.ShowingItems);
                    e.IsHandled = true;
                }
                else if (AppState.Mode == AppMode.ShowingItems)
                {
                    if (isKeyUp) setMode(AppMode.Idle);
                    e.IsHandled = true;
                }
            }
            else if (isControlDown && e.Key == VKeys.KEY_0)
            {
                if (AppState.Mode == AppMode.Idle)
                {
                    if (isKeyUp)
                    {
                        setMode(AppMode.Recording);
                        System.Diagnostics.Debug.WriteLine("Start recording");
                        Recorder.Record(items =>
                        {
                            System.Diagnostics.Debug.WriteLine("Finished recording detected in APp Enine. Continuing on APp thread");
                            var op = ShowItemsWindow.Dispatcher.BeginInvoke((Action<IEnumerable<RecorderEvent>>)(ev =>
                             {
                                 Debug.WriteLine("OnCompleted being called on App thread");
                                 ProcessRecordedEvents(items);
                             }), System.Windows.Threading.DispatcherPriority.Send, items);
                            setMode(AppMode.Idle);
                        });
                    }
                }
            }
            else if (AppState.Mode == AppMode.ShowingItems)
            {
                var range = Enumerable.Range((int)VKeys.KEY_0, 10).Cast<VKeys>();
                if (range.Contains(e.Key))
                {
                    if (isKeyUp)
                    {
                        setMode(AppMode.Playing);
                        var slotIndex = (int)e.Key - (int)VKeys.KEY_0;
                        await PlayFromSlotAsync(slotIndex);
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            SetMode(AppMode.Idle);
                        }, System.Windows.Threading.DispatcherPriority.Normal);
                    }
                    e.IsHandled = true;
                }
            }
        }

        private void ProcessRecordedEvents(IEnumerable<RecorderEvent> items)
        {
            System.Diagnostics.Debug.WriteLine($"{items.Count()} events");
            var saveItemsWindow = new SaveItemWindow(items);
            var dialogResult = saveItemsWindow.ShowDialog();
            if (dialogResult.HasValue && dialogResult.Value)
            {
                var slotIndex = saveItemsWindow.Model.CurrentSlot;
                var events = saveItemsWindow.Model.Events;
                var macroSlot = new MacroSlot
                {
                    Events = events,
                    Description = "Not Implemented"
                };
                System.Diagnostics.Debug.WriteLine($"Saving events to slot {slotIndex}");
                if (Slots.ContainsKey(slotIndex)) Slots[slotIndex] = macroSlot;
                else Slots.Add(slotIndex, macroSlot);
                MacroSlotStore.Save(Slots);
                System.Diagnostics.Debug.WriteLine($"Finished saving events");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Cancelled by user. Recording ");
            }
        }

        private async Task PlayFromSlotAsync(int slotIndex)
        {
            SetMode(AppMode.Playing);
            Debug.WriteLine($"Playing from slot {slotIndex}");
            if (!Slots.ContainsKey(slotIndex)) return;
            var macro = Slots[slotIndex];
            await Playback.Play(macro.Events);
            Debug.WriteLine($"Playback complete.");
            await Task.CompletedTask;
            
        }

        public IAppState AppState { get; }
        public IServiceProvider ServiceProvider { get; }
        public ShowItemsWindow ShowItemsWindow { get; private set; }
        public IRecorder2 Recorder { get; }
        public IMacroSlotStore MacroSlotStore { get; }
        public IPlayback Playback { get; }
        public IBackgroundListener BackgroundListener { get; }
        public StatusWindowModel StatusWindowModel { get; }
        public StatusWindow StatusWindow { get; }
        public MacroSlotCollection Slots { get; private set; }

        public void SetMode(AppMode appMode)
        {
            if (appMode == AppMode.Idle)
            {
                ShowItemsWindow.CloseIfOpen();
                StatusWindowModel.IsRecording = false;
                StatusWindowModel.IsPlaying = false;
            }
            else if (appMode == AppMode.Initializing)
            {
                throw new NotSupportedException();
            }
            else if (appMode == AppMode.Playing)
            {
                ShowItemsWindow.CloseIfOpen();
                StatusWindowModel.IsRecording = false;
                StatusWindowModel.IsPlaying = true;
            }
            else if (appMode == AppMode.Recording)
            {
                ShowItemsWindow.CloseIfOpen();
                StatusWindowModel.IsRecording = true;
                StatusWindowModel.IsPlaying = false;
            }
            else if (appMode == AppMode.ShowingItems)
            {
                StatusWindowModel.IsRecording = false;
                StatusWindowModel.IsPlaying = false;
                ShowItemsWindow.CloseIfOpen();
                ShowItemsWindow = new ShowItemsWindow();
                ShowItemsWindow.Show();
            }
            
            var shouldStatusWindowBeVisible = StatusWindowModel.IsRecording || StatusWindowModel.IsPlaying;
            var show = shouldStatusWindowBeVisible && !StatusWindow.IsVisible;
            var hide = StatusWindow.IsVisible && !shouldStatusWindowBeVisible;
            if (show) StatusWindow.Show();
            if (hide) StatusWindow.Hide();

            AppState.Mode = appMode;
        }
    }
}