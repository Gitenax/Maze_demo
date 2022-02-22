using UnityEngine;

namespace Source.Interaction.Areas
{
    public class Area : MonoBehaviour
    {
        [SerializeField] protected ColliderTrigger _geomertyTrigger;

        private void OnEnable()
        {
            _geomertyTrigger.OnEnter += OnTriggerEnterHandler;
            _geomertyTrigger.OnStay += OnTriggerStayHandler;
        }

        private void OnDisable()
        {
            _geomertyTrigger.OnEnter -= OnTriggerEnterHandler;
            _geomertyTrigger.OnStay  -= OnTriggerStayHandler;
        }
        
        protected virtual void OnTriggerEnterHandler(Collider other) {}
        protected virtual void OnTriggerStayHandler(Collider other) {}
    }
}