using UnityEngine;

namespace Assets.Genetic_Algorithm.Scripts
{
    public class Furniture : MonoBehaviour
    {
        public GameObject FurnitiGameObject { get; set; }
        public FurnitureCategory Category;
        private static readonly System.Random Random = new System.Random();

        #region Helper Methods
        public void DestroyFurniture()
        {
            // FurnitiGameObject.SetActive(false);
            Destroy(FurnitiGameObject);
        }
        public static Vector3 GetRandomPossition(int maxXPos, int maxZPos)
        {
            return new Vector3(Random.Next(maxXPos), 0, Random.Next(maxZPos));

        }
        public static Quaternion GetRandomRotation()
        {
            return Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        } 
        #endregion
    }
}
