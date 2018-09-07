using Controller;
using Managers;
using RSG;

namespace GameStates
{
    public class LoadState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(() => base.StartState(),
                MainManager.Instance.NewGameTimer,
                GetManager<AnimManager>().Loading.ScreenExitAnimation,
                () => StateManager.MoveTo(States.Attract)
            );
        }
    }
}