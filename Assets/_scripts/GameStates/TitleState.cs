using Managers;
using RSG;

namespace GameStates
{
    public class TitleState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(
                () => base.StartState(),
                () => GetManager<AnimManager>().Title.ScreenEnterAnimation(),
                () => GetManager<AnimManager>().Menu.ScreenEnterAnimation()
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