using System.Linq;
using GameStates;
using RSG;
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
        private StateBase[] stateClasses;
        private readonly States[] stateNums =
        {
            States.Title,
            States.Attract,
            States.Pause,
            States.Play,
            States.Critical,
            States.End,
            States.Score
        };

        public States Current { get { return stateNums[current.transform.GetSiblingIndex()]; }}

        public bool CurrentStateIs(States queryState)
        {
            return current == stateClasses[(int) queryState];
        }

        public void SetStateObject(State stateObject)
        {
            this.stateObject = stateObject;
            stateClasses = stateObject.GetComponentsInChildren<StateBase>(true);
            MoveToState(stateClasses.First());
        }

        public IPromise MoveToState(States newState)
        {
            return MoveToState(stateClasses[(int)newState]);
        }

        public IPromise MoveToState(GameObject newState)
        {
            return MoveToState(newState.GetComponent<StateBase>());
        }

        private IPromise MoveToState(StateBase newState)
        {
            return Promise.Sequence(
                () => current ? current.EndState() : Promise.Resolved(),
                () => SetNewState(newState),
                () => current.StartState());
        }

        private IPromise SetNewState(StateBase newState)
        {
            current = newState;
            stateObject.Current = newState.gameObject;
            Debug.Log("Setting state:" + newState.name);
            return Promise.Resolved();
        }
    }
}