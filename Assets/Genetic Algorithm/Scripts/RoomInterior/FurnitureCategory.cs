namespace Assets.Genetic_Algorithm.Scripts
{
    [System.Serializable]
    public class FurnitureCategory
    {
        public Category Category;

        /// <summary>
        /// the list of object categories which can be 
        /// possible parents of a current object. 
        /// </summary>
        public Category[] PossibleParents;

        /// <summary>
        /// [front, back, left, right]
        /// specify the empty space around the 
        /// furniture object required for its comfortable use
        /// </summary>
        public float ClearanceConstraints;

        /// <summary>
        /// specifies how important is it for an 
        /// object to stand near the wall
        /// </summary>
        public decimal ProbabilityOfSnapptingToWall;

        /// <summary>
        /// represents importance for the object of being in a 
        /// group relationship with the other objects. 
        /// </summary>
        public decimal ProbabilityoFHavingAParent;

        /// <summary>
        /// Contains a minimum required and maximum allowed 
        /// number of objects of this category in a specific room. 
        /// For example a maximum of one television can be in a living room.
        /// </summary>
        public int DesiredCount;
    }
}
