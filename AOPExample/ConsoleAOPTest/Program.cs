using AOPLibrary;
using NConcern;
using System;
using System.Threading;

namespace ConsoleAOPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Aspect.Weave<Logging>(typeof(Example));

            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ===> Este comando não está em um método externo, mas o próximo estará.");

            Console.WriteLine(); Console.WriteLine();


            Thread thread1 = new Thread(new ThreadStart(ExecuteThread));
            thread1.Start();
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ===> Iniciando 1º conjunto de Thread.");

            Thread thread2 = new Thread(new ThreadStart(ExecuteThread));
            thread2.Start();
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ===> Iniciando 2º conjunto de Thread.");

            Console.WriteLine(); Console.WriteLine();

            // método comum
            new Example().WriteMessageExample();

            Console.WriteLine(); Console.WriteLine();

            // método estático
            Example.WriteMessageExampleStatic();

            try
            {
                Example.WriteMessageExampleStaticWithException();
            }
            catch { }



            Console.ReadKey();
        }

        static void ExecuteThread()
        {
            Console.WriteLine("Vamos executar 10 vezes a Thread.");

            Console.WriteLine(); Console.WriteLine();

            for (int i = 1; i <= 10; i++)
            {
                Example.WriteMessageExampleStaticForThread();
                Console.Write($"Posicao {i}");
                Console.WriteLine(); Console.WriteLine();
            }
        }


    }
}
