using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AOPLibrary
{
    public class Example
    {
        public void WriteMessageExample()
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ===> Este é um método exemplo que terá uma espera de 1 segundo");
            Thread.Sleep(1000);
        }

        public static void WriteMessageExampleStatic()
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ===> Este é um método static de exemplo que terá uma espera de 1 segundo");
            Thread.Sleep(1000);
        }

        public static void WriteMessageExampleStaticForThread()
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ===> Este é um método static exemplo terá uma espera de 1 segundo, usado em thread");
            Thread.Sleep(1000);
        }

        public static void WriteMessageExampleStaticWithException()
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ===> Este é um método static exemplo com lançamento de Exception");
            throw new Exception($"[{Thread.CurrentThread.ManagedThreadId}] ===> Lançamento de Exception");
        }

    }
}
