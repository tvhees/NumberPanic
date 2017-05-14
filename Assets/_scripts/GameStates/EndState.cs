using System.Threading;
using Controller;
using Managers;
using RSG;
using UnityEngine;

namespace GameStates
{
    public class EndState : StateBase
    {
        private float countdownTimer;
        private bool runTimer;
        private const float MaxCountdownTime = 5.0f;

        /// <summary>
        /// Returns the current game timer
        /// </summary>
        public float TimeRemaining
        {
            get
            {
                return Mathf.CeilToInt(Mathf.Max(MaxCountdownTime - countdownTimer, 0));
            }
        }

        public override IPromise StartState()
        {
            var animationManager = GetManager<AnimManager>();
            return Promise.Sequence(
                base.StartState,
                () => Controller.Game.SetTargetTimeScale(0.0f),
                animationManager.Continue.ScreenEnterAnimation,
                StartTimer
            );
        }

        public override IPromise FinishState()
        {
            return Promise.Sequence(
                StopTimer,
                () => Controller.Game.SetTargetTimeScale(1.0f),
                GetManager<AnimManager>().Continue.ScreenExitAnimation,
                base.FinishState
            );
        }

        private IPromise StartTimer()
        {
            return new Promise((resolve, reject) =>
            {
                countdownTimer = 0f;
                runTimer = true;
                resolve();
            });
        }

        private IPromise StopTimer()
        {
            return new Promise((resolve, reject) =>
            {
                runTimer = false;
                resolve();
            });
        }

        private void Update()
        {
            if (runTimer)
            {
                countdownTimer += Time.unscaledDeltaTime;
            }

            if (countdownTimer > MaxCountdownTime)
            {
                StateManager.MoveTo(States.Score);
            }
        }
    }
}