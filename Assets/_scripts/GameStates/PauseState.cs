using RSG;
using UnityEngine;

namespace GameStates
{
    public class PauseState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(() => base.StartState(),
                () => Controller.Game.SetTargetTimeScale(0.0f)
            )
                .Then(() => Time.timeScale = 0.0f);
        }
    }
}