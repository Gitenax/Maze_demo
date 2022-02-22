using System.Collections.Generic;
using Source.Interaction.Areas;
using UnityEngine;

namespace Source.Maze
{
    public sealed class TrapSpawner : MonoBehaviour
    {
        private const int MinimalGapBetweenTraps = 1;
        
        [SerializeField] private DeadArea _prefab;
        [SerializeField] private int _trapToDeploy = 3;
        
        public void SetTraps(MazePath path)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            int remainingToDeploy = _trapToDeploy;
            var usedPoints = new List<int>();
           
            if(path.Count <= _trapToDeploy || path.Count < 3)
                return;
            
            while (remainingToDeploy != 0)
            {
                // Не берем стартовую и конечную ячейку с небольшим буфером
                int randomPoint = Random.Range(2, path.Count - 2); 
                
                if(usedPoints.Contains(randomPoint))
                    continue;
                
                bool minimalGap = false;
                foreach (int point in usedPoints)
                {
                    if (Mathf.Abs(point - randomPoint) != MinimalGapBetweenTraps)
                        continue;
                    
                    minimalGap = true;
                    break;
                }
                
                if(minimalGap)
                    continue;
                
                usedPoints.Add(randomPoint);
                
                Vector3 ranomPosition = path[randomPoint];
                Vector3 XZtrapPosition = new Vector3(ranomPosition.x, _prefab.transform.position.y, ranomPosition.z);

                Instantiate(_prefab, XZtrapPosition, Quaternion.identity, transform);

                remainingToDeploy--;
            }
        }
    }
}