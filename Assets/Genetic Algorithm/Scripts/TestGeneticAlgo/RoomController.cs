using UnityEngine;

namespace Assets.Scripts
{
    class RoomController : MonoBehaviour
    {
        public static RoomController Instance;

        [Header("Genetic Algorithm")]
        [SerializeField]
        private GameObject[] _validFurnitureObjectList;
        [SerializeField] private float _mutationRate = 0.01f;
        [SerializeField] private int _populationSize = 1;
        [SerializeField] private int _elitism = 5;
        [SerializeField] private int _maxGenesInAChromosome = 5;

        [Header("Gene (Furniture) Meta Info")]
        public int MaxZPos = 7;
        public int MaxXPos = 2;

        private GeneticAlgorithm<Furniture> GA;
        private System.Random _random;

        private void Awake()
        {
            Instance = this;
        }


        /// <summary>
        /// When the unity start runing
        /// Start method will run for the 1x time initially.
        /// </summary>
        private void Start()
        {
            _random = new System.Random();
            if (_validFurnitureObjectList.Length < 1)
            {
                Debug.Log("Valid Furniture Object List Cannot Be Empty");

                this.enabled = false;
            }

            GA = new GeneticAlgorithm<Furniture>(_populationSize, _maxGenesInAChromosome, _random, GetRandomFurnitureGene, FitnessFunction, _elitism, _mutationRate);
        }

        private void Update()
        {
            GA.NewGeneration();
        }


        /// <summary>
        /// Instantiate and get a random furniture
        /// returned furniture will be in a random position and in a random rotation
        /// </summary>
        /// <returns></returns>
        private Furniture GetRandomFurnitureGene()
        {
            int index = _random.Next(_validFurnitureObjectList.Length);
            Furniture randomFurniture = _validFurnitureObjectList[index].GetComponent<Furniture>();

            // According to research paper, position should also be random during initialization
            var randomPosition = Furniture.GetRandomPossition(MaxXPos, MaxZPos);
            var randomRotation = Furniture.GetRandomRotation();

            randomFurniture.FurnitiGameObject = Instantiate(_validFurnitureObjectList[index], randomPosition, randomRotation);
            return randomFurniture;
        }

        //TODO: write code to identify fitness function
        private float FitnessFunction(int index)
        {
            float score = 0;

            Chromosome<Furniture> selectedChromosome = GA.Population[index];

            for (int i = 0; i < selectedChromosome.Genes.Length; i++)
            {
                Furniture furniture = selectedChromosome.Genes[i];

            }

            return _random.Next(100);
        }
    }
}
