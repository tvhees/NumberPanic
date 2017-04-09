using System.Linq;
using GameStates;
using Managers;
using UnityEngine;

namespace Managers
{
    public enum States { ResetGame, NewGame, Cleanup, WaitingForPlayer, Door, Prophecy, CompleteProphecy, Nightmare, ProcessingTurn, CheckingWinLoss, GameWon, GameLost }

    [CreateAssetMenu(fileName = "StateController.asset", menuName = "Controllers/State")]
    public class StateController : Manager
    {
        private State stateObject;
        private StateBase current;
        private StateBase[] allStates;
        private GameMan gameController;

        public bool IsInCleanup { get { return current == allStates[(int)States.Cleanup]; } }
        public bool IsInProphecy { get { return current == allStates[(int) States.Prophecy]; } }

        public void SetStateObject(State stateObject)
        {
            this.stateObject = stateObject;
            allStates = stateObject.GetComponentsInChildren<StateBase>(true);
            current = allStates.First();
        }

        public void SetGameController(GameMan gameController)
        {
            this.gameController = gameController;
        }

        public void MoveToNext()
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
            current.EndState();
            current = newState;
            stateObject.Current = newState.gameObject;
            current.gameObject.SetActive(true);
            current.StartState();
        }
    }
}