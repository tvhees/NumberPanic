using GameData;
using Managers;
using RSG;
using UnityEngine;

namespace GameStates
{
    [ManagerDependency(typeof(ManagerContainer))]
    public abstract class StateBase : BaseMonoBehaviour
    {
        protected GameManager Game;
        protected StateManager StateManager;

        protected override void Awake()
        {
            Game = GetManager<GameManager>();
            StateManager = GetManager<StateManager>();
        }

        public virtual IPromise StartState()
        {
            Settings.OnChanged.AddListener(RestartThisState);
            gameObject.SetActive(true);
            Debug.Log("Starting state:" + name);
            return Promise.Resolved();
        }

        public virtual IPromise EndState()
        {
            Debug.Log("Ending state:" + name);
            Settings.OnChanged.RemoveListener(RestartThisState);
            gameObject.SetActive(false);
            return Promise.Resolved();
        }

        protected void RestartThisState()
        {
            StateManager.MoveToState(this.gameObject);
        }
    }
}
