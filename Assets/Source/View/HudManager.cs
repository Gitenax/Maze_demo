using System;
using System.Linq;
using System.Reflection;
using Source.View.Buttons;
using UnityEngine;

namespace Source.View
{
    public class HudManager : MonoBehaviour
    {
        [SerializeField] private UIShieldButton _shieldButton;
        [SerializeField] private UIButton _pauseButton;
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _rebuildButton;
        [SerializeField] private WindowPauseScreen _pauseScreen;
        [SerializeField] private WindowEndGameScreen _endGameScreen;

        public event Action<UIButton, string> OnButtonClick;
        public event Action<UIButton, string> OnButtonBeginPress;
        public event Action<UIButton, string> OnButtonEndPress;
        
        private void OnEnable()
        {
            _shieldButton.OnButtonPressBegin += OnButtonPressBeginHandler;
            _shieldButton.OnButtonPressEnd += OnButtonPressEndHandler;
            _shieldButton.OnCoundownEnd += OnButtonPressEndHandler;
            _pauseButton.OnButtonClick += OnButtonClickHandler;
            _restartButton.OnButtonClick += OnButtonClickHandler;
            _rebuildButton.OnButtonClick += OnButtonClickHandler;
            _pauseScreen.OnButtonClick += OnButtonClickHandler;
        }

        private void OnDisable()
        {
            _shieldButton.OnButtonPressBegin -= OnButtonPressBeginHandler;
            _shieldButton.OnButtonPressEnd -= OnButtonPressEndHandler;
            _shieldButton.OnCoundownEnd -= OnButtonPressEndHandler;
            _pauseButton.OnButtonClick -= OnButtonClickHandler;
            _restartButton.OnButtonClick -= OnButtonClickHandler;
            _rebuildButton.OnButtonClick -= OnButtonClickHandler;
            _pauseScreen.OnButtonClick -= OnButtonClickHandler;
        }

        public TWindow GetWindow<TWindow>() where TWindow : Window
        {
            return typeof(HudManager)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(f => f.FieldType == typeof(TWindow))?
                .GetValue(this) as TWindow;
        }

        private void OnButtonClickHandler(object sender, string command) => OnButtonClick?.Invoke((UIButton)sender, command);

        private void OnButtonPressBeginHandler(object sender, string command) => OnButtonBeginPress?.Invoke((UIButton)sender, command);

        private void OnButtonPressEndHandler(object sender, string command) => OnButtonEndPress?.Invoke((UIButton)sender, command);
    }
}