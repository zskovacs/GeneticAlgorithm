using System;

namespace GeneticAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            var ga = new TestShakespeare();

            ga.Awake();
            ga.Start();
            while(ga.Enabled)
            {
                ga.Update();
            }
            

            Console.ReadLine();
        }
    }
}
