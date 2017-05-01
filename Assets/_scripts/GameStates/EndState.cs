using Controller;
using Managers;
using RSG;

namespace GameStates
{
    public class EndState : StateBase
    {
        public override IPromise StartState()
        {
            var animationManager = GetManager<AnimManager>();
            return Promise.Sequence(
                base.StartState,
                () => Controller.Game.SetTargetTimeScale(0.0f),
                animationManager.Continue.ScreenEnterAnimation
            );
        }

        public override IPromise FinishState()
        {
            return Promise.Sequence(
                base.StartState,
                () => Controller.Game.SetTargetTimeScale(1.0f),
                GetManager<AnimManager>().Continue.ScreenExitAnimation
            );
        }
    }
}