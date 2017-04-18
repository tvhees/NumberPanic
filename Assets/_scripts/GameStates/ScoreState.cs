using Managers;
using RSG;

namespace GameStates
{
    public class ScoreState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(
                base.StartState,
                GetManager<AnimManager>().Score.ScreenEnterAnimation,
                GetManager<AnimManager>().Menu.ScreenEnterAnimation
            );
        }

        public override IPromise EndState()
        {
            return Promise.Sequence(
                GetManager<AnimManager>().Score.ScreenExitAnimation,
                base.EndState
            );
        }
    }
}