using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public class TestConsoleText
    {
        string targetString;
        string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
        int populationSize = 200;
        float mutationRate = 0.01f;
        int elitism = 5;
        
        int numCharsPerText = 15000;

        private int numCharsPerTextObj;
        private List<string> textList = new List<string>();

        private GeneticAlgorithm<char> ga;
        private Random random;

        private bool verbose;
        public bool Enabled { get; set; }


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

            UpdateText(ga.BestGenes, ga.BestFitness, ga.Generation, ga.Population.Count, (j) => ga.Population[j].Genes);

            if(verbose)
            {
                Console.Clear();
                WriteColor("Best genes", new string(ga.BestGenes));
                WriteColor("Best fitness", ga.BestFitness.ToString(), ColorMapper(ga.BestFitness));
                WriteColor("Generation", ga.Generation.ToString());
                WriteColor("Population", ga.Population.Count.ToString());
            }

            if (ga.BestFitness == 1)
            {
                Enabled = false;
            }
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

        public void Initialize()
        {
            numCharsPerTextObj = numCharsPerText / validCharacters.Length;
            if (numCharsPerTextObj > populationSize) numCharsPerTextObj = populationSize;

            int numTextObjects = (int)Math.Ceiling((float)populationSize / numCharsPerTextObj);

            for (int i = 0; i < numTextObjects; i++)
            {
                textList.Add("");
            }
        }

        private void UpdateText(char[] bestGenes, float bestFitness, int generation, int populationSize, Func<int, char[]> getGenes)
        {
            for (int i = 0; i < textList.Count; i++)
            {
                var sb = new StringBuilder();
                int endIndex = i == textList.Count - 1 ? populationSize : (i + 1) * numCharsPerTextObj;
                for (int j = i * numCharsPerTextObj; j < endIndex; j++)
                {
                    foreach (var c in getGenes(j))
                    {
                        sb.Append(c);
                    }
                    if (j < endIndex - 1) sb.AppendLine();
                }

                textList[i] = sb.ToString();
            }
        }

        private ConsoleColor ColorMapper(float fitness)
        {
            if(fitness < 0.2)
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
    }
}
