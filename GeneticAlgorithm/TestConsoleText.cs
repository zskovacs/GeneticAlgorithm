using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public class TestConsoleText
    {
        string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
        int populationSize = 200;
        float mutationRate = 0.01f;
        int elitism = 5; // Keep the best 5

        private string targetString; // The desired child
        private bool verbose;
        
        private GeneticAlgorithm<char> ga;
        private Random random;

        public bool Enabled { get; private set; }

        public TestConsoleText(string text, bool verbose = true)
        {
            this.verbose = verbose;
            targetString = text;
        }

        public void Start()
        {
            random = new Random();
            Enabled = true;
            ga = new GeneticAlgorithm<char>(populationSize, targetString.Length, random, GetRandomCharacter, FitnessFunction, elitism, mutationRate);
        }

        public void Update()
        {
            ga.NewGeneration();

            if (verbose)
                UpdateUserInterface();

            if (ga.BestFitness == 1)
                Enabled = false;
        }

        private char GetRandomCharacter()
        {
            int i = random.Next(validCharacters.Length);
            return validCharacters[i];
        }
        private float FitnessFunction(int index)
        {
            float score = 0;
            DNA<char> dna = ga.Population[index];

            for (int i = 0; i < dna.Genes.Length; i++)
            {
                if (dna.Genes[i] == targetString[i])
                {
                    score += 1;
                }
            }

            score /= targetString.Length;

            score = ((float)Math.Pow(5, score) - 1) / (5 - 1);

            return score;
        }

        #region UI
        private void UpdateUserInterface()
        {

            Console.Clear();
            WriteColor("Best genes", new string(ga.BestGenes));
            WriteColor("Best fitness", ga.BestFitness.ToString(), ColorMapper(ga.BestFitness));
            WriteColor("Generation", ga.Generation.ToString());
            WriteColor("Population", ga.Population.Count.ToString());

        }
        private ConsoleColor ColorMapper(float fitness)
        {
            if (fitness < 0.2)
            {
                return ConsoleColor.DarkRed;
            }
            else if (fitness < 0.4)
            {
                return ConsoleColor.Red;
            }
            else if (fitness < 0.6)
            {
                return ConsoleColor.DarkYellow;
            }
            else if (fitness < 0.8)
            {
                return ConsoleColor.Yellow;
            }
            else if (fitness < 1.0)
            {
                return ConsoleColor.DarkGreen;
            }
            else
            {
                return ConsoleColor.Green;
            }
        }
        private void WriteColor(string key)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(key + ": ");
            Console.ResetColor();
        }
        private void WriteColor(string key, string value, ConsoleColor color = ConsoleColor.White)
        {
            WriteColor(key);
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }
        #endregion
    }
}
