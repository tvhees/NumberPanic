using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class UiManager : Singleton<UiManager> {

        public PanelButton settingsButton;

        // Score displays
        [HideInInspector] public Score score;
        [HideInInspector] public HighScore highScore;

        // Timer display
        [HideInInspector] public TimerDisplay timerDisplay;

        // Game mode dropdowns
        [HideInInspector] public MainMode modes;
        [HideInInspector] public SubMode subModes;

        // UI panels
        [HideInInspector] public GameObject continuePanel;
        [HideInInspector] public GameObject scorePanel;
        [HideInInspector] public GameObject menuPanel;

        private float current;
        private Game.State lastState;

        private void Start()
        {
            Manager.Instance.ui = this;
            current = Manager.Current;
        }

        public void Update() {
            if (Manager.Instance.gameTimer != null)
            {
                var timeRemaining = Manager.Instance.gameTimer.UpdateTimer(-Time.unscaledDeltaTime);
                UpdateTimer(timeRemaining);
            }

            if (Manager.Current > current)
            {
                UpdateScore();
            }

            if (current > Manager.Current)
            {
                current = Manager.Current;
            }

            // Process changes in state here
            if (Manager.Instance.game.state == lastState) return;
            switch (Manager.Instance.game.state)
            {
                case Game.State.TITLE:
                    menuPanel.SetActive(true);
                    break;
                case Game.State.ATTRACT:
                    if (score != null)
                        score.gameObject.SetActive(true);
                    scorePanel.SetActive(false);
                    menuPanel.SetActive(false);
                    continuePanel.SetActive(false);
                    break;
                case Game.State.END:
                    continuePanel.SetActive(true);
                    break;
                case Game.State.SCORE:
                    StartCoroutine(LoadScore());
                    break;
                default:
                    if (score != null)
                        score.gameObject.SetActive(true);
                    scorePanel.SetActive(false);
                    menuPanel.SetActive(false);
                    break;
            }
            lastState = Manager.Instance.game.state;
        }

        private IEnumerator LoadScore()
        {
            Preferences.Instance.Save();
            if (continuePanel.activeSelf)
                yield return StartCoroutine(AnimationManager.Instance.Continue(false));
            scorePanel.SetActive(true);
            menuPanel.SetActive(true);
            settingsButton.TogglePanel();
        }

        private void UpdateScore()
        {
            current = Manager.Current;

            score.Increment();

            if (score.fV.Value >= Preferences.HighScore.Value)
            {
                Preferences.Instance.UpdateHighScore(score.fV);
            }
        }

        public void UpdateTimer(float remainingTime)
        {
            var remainingTimeString = string.Format("{0:0.0}", remainingTime);
            timerDisplay.SetRemainingTime(remainingTime, remainingTimeString);
        }
    }
}
