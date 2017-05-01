using System;
using Controller;
using GameData;
using UnityEngine;

namespace GameData
{
    public partial class Settings
    {
        [SerializeField] private GameTimer.TimerSettings timerSettings;
        public static GameTimer.TimerSettings TimerSettings { get { return instance.timerSettings; }}
    }
}

namespace Controller
{
    public class GameTimer 
    {
        [Serializable]
        public class TimerSettings
        {
            public float StartingTime;
            public float TimeBonus;
            public float TimePenalty;
        }

        private float remainingTime;

        public GameTimer()
        {
            remainingTime = Settings.TimerSettings.StartingTime;
        }

        public void AddTimeBonus()
        {
            remainingTime += Settings.TimerSettings.TimeBonus;
        }

        public void AddTimePenalty()
        {
            remainingTime = Mathf.Max(0f, remainingTime - Settings.TimerSettings.TimePenalty);
        }

        public float UpdateTimer(float delta)
        {
            remainingTime = Mathf.Max(0f, remainingTime + delta);

            if (remainingTime <= Mathf.Epsilon)
                MainManager.Instance.game.ProcessGameLoss();

            return remainingTime;
        }

    }
}