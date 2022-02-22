using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Source.View.Buttons
{
    public sealed class UIShieldButton : UIButton
    {
        [SerializeField] private string _coundownEndCommand;
        [SerializeField] private TMP_Text _timerText;
        private bool _isAlreadyPressed;
        private bool _isCoundownComplete = true;
        
        public event  EventHandler<string> OnCoundownEnd;
        
        public void DrawCountdown(TimeSpan time)
        {
            if(_isAlreadyPressed)
                return;
            
            _timerText.gameObject.SetActive(true);
            StartCoroutine(nameof(Coundown), time);
        }

        protected override void OnPointerDown()
        {
            _isAlreadyPressed = true;
        }

        protected override void OnPointerUp()
        {
            _isCoundownComplete = true;
            _isAlreadyPressed = false;
        }

        private IEnumerator Coundown(TimeSpan time)
        {
            _isCoundownComplete = false;
            double remaining = time.TotalSeconds;

            while (remaining > 0)
            {
                if(_isCoundownComplete)
                {
                    Complete();
                    yield break;
                }
                
                remaining -= Time.deltaTime;
                _timerText.text = @$"{TimeSpan.FromSeconds(remaining):ss\:ff}";
                yield return null;
            }

            Complete();
            
            void Complete()
            {
                _timerText.gameObject.SetActive(false);
                _isAlreadyPressed = false;
                _isCoundownComplete = true;
                OnCoundownEnd?.Invoke(this, _coundownEndCommand);
            }
        }
    }
}