using System;
using System.Collections.Generic;
using System.Linq;
using Source.Camera;
using Source.Events;
using Source.Maze;
using Source.View;
using Source.View.Buttons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source
{
    public sealed class ProjectContext : MonoBehaviour, IPause
    {
        [SerializeField] private MazeFactory _mazeFactory;
        [SerializeField] private GameplayCamera _camera;
        [SerializeField] private TrapSpawner _trapSpawner;
        [SerializeField] private HudManager _hudManager;
        [SerializeField] private Player _player;
        private PauseManager _pauseManager;
        private bool _isEndGame;
        
        public static ProjectContext Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            SetUpPauseManager();
            SetUpHUD();
            BuildMap();
        }

        void IPause.Pause()
        {
            Time.timeScale = 0f;
        }
        
        void IPause.Resume() => Time.timeScale = 1f;

        private void SetUpPauseManager()
        {
            List<IPause> pausableObjcects = SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .Where(g => g.GetComponentInChildren<IPause>() != null)
                .Select(g => g.GetComponentInChildren<IPause>())
                .ToList();

            _pauseManager = new PauseManager(pausableObjcects);
        }

        private void SetUpHUD()
        {
            _hudManager.OnButtonClick += OnHudManagerButtonClickHandler;
            _hudManager.OnButtonBeginPress += OnHudManagerButtonBeginPressHandler;
            _hudManager.OnButtonEndPress += OnHudManagerButtoEndPressHandler;
        }

        private void BuildMap()
        {
            _mazeFactory.OnMazeInitialized += OnMazeInitializedHandler;
            _mazeFactory.GenerateMaze();
            _mazeFactory.Finish.OnSafeAreaReached += OnFinishReachedHandler;
        }

        private void OnFinishReachedHandler()
        {
            _player.Reset();
            BuildMap();
        }

        private void SetUpPlayer(MazePath playerPath)
        {
            _player.OnPlayerKilled += () => _player.Reset();
            _player.StartMove(playerPath);
        }
        
        private void OnMazeInitializedHandler()
        {
            _mazeFactory.OnMazeInitialized -= OnMazeInitializedHandler;
            _trapSpawner.SetTraps(_mazeFactory.Maze.ShorcutPath);
            _camera.SetCamera(_mazeFactory.transform);
            SetUpPlayer(_mazeFactory.Maze.ShorcutPath);
        }
        
        private void OnHudManagerButtonClickHandler(UIButton button, string command)
        {
            switch (command)
            {
                case Constants.ButtonCommands.GamePause:
                    _pauseManager.Pause();
                    break;
                
                case Constants.ButtonCommands.GameResume:
                    _pauseManager.Resume();
                    break; 
                
                case Constants.ButtonCommands.GameRestart:
                    _player.Reset();
                    break;
                
                case Constants.ButtonCommands.GameRebuild:
                    _player.Reset();
                    BuildMap();
                    break;
                
                case Constants.ButtonCommands.GameExit:
                    Application.Quit();
                    break;
            }
        }

        private void OnHudManagerButtonBeginPressHandler(UIButton button, string command)
        {
            switch (command)
            {
                case Constants.ButtonCommands.ShieldEnable:
                    _player.SetShieldActive(true);
                    if(button is UIShieldButton shieldButton) 
                        shieldButton.DrawCountdown(new TimeSpan(0, 0, 2));
                    break;
            }
        }

        private void OnHudManagerButtoEndPressHandler(UIButton button, string command)
        {
            switch (command)
            {
                case Constants.ButtonCommands.ShieldDisable:
                    _player.SetShieldActive(false);
                    break;
            }
        }
    }
}