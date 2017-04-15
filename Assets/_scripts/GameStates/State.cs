using Managers;
using UnityEngine;

namespace GameStates
{
    [ManagerDependency(typeof(ManagerContainer))]
    public class State : BaseMonoBehaviour
    {
        public GameObject Current;

        protected override void Awake()
        {
            base.Awake();
            GetManager<StateManager>().SetStateObject(this);
        }
    }
}