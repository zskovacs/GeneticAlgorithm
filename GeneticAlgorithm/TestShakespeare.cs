using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public class TestShakespeare
    {
        string targetString = "To be, or not to be, that is the question.";
        string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";
        int populationSize = 200;
        float mutationRate = 0.01f;
        int elitism = 5;
        
        int numCharsPerText = 15000;
        string targetText;
        string bestText;
        string bestFitnessText;
        string numGenerationsText;
        string textPrefab;

        private GeneticAlgorithm<char> ga;
        private Random random;

        public bool Enabled { get; set; }

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


            Console.Clear();
            Console.WriteLine("Best genes: {0}", new string(ga.BestGenes));

            Console.WriteLine("Best fitness: {0}", ga.BestFitness);
            Console.WriteLine("Best generation: {0}", ga.Generation);
            Console.WriteLine("Population: {0}", ga.Population.Count);

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

        private int numCharsPerTextObj;
        private List<string> textList = new List<string>();

        public void Awake()
        {
            numCharsPerTextObj = numCharsPerText / validCharacters.Length;
            if (numCharsPerTextObj > populationSize) numCharsPerTextObj = populationSize;

            int numTextObjects = (int)Math.Ceiling((float)populationSize / numCharsPerTextObj);

            for (int i = 0; i < numTextObjects; i++)
            {
                textList.Add(textPrefab);
            }
        }

        private void UpdateText(char[] bestGenes, float bestFitness, int generation, int populationSize, Func<int, char[]> getGenes)
        {
            bestText = CharArrayToString(bestGenes);
            bestFitnessText = bestFitness.ToString();

            numGenerationsText = generation.ToString();

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

        private string CharArrayToString(char[] charArray)
        {
            var sb = new StringBuilder();
            foreach (var c in charArray)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
