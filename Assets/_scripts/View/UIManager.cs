using Controller;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace View
{
    public class UiManager : Singleton<UiManager> {

        public PanelButton settingsButton;

        [HideInInspector] public Score score;
        [HideInInspector] public HighScore highScore;
        [HideInInspector] public TimerDisplay timerDisplay;
        public MainMode modes;
        public SubMode subModes;
        public GameObject continuePanel;
        public TutorialPanel tutorialPanel;
        public GameObject tutorialArrow;
        [SerializeField] private Toggle tutorialToggle;
        [SerializeField] private Toggle shakeToggle;

        private float current;

        private void Start()
        {
            current = MainManager.Current;
        }

        public void Update() {
            if (MainManager.Instance.GameTimer != null)
            {
                var timerIsStopped = MainManager.Instance.StateManager.CurrentStateIs(States.Title, States.Load, States.Attract, States.Pause);
                var delta = timerIsStopped ? 0 : -Time.unscaledDeltaTime;
                var timeRemaining = MainManager.Instance.GameTimer.UpdateTimer(delta);
                UpdateTimer(timeRemaining);
            }

            if (MainManager.Current > current)
            {
                UpdateScore();
            }

            if (current > MainManager.Current)
            {
                current = MainManager.Current;
            }
        }

        private void UpdateScore()
        {
            current = MainManager.Current;

            score.Increment();

            if (score.Fv.Value >= Preferences.HighScore.Value)
            {
                Preferences.Instance.UpdateHighScore(score.Fv);
            }
        }

        public void UpdateTimer(float remainingTime)
        {
            var remainingTimeString = string.Format("{0:0.0}", remainingTime);
            timerDisplay.SetRemainingTime(remainingTime, remainingTimeString);
        }

        public void SetTutorialToggle(bool isOn)
        {
            tutorialToggle.isOn = isOn;
        }

        public void SetShakeToggle(bool isOn)
        {
            shakeToggle.isOn = isOn;
        }

        public void ToggleTutorial(Toggle toggle)
        {
            Preferences.Instance.ToggleTutorial(toggle.isOn);
        }

        public void ToggleShake(Toggle toggle)
        {
            Preferences.Instance.ToggleShake(toggle.isOn);
        }
    }
}
