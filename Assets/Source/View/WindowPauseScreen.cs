using System;
using Source.View.Buttons;
using UnityEngine;

namespace Source.View
{
    public class WindowPauseScreen : Window
    {
        [SerializeField] private UIButton _resumeButton;
        [SerializeField] private UIButton _exitButton;
        
        public event Action<UIButton, string> OnButtonClick;

        private void OnEnable()
        {
            _resumeButton.OnButtonClick += OnButtonClickHandler;
            _exitButton.OnButtonClick += OnButtonClickHandler;
        }

        private void OnDisable()
        {
            _resumeButton.OnButtonClick -= OnButtonClickHandler;
            _exitButton.OnButtonClick -= OnButtonClickHandler;
        }

        private void OnButtonClickHandler(object sender, string command) => OnButtonClick?.Invoke((UIButton)sender, command);
    }
}