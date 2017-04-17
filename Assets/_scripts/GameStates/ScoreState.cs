using Managers;
using RSG;

namespace GameStates
{
    public class ScoreState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(() => base.StartState(),
                GetManager<AnimManager>().Score.ScreenEnterAnimation
            );
        }
    }
}