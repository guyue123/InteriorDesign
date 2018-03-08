using System;
using UnityEngine;
using Random = System.Random;

namespace Assets.Genetic_Algorithm.Scripts
{
    public class Chromosome<T>
    {
        public T[] Genes { get; private set; }
        public float Fitness { get; private set; }

        private readonly Random _random;
        private readonly Func<T> _getRandomGene;
        private readonly Func<int, float> _fitnessFunction;

        public Chromosome(int sizeOfGenes, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
        {
            Genes = new T[sizeOfGenes];
            this._random = random;
            this._getRandomGene = getRandomGene;
            this._fitnessFunction = fitnessFunction;

            if (shouldInitGenes)
            {
                for (int i = 0; i < Genes.Length; i++)
                {
                    Genes[i] = getRandomGene();
                }
            }
        }

        /// <summary>
        /// Helps in natural selection by scoring each chromosome
        /// </summary>
        public float CalculateFitness(int index)
        {
            Fitness = _fitnessFunction(index);
            return Fitness;
        }

        /// <summary>
        /// New child will born containing genes from both of its parents
        /// </summary>
        /// <param name="otherParent">Father</param>
        /// <returns></returns>
        public Chromosome<T> Crossover(Chromosome<T> otherParent)
        {
            Chromosome<T> child = new Chromosome<T>(Genes.Length, _random, _getRandomGene, _fitnessFunction, shouldInitGenes: false);

            for (int i = 0; i < Genes.Length; i++)
            {
                // Randomly select genes of both parents for childs
                child.Genes[i] = _random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
            }

            return child;
        }

        public void Mutate(float mutationRate)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                if (_random.NextDouble() < mutationRate)
                {
                    Genes[i] = _getRandomGene();
                }
            }
        }

        /// <summary>
        /// Destroy all genes unity gameobject
        /// </summary>
        public void DestroyAllGenes()
        {
            if (Genes is Furniture[])
            {
                Furniture[] furnitureObjects = Genes as Furniture[];

                for (int i = 0; i < furnitureObjects.Length; i++)
                {

                    furnitureObjects[i].DestroyFurniture();
                }
            }
        }


    } 
}