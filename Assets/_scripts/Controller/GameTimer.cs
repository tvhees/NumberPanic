using UnityEngine;

namespace Controller
{
    public class GameTimer 
    {
        public const float StartingTime = 20.0f;
        public const float TimeBonus = 3.0f;
        public const float TimePenalty = 6.0f;

        private float remainingTime;

        public GameTimer()
        {
            remainingTime = StartingTime;
        }

        public void AddTimeBonus()
        {
            remainingTime += TimeBonus;
        }

        public void AddTimeBonus(float inputBonus)
        {
            remainingTime += inputBonus;
        }

        public void AddTimePenalty()
        {
            remainingTime = Mathf.Max(0f, remainingTime - TimePenalty);
        }

        public float UpdateTimer(float delta)
        {
            remainingTime = Mathf.Max(0f, remainingTime + delta);

            if (remainingTime <= Mathf.Epsilon)
                MainManager.Instance.game.ProcessGameLoss();
            else if (remainingTime <= TimePenalty && !MainManager.Instance.TimeAttackMode)
                MainManager.Instance.game.EnterCriticalState();

            return remainingTime;
        }

    }
}