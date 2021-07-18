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

    public static class RecorderEventExtensions
    {

        public static IEnumerable<RecorderEvent> RemoveOrphanUpEvents(this IEnumerable<RecorderEvent> filtered)
        {
            var idxToRemove = new List<int>();
            var filteredCount = filtered.Count();
            List<VKeys> downKeys = new();
            for (int i = 0; i < filtered.Count(); i++)
            {
                var f = filtered.ElementAt(i);
                if (f.EventType == RecorderEventType.KeyUp)
                {
                    var li = downKeys.LastIndexOf(f.Key);
                    if (li == -1)
                    {
                        idxToRemove.Add(i);
                    }
                    else
                    {
                        downKeys.RemoveAt(li);
                    }
                }
                if (f.EventType == RecorderEventType.KeyDown)
                {
                    downKeys.Add(f.Key);
                }
            }
            List<RecorderEvent> newEvents = new();
            for (int i = 0; i < filteredCount; i++)
            {
                if (!idxToRemove.Contains(i))
                {
                    newEvents.Add(filtered.ElementAt(i));
                }
            }
            return newEvents;

        }

        public static IEnumerable<RecorderEvent> RemoveOEM(this IEnumerable<RecorderEvent> filtered)
        {
            var idxToRemove = new List<int>();
            var hasRemovedOemDown = false;
            var hasRemovedOemUp = false;
            var hasRemovedCtrlDown = false;
            var filteredCount = filtered.Count();
            for (int i = 0; i < filtered.Count(); i++)
            {
                var f = filtered.ElementAt(filteredCount - 1 - i);
                if (!hasRemovedOemUp && f.EventType == RecorderEventType.KeyUp && f.Key == VKeys.OEM_3)
                {
                    idxToRemove.Add(filteredCount - 1 - i);
                    hasRemovedOemUp = true;
                }
                if (!hasRemovedOemDown && f.EventType == RecorderEventType.KeyDown && f.Key == VKeys.OEM_3)
                {
                    idxToRemove.Add(filteredCount - 1 - i);
                    hasRemovedOemDown = true;
                }
                var isCtrl = f.EventType == RecorderEventType.KeyDown && (f.Key == VKeys.LCONTROL || f.Key == VKeys.RCONTROL || f.Key == VKeys.CONTROL);
                if (!hasRemovedCtrlDown && isCtrl)
                {
                    idxToRemove.Add(filteredCount - 1 - i);
                    hasRemovedCtrlDown = true;
                }
            }
            List<RecorderEvent> newEvents = new();
            for (int i = 0; i < filteredCount; i++)
            {
                if (!idxToRemove.Contains(i))
                {
                    newEvents.Add(filtered.ElementAt(i));
                }
            }
            return newEvents;

        }

        public static IEnumerable<RecorderEvent> Filter(this IEnumerable<RecorderEvent> inEvents)
        {
            List<RecorderEvent> retval = new();
            for (int i = 0; i < inEvents.Count() - 1; i++)
            {
                var tEvent = inEvents.ElementAt(i);
                var nEvent = inEvents.ElementAt(i + 1);
                var doSkip = tEvent.EventType == RecorderEventType.MouseMove && nEvent.EventType == RecorderEventType.MouseMove;
                if (!doSkip)
                {
                    retval.Add(tEvent);
                }
            }
            return retval;
        }
    }
}