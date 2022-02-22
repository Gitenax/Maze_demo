using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.View.Buttons
{
    [RequireComponent(typeof(Button))]
    public class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Commands")]
        [SerializeField] private string _onClickCommand;
        [SerializeField] private string _onPressBeginCommand;
        [SerializeField] private string _onPressEndCommand;

        public event EventHandler<string> OnButtonClick; 
        public event EventHandler<string> OnButtonPressBegin; 
        public event EventHandler<string> OnButtonPressEnd;

        public Button Button { get; protected set; }

        public bool Interactable
        {
            get => Button.interactable; 
            set => Button.interactable = value;
        }

        private void Awake()
        {
            Button = GetComponent<Button>();
            OnAwake();
        }

        protected virtual void OnAwake(){}
        
        protected virtual void OnPointerDown(){}
        
        protected virtual void OnPointerUp(){}
        
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if(!Interactable)
                return;
            
            OnButtonClick?.Invoke(this, _onClickCommand);
            OnButtonPressBegin?.Invoke(this, _onPressBeginCommand);
            OnPointerDown();
        }
        
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if(!Interactable)
                return;
            
            OnButtonPressEnd?.Invoke(this, _onPressEndCommand);
            OnPointerUp();
        }
    }
}