using Managers;
using RSG;
using UnityEngine;

namespace GameStates
{
    public class TitleState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(
                () => base.StartState(),
                DebugMsg,
                () => GetManager<AnimManager>().Title.ScreenEnterAnimation(),
                () => GetManager<AnimManager>().Menu.ScreenEnterAnimation()
            );
        }

        private IPromise DebugMsg()
        {
            Debug.Log(GetManager<AnimManager>());
            Debug.Log("1");
            Debug.Log(GetManager<AnimManager>().Title);
            Debug.Log("2");
            return Promise.Resolved();
        }

        public override IPromise EndState()
        {
            return Promise.Sequence(
                () => GetManager<AnimManager>().Menu.ScreenExitAnimation(),
                () => GetManager<AnimManager>().Title.ScreenExitAnimation(),
                () => base.EndState()
            );
        }
    }
}