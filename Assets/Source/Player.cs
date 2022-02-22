using System;
using System.Collections;
using Source.Effects;
using Source.Events;
using Source.Maze;
using UnityEngine;

namespace Source
{
    public sealed class Player : MonoBehaviour, IPause
    {
        private const float DelayBeforeStart = 2f;
        
        [SerializeField] private float _speed = 2f;
        [SerializeField] private GameObject _shield;
        [SerializeField] private MeshRenderer _visual;
        private CubeExplosion _effect;
        private MazePath _path;
        private Color _defaultColor;
        private Color _shieldActiveColor;
        private int _currentTargetPoint = 1;
        private bool _isMoveing;
        
        public event Action OnPlayerKilled;

        public bool ShieldActive { get; private set; }

        private void Awake()
        {
            _effect = new CubeExplosion(transform, new Vector3(0.1f, 0.1f, 0.1f), 3);
            ColorUtility.TryParseHtmlString(Constants.ColorScheme.Player, out _defaultColor);
            ColorUtility.TryParseHtmlString(Constants.ColorScheme.PlayerShieldActive, out _shieldActiveColor);
        }
        
        private void Update()
        {
            if(!_isMoveing)
                return;
           
            MoveToNextPoint();
        }
        
        void IPause.Pause()
        {
            _isMoveing = false;
        }

        void IPause.Resume()
        {
            _isMoveing = _currentTargetPoint != _path.Count;
        }

        public void StartMove(MazePath path)
        {
            _path = path;
            StartCoroutine(nameof(CountdownToStart));
        }
        
        public void SetShieldActive(bool isActive)
        {
            _shield.gameObject.SetActive(isActive);
            ShieldActive = isActive;
            _visual.sharedMaterial.color = isActive
                ? _shieldActiveColor
                : _defaultColor;
        }

        public void Kill()
        {
            gameObject.SetActive(false);
            _effect.Execute();
            _isMoveing = false;
            OnPlayerKilled?.Invoke();
        }
        
        public void Reset()
        {
            gameObject.SetActive(true);
            transform.position = new Vector3(_path[0].x, 0.5f, _path[0].z);
            _currentTargetPoint = 1;
            _isMoveing = false;
            StartCoroutine(nameof(CountdownToStart));
        }
        
        private IEnumerator CountdownToStart()
        {
            yield return new WaitForSeconds(DelayBeforeStart);
            _isMoveing = true;
        }

        private void MoveToNextPoint()
        {
            if(_currentTargetPoint == _path.Count)
                return;

            var destination = new Vector3(_path[_currentTargetPoint].x, 0, _path[_currentTargetPoint].z);
            transform.position = Vector3.MoveTowards(transform.position, destination + new Vector3(0, 0.5f, 0), _speed * Time.deltaTime);

            var currentPos = new Vector3(transform.position.x, 0, transform.position.z);
            
            if (currentPos == destination)
                _currentTargetPoint++;
        }
    }
}