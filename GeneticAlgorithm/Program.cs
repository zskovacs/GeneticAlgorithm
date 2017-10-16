using System;

namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            var ga = new TestConsoleText("To be, or not to be, that is the question.");

            ga.Initialize();
            ga.Start();
            while(ga.Enabled)
            {
                ga.Update();
            }
            Console.ReadLine();
        }
    }
}
