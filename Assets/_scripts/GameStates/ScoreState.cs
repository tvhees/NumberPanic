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
                () => Controller.Game.SetTargetTimeScale(1.0f),
                GetManager<AnimManager>().Score.ScreenEnterAnimation,
                GetManager<AnimManager>().Menu.ScreenEnterAnimation
            );
        }

        public override IPromise FinishState()
        {
            return GetManager<AnimManager>().SubMenus[0].ScreenExitAnimation()
            .Then(
                () => Promise.All(
                    GetManager<AnimManager>().Menu.ScreenExitAnimation(),
                    GetManager<AnimManager>().Score.ScreenExitAnimation()
                ))
            .Then(() => base.FinishState());
        }
    }
}