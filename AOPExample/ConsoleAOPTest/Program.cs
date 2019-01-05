using AOPLibrary;
using NConcern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleAOPTest
{
    class Program
    {
        static void Main(string[] args)
        {

            // Existe algumas limitações
            // Não funcionou com classes Staticas
            // Não funciona para métodos privados
            
            // Mas é uma fácil maneira de atingir praticamente todo o sistema com pouca alteração.

            foreach (var type in GetAllClassByExecutionAssemblies())
            {
                if (IsValidType(type))
                    Aspect.Weave<Logging>(type);
            }


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

            Example2 example2 = new Example2();

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

        static Type[] GetAllClassByExecutionAssemblies()
        {
            // Transformar em parâmetro
            var assembliesFilter = new string[] { "AOPLibrary", "ConsoleAOPTest" };

            List<Type> typeList = new List<Type>();
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembliesFilter.Contains(assembly.GetName().Name))
                    typeList.AddRange(assembly.GetTypes());
            }

            return typeList.ToArray();
        }

        static bool IsValidType(Type type)
        {
            if (type.FullName != null
                // Representa que não são as classe de injeção do Neptune
                && !type.FullName.ToUpper().Contains("NEPTUNE")
                // Representa uma classe não estática
                && !type.IsAbstract && !type.IsSealed
                // Representa uma classe pública 
                && type.IsPublic)
                return true;

            return false;
        }
    }
}
