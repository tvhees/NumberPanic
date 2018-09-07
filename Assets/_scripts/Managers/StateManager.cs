using System;
using System.Linq;
using Controller;
using GameStates;
using RSG;
using UnityEngine;

namespace Managers
{
    public enum States { Title, Load, Attract, Pause, Play, End, Score }

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
            // States.End,
            States.Score
        };

        public States Current { get { return stateNums[current.transform.GetSiblingIndex()]; }}

        public bool CurrentStateIs(States queryState)
        {
            return current == stateClasses[(int) queryState];
        }

        public bool CurrentStateIs(params States[] queryStates)
        {
            return queryStates.Any(s => current == stateClasses[(int) s]);
        }

        public void SetStateObject(State stateObject)
        {
            MainManager.Instance.StateManager = this;
            this.stateObject = stateObject;
            stateClasses = stateObject.GetComponentsInChildren<StateBase>(true);
            MoveTo(stateClasses.First());
        }

        public IPromise MoveTo(States newState, bool force = false)
        {
            return MoveTo(stateClasses[(int)newState], force);
        }

        public IPromise MoveTo(GameObject newState, bool force = false)
        {
            return MoveTo(newState.GetComponent<StateBase>(), force);
        }

        private IPromise MoveTo(StateBase newState, bool force = false)
        {
            if (force || current != newState)
            {
                return Promise.Sequence(
                        () => current ? current.FinishState() : Promise.Resolved(),
                        () => SetNewState(newState),
                        () => current.StartState())
                    .Catch(ex => Debug.LogException(ex, this));
            }

            Debug.LogWarning(newState.name + " is already active");
            return Promise.Resolved();
        }

        private IPromise SetNewState(StateBase newState)
        {
            return new Promise((resolve, reject) => {
                try
                {
                    current = newState;
                    stateObject.Current = newState.gameObject;
                    resolve();
                }
                catch (Exception e)
                {
                    reject(e);
                }
            });
        }
    }
}