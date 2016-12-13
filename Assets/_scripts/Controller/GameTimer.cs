using Assets._scripts.Controller;
using UnityEngine;

namespace _scripts.Controller
{
    public class GameTimer 
    {
        public const float startingTime = 20.0f;
        public const float timeBonus = 3.0f;
        public const float timePenalty = 6.0f;

        public float remainingTime;

        public GameTimer()
        {
            remainingTime = startingTime;
        }

        public void AddTimeBonus()
        {
            remainingTime += timeBonus;
        }

        public void AddTimeBonus(float inputBonus)
        {
            remainingTime += inputBonus;
        }

        public void AddTimePenalty()
        {
            remainingTime = Mathf.Max(0f, remainingTime - timePenalty);
        }

        public float UpdateTimer(float delta)
        {
            remainingTime = Mathf.Max(0f, remainingTime + delta);

            if (remainingTime <= Mathf.Epsilon)
                Manager.Instance.game.ProcessGameLoss();
            else if (remainingTime <= timePenalty && !Manager.Instance.TimeAttackMode)
                Manager.Instance.game.EnterCriticalState();

            return remainingTime;
        }

    }
}