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
            return new Promise((resolve, reject) =>
            {
                Settings.OnChanged.AddListener(RestartThisState);
                gameObject.SetActive(true);
                resolve();
            });
        }

        public virtual IPromise FinishState()
        {
            Settings.OnChanged.RemoveListener(RestartThisState);
            gameObject.SetActive(false);
            return Promise.Resolved();
        }

        protected void RestartThisState()
        {
            StateManager.MoveTo(this.gameObject, force: true);
        }
    }
}
