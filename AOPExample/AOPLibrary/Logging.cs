using NConcern;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace AOPLibrary
{
    public class Logging : IAspect
    {
        private static Dictionary<string, Stopwatch> WatchManagement = new Dictionary<string, Stopwatch>();

        public IEnumerable<IAdvice> Advise(MethodBase method)
        {

            yield return Advice.Basic.Before((instance, arguments) =>
            {
                lock (WatchManagement)
                {
                    string uniqueName = GetUniqueName(method, instance, arguments);
                    //string uniqueName = method.GetRefId().ToString();
                    //string uniqueName = GetUniqueNameByMemory(method, instance, arguments);
                    WatchManagement[uniqueName] = new Stopwatch();
                    WatchManagement[uniqueName].Start();
                    //Console.BackgroundColor = ConsoleColor.Blue;
                    //Console.ForegroundColor = ConsoleColor.White;
                    //Console.WriteLine($"++++++++++++++++++++++{method.Name} - Key: [{uniqueName}] - Iniciou Watch: ");
                    //Console.ResetColor();
                }
            });

            yield return Advice.Basic.After((instance, arguments) =>
            {
                string uniqueName = GetUniqueName(method, instance, arguments);
                //string uniqueName = method.GetRefId().ToString();
                //string uniqueName = GetUniqueNameByMemory(method, instance, arguments);
                lock (WatchManagement)
                {
                    if (WatchManagement.ContainsKey(uniqueName))
                    {
                        WatchManagement[uniqueName].Stop();
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(
                            $"----------------------{method.Name} - Key: [{uniqueName}] - Finalizou Watch {WatchManagement[uniqueName].ElapsedMilliseconds}");
                        Console.ResetColor();
                        WatchManagement.Remove(uniqueName);
                    }
                }
            });
        }

        private static string GetUniqueName(MethodBase method, object instance, object arguments)
        {
            var keyWatch = string.Empty;
            if (instance != null)
                keyWatch += $"{instance.GetHashCode()}_";

            //if (arguments != null)
            //    keyWatch += $"{arguments.GetHashCode()}_";

            keyWatch += $"{method.GetHashCode()}_{Thread.CurrentThread.ManagedThreadId}";

            return keyWatch;
        }

        private static string GetMemoryAdress(object obj)
        {
            GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Normal);
            IntPtr pointer = GCHandle.ToIntPtr(handle);
            handle.Free();
            return "0x" + pointer.ToString("X");
        }

        private static string GetUniqueNameByMemory(MethodBase method, object instance, object arguments)
        {
            lock (WatchManagement)
            {
                string uniqueName = String.Empty;
                uniqueName += GetMemoryAdress(method);
                if (instance != null)
                    uniqueName += $"_{GetMemoryAdress(instance)}";
                if (arguments != null)
                    uniqueName += $"_{GetMemoryAdress(arguments)}";

                return uniqueName;
            }
        }
    }
}
