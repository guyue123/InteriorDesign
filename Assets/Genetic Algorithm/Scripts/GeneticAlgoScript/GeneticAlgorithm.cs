using System;
using System.Collections.Generic;

namespace Assets.Genetic_Algorithm.Scripts
{
    public class GeneticAlgorithm<T>
    {
        public List<Chromosome<T>> Population { get; private set; }
        public int Generation { get; private set; }
        public float BestFitness { get; private set; }
        public T[] BestGenes { get; private set; }

        public int Elitism;
        public float MutationRate;

        private List<Chromosome<T>> newPopulation;
        private Random random;
        private float fitnessSum;
        private int _maxGenes;
        private Func<T> getRandomGene;
        private Func<int, float> fitnessFunction;

        public GeneticAlgorithm(int populationSize, int maxGenes, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction,
            int elitism, float mutationRate = 0.01f)
        {
            Generation = 1;
            Elitism = elitism;
            MutationRate = mutationRate;
            Population = new List<Chromosome<T>>(populationSize);
            newPopulation = new List<Chromosome<T>>(populationSize);
            this.random = random;
            this._maxGenes = maxGenes;
            this.getRandomGene = getRandomGene;
            this.fitnessFunction = fitnessFunction;

            BestGenes = new T[maxGenes];

            for (int i = 0; i < populationSize; i++)
            {
                Population.Add(new Chromosome<T>(maxGenes, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
            }
        }

        public void NewGeneration(int numNewchromosome = 0, bool crossoverNewchromosome = false)
        {
            int finalCount = Population.Count + numNewchromosome;

            if (finalCount <= 0)
            {
                return;
            }

            if (Population.Count > 0)
            {
                // Give fitness value to each chromosome of the population
                CalculateFitnessForAllChromosomeInThePopulation();
                // Sort the population according to its fitness value. The most fittest
                // population will come on top
                Population.Sort(CompareChromosome);
            }

            // Clear all previously added new population
            for (int i = 0; i < newPopulation.Count; i++)
            {
                newPopulation[i].DestroyAllGenes(); ;
            }
            newPopulation.Clear();

            // Determine new population set
            for (int i = 0; i < Population.Count; i++)
            {
                if (i < Elitism && i < Population.Count)
                {
                    newPopulation.Add(Population[i]);
                }
                if (i < Population.Count || crossoverNewchromosome)
                {
                    // Fittest parents will be chosen for crossover
                    Chromosome<T> parent1 = ChooseTheFittestParent();
                    Chromosome<T> parent2 = ChooseTheFittestParent();
                    Chromosome<T> child = parent1.Crossover(parent2);

                    child.Mutate(MutationRate);

                    newPopulation.Add(child);
                }
                else
                {
                    newPopulation.Add(new Chromosome<T>(_maxGenes, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
                }
            }

            // Randomize the rotation and position of each gene in the chromosome 
            for (int i = 0; i < newPopulation.Count; i++)
            {
                if (newPopulation[i].Genes is Furniture[])
                {
                    var furnitures = newPopulation[i].Genes as Furniture[];

                    for (int j = 0; j < furnitures.Length; j++)
                    {
                        if (furnitures[j].FurnitiGameObject == null) continue;
                        furnitures[j].FurnitiGameObject.transform.position = Furniture.GetRandomPossition(RoomController.Instance.MaxXPos, RoomController.Instance.MaxZPos);
                        furnitures[j].FurnitiGameObject.transform.rotation = Furniture.GetRandomRotation();
                    }
                }

            }


            List<Chromosome<T>> tmpList = Population;
            Population = newPopulation;
            newPopulation = tmpList;

            Generation++;
        }

        private int CompareChromosome(Chromosome<T> a, Chromosome<T> b)
        {
            if (a.Fitness > b.Fitness)
            {
                return -1;
            }
            else if (a.Fitness < b.Fitness)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }



        private void CalculateFitnessForAllChromosomeInThePopulation()
        {
            fitnessSum = 0;
            Chromosome<T> best = Population[0];

            for (int i = 0; i < Population.Count; i++)
            {
                // Call the RoomController.FitnessFunction(index)
                fitnessSum += Population[i].CalculateFitness(i);

                // Find the best fittest chromosome in the population
                if (Population[i].Fitness > best.Fitness)
                {
                    best = Population[i];
                }
            }

            BestFitness = best.Fitness;
            best.Genes.CopyTo(BestGenes, 0);
        }

        private Chromosome<T> ChooseTheFittestParent()
        {
            double randomNumber = random.NextDouble() * fitnessSum;

            for (int i = 0; i < Population.Count; i++)
            {
                if (randomNumber < Population[i].Fitness)
                {
                    return Population[i];
                }

                randomNumber -= Population[i].Fitness;
            }

            return null;
        }
    } 
}