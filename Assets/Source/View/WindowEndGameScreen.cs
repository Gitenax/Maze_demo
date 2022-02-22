using UnityEngine;

namespace Source.View
{
    [RequireComponent(typeof(Animator))]
    public sealed class WindowEndGameScreen : Window
    {
        private Animator _animator;
       
        private void Awake() => _animator = GetComponent<Animator>();

        public void FadeIn()
        {
            _animator.SetTrigger(Animator.StringToHash(Triggers.FadeIn));
            _animator.ResetTrigger(Animator.StringToHash(Triggers.FadeOut));
        }

        public void FadeOut()
        {
            _animator.SetTrigger(Animator.StringToHash(Triggers.FadeOut));
            _animator.ResetTrigger(Animator.StringToHash(Triggers.FadeIn));
        }

        private sealed class Triggers
        {
            public const string FadeIn = nameof(FadeIn);
            public const string FadeOut = nameof(FadeOut);
        }
    }
}