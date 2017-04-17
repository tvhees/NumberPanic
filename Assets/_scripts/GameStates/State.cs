using Managers;
using UnityEngine;

namespace GameStates
{
    [ManagerDependency(typeof(ManagerContainer))]
    public class State : BaseMonoBehaviour
    {
        public GameObject Current;

        protected void Start()
        {
            GetManager<StateManager>().SetStateObject(this);
        }
    }
}