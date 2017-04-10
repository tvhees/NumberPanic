using System.Linq;
using GameStates;
using UnityEngine;

namespace Managers
{
    public enum States { Title, Attract, Pause, Play, Critical, End, Score }

    [ManagerAlwaysGlobal]
    [CreateAssetMenu(fileName = "StateManager.asset", menuName = "Manager/State")]
    public class StateManager : Manager
    {
        private State stateObject;
        private StateBase current;
        private StateBase[] allStates;

        public bool CurrentStateIs(States queryState)
        {
            return current == allStates[(int) queryState];
        }

        public void SetStateObject(State stateObject)
        {
            this.stateObject = stateObject;
            allStates = stateObject.GetComponentsInChildren<StateBase>(true);
            MoveToState(allStates.First());
        }

        public void GoToNextState()
        {
            var currentIndex = current.transform.GetSiblingIndex();
            var newIndex = (int)Mathf.Repeat(currentIndex + 1, stateObject.transform.childCount);
            MoveToState(allStates[newIndex]);
        }

        public void MoveToState(States newState)
        {
            MoveToState(allStates[(int)newState]);
        }

        public void MoveToState(GameObject newState)
        {
            MoveToState(newState.GetComponent<StateBase>());
        }

        private void MoveToState(StateBase newState)
        {
            if(current) { current.EndState(); }
            current = newState;
            stateObject.Current = newState.gameObject;
            current.gameObject.SetActive(true);
            current.StartState();
        }
    }
}