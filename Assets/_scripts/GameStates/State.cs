using Managers;
using UnityEngine;

namespace GameStates
{
    [ManagerDependency(typeof(ManagerContainer))]
    public class State : BaseMonoBehaviour
    {
        public GameObject Current;

        private void Start()
        {
            GetManager<StateManager>().SetStateObject(this);
        }
    }
}