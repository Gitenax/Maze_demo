using System;
using UnityEngine;

namespace Source.Interaction.Areas
{
    public class SafeArea : Area
    {
        [SerializeField] private GameObject _confetti;

        public event Action OnSafeAreaReached;
        
        protected override void OnTriggerEnterHandler(Collider other)
        {
            if(LayerMask.GetMask(Constants.Layers.Player) != 1 << other.gameObject.layer)
                return;

            _confetti.SetActive(true);
            OnSafeAreaReached?.Invoke();
        }
    }
}