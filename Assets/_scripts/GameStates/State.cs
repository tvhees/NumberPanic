using System;
using Managers;
using UnityEngine;

namespace GameStates
{
    [ManagerDependency(typeof(ManagerContainer))]
    public class State : BaseMonoBehaviour
    {
        public GameObject Current;
        private StateController stateController;

        protected override void Awake()
        {
            base.Awake();
            stateController = GetManager<StateController>();
            stateController.SetStateObject(this);
        }

        public void SetState(GameObject newState)
        {
            stateController.MoveToState(newState);
        }
    }
}