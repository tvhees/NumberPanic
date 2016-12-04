using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
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

        private float current;

        private void Start()
        {
            Manager.Instance.ui = this;
            current = Manager.Current;
            EventManager.onStateChanged.AddListener((OnStateChanged));
        }

        public void Update() {
            if (Manager.Instance.gameTimer != null)
            {
                var delta = Manager.Instance.game.GameState == Game.State.Attract ? 0 : -Time.unscaledDeltaTime;
                var timeRemaining = Manager.Instance.gameTimer.UpdateTimer(delta);
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
        }

        private void OnStateChanged(Game.State state)
        {
            switch (state)
            {
                case Game.State.Title:
                    menuPanel.SetActive(true);
                    break;
                case Game.State.Attract:
                    if (score != null)
                        score.gameObject.SetActive(true);
                    scorePanel.SetActive(false);
                    menuPanel.SetActive(false);
                    continuePanel.SetActive(false);
                    break;
                case Game.State.End:
                    Debug.Log("Set Continue Panel Active");
                    continuePanel.SetActive(true);
                    break;
                case Game.State.Score:
                    StartCoroutine(LoadScore());
                    break;
                default:
                    if (score != null)
                        score.gameObject.SetActive(true);
                    scorePanel.SetActive(false);
                    menuPanel.SetActive(false);
                    break;
            }
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
