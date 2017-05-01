using RSG;

namespace GameStates
{
    public class CriticalState : StateBase
    {
        public override IPromise StartState()
        {
            return Promise.Sequence(() => base.StartState(),
                () => Controller.Game.SetTargetTimeScale(1.0f)
            );
        }
    }
}