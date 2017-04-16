using Managers;
using RSG;

namespace GameStates
{
    public class AttractState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(() => base.StartState(),
                () => GetManager<AnimManager>().Loading.ScreenExitAnimation());
        }
    }
}