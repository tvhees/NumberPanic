using Managers;
using RSG;
using UnityEngine;

namespace GameStates
{
    public class TitleState : StateBase
    {
        public override IPromise StartState()
        {
            var animationManager = GetManager<AnimManager>();
            return Promise.Sequence(
                base.StartState,
                animationManager.Title.ScreenEnterAnimation,
                animationManager.Menu.ScreenEnterAnimation
            );
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