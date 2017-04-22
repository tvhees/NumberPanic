using System;
using Managers;
using RSG;

namespace GameStates
{
    public class AttractState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(() => base.StartState(),
                () => Controller.Game.SetTargetTimeScale(1.0f),
                () => GetManager<AnimManager>().Loading.ScreenExitAnimation()
            );
        }
    }
}