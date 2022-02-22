using System.Collections.Generic;
using UnityEngine;

namespace Source.Effects
{
    public sealed class CubeExplosion
    {
        private const float RigidbodyMass = 0.2f;

        private readonly List<GameObject> _pieces = new List<GameObject>();
        private readonly Transform _parent;
        private readonly Vector3 _size;
        private readonly int _count;
        private Vector3 _pivot;

        public CubeExplosion(Transform parent, Vector3 size, int count)
        {
            _parent = parent;
            _size = size;
            _count = count;
            _pivot = _size * count / 2;
        }

        public void Execute()
        {
            const float explosionForce = 20;
            const float explosionRadius = 3;
            const float explosionUpwards = 0.4f;
            const float scaleModifier = 0.05f;
            const float destroyDelay = 2f;
            
            for (int x = 0; x < _count; x++)
            for (int y = 0; y < _count; y++)
            for (int z = 0; z < _count; z++)
            {
                CreatePiece(new Vector3(x * scaleModifier, y * scaleModifier, z * scaleModifier));
            }
            
            foreach (GameObject piece in _pieces)
            {
                piece
                    .GetComponent<Rigidbody>()
                    .AddExplosionForce(explosionForce, _parent.position, explosionRadius, explosionUpwards);
                
                Object.Destroy(piece, destroyDelay);
            }
            
            _pieces.Clear();
        }

        private void CreatePiece(Vector3 position)
        {
            GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

            piece.transform.position = 
                _parent.position 
                + new Vector3(position.x+ _size.x, position.y + _size.y, position.z + _size.z)
                - _pivot;
            
            piece.transform.localScale = _size;
            
            Rigidbody rigidbody = piece.AddComponent<Rigidbody>();
            rigidbody.mass = RigidbodyMass;
            
            _pieces.Add(piece);
        }
    }
}