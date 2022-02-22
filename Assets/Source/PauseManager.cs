using System.Collections.Generic;
using Source.Events;

namespace Source
{
    public sealed class PauseManager : IPause
    {
        private readonly List<IPause> _gameObjects = new List<IPause>();

        public PauseManager(List<IPause> gameObjects)
            => _gameObjects = gameObjects;

        public void Pause()
        {
            foreach (IPause gameObject in _gameObjects)
                gameObject.Pause();
        }

        public void Resume()
        {
            foreach (IPause gameObject in _gameObjects)
                gameObject.Resume();
        }
    }
}