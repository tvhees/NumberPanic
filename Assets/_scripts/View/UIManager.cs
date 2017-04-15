using System.Collections;
using Controller;
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
        public GameObject scorePanel;
        public GameObject menuPanel;
        public TutorialPanel tutorialPanel;
        public GameObject tutorialArrow;
        [SerializeField] private Toggle tutorialToggle;
        [SerializeField] private Toggle shakeToggle;
        private Game.State gameState;

        private float current;

        private void Start()
        {
            MainManager.Instance.ui = this;
            current = MainManager.Current;
            EventManager.OnStateChanged.AddListener((OnStateChanged));
        }

        public void Update() {
            if (MainManager.Instance.GameTimer != null)
            {
                var timerIsRunning = gameState == Game.State.Attract || gameState == Game.State.Pause;
                var delta = timerIsRunning ? 0 : -Time.unscaledDeltaTime;
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

        private void OnStateChanged(Game.State state)
        {
            gameState = state;
            switch (state)
            {
                case Game.State.Attract:
                    if (score != null)
                        score.gameObject.SetActive(true);
                    scorePanel.SetActive(false);
                    continuePanel.SetActive(false);
                    tutorialPanel.gameObject.SetActive((false));
                    break;
                case Game.State.Pause:
                    scorePanel.SetActive(false);
                    menuPanel.SetActive(false);
                    continuePanel.SetActive(false);
                    tutorialPanel.gameObject.SetActive((true));
                    break;
                case Game.State.End:
                    continuePanel.SetActive(true);
                    tutorialPanel.gameObject.SetActive((false));
                    break;
                case Game.State.Score:
                    StartCoroutine(LoadScore());
                    break;
                default:
                    if (score != null)
                        score.gameObject.SetActive(true);
                    scorePanel.SetActive(false);
                    menuPanel.SetActive(false);
                    tutorialPanel.gameObject.SetActive(false);
                    break;
            }
        }

        private IEnumerator LoadScore()
        {
            Preferences.Instance.Save();
            if (continuePanel.activeSelf)
                yield return StartCoroutine(AnimationManager.Instance.ContinueButtonPressed(false));
            scorePanel.SetActive(true);
            menuPanel.SetActive(true);
            settingsButton.TogglePanel();
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
