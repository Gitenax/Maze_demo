using UnityEngine;

namespace Source.Maze
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public sealed class MazeCell : MonoBehaviour
    {
        [SerializeField] private GameObject _wallLeft;
        [SerializeField] private GameObject _wallBottom;
        [SerializeField] private GameObject _floor;

        public GameObject WallLeft => _wallLeft;
        public GameObject WallBottom => _wallBottom;
        public GameObject Foor => _floor;

        private void Awake()
        {
            return;
            
            MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
            var combine = new CombineInstance[filters.Length];

            for (int i = 0; i < filters.Length; i++)
            {
                combine[i].mesh = filters[i].sharedMesh;
                combine[i].transform = filters[i].transform.localToWorldMatrix;
                filters[i].gameObject.SetActive(false);
            }

            var meshFilter = GetComponent<MeshFilter>();
            
            meshFilter.mesh = new Mesh();
            meshFilter.mesh.CombineMeshes(combine);
            gameObject.SetActive(true);
        }
    }
}