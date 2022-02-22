using System;
using UnityEngine;

namespace Source.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class ColliderTrigger : MonoBehaviour
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnStay;
        public event Action<Collider> OnExit;
        
        public Collider Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<Collider>();
            Collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other) => OnEnter?.Invoke(other);

        private void OnTriggerStay(Collider other) => OnStay?.Invoke(other);
        
        private void OnTriggerExit(Collider other) => OnExit?.Invoke(other);
    }
}