using Managers;
using UnityEngine;

namespace GameStates
{
    public abstract class StateBase : BaseMonoBehaviour
    {
        protected GameManager Game;
        protected StateManager StateManager;

        protected override void Awake()
        {
            Game = GetManager<GameManager>();
            StateManager = GetManager<StateManager>();
        }

        public virtual void StartState()
        {
            Debug.Log(name);
        }

        public virtual void EndState()
        {
            gameObject.SetActive(false);
        }

        protected virtual void NextState()
        {
            StateManager.GoToNextState();
        }
    }
}
