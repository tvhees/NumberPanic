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
            Debug.Log(name);
            gameObject.SetActive(true);
            return Promise.Resolved();
        }

        public virtual IPromise EndState()
        {
            gameObject.SetActive(false);
            return Promise.Resolved();
        }
    }
}
