using Managers;
using RSG;

namespace GameStates
{
    public class PlayState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(() => base.StartState(),
                () => Controller.Game.SetTargetTimeScale(1.0f)
            );
        }
    }
}