using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : Singleton<UIManager> {

    // Score displays
    [HideInInspector] public Score score;
    [HideInInspector] public HighScore highScore;

    // Game mode dropdowns
    [HideInInspector] public MainMode modes;
    [HideInInspector] public SubMode subModes;

    // UI panels
    [HideInInspector] public GameObject continuePanel;
    [HideInInspector] public GameObject scorePanel;
    [HideInInspector] public GameObject menuPanel;

    // UI Buttons
    [HideInInspector]
    public Button noAdsButton;

    private float current;
    private Game.State lastState;

    void Start()
    {
        Manager.Instance.ui = this;
        current = Manager.current;
    }

    void Update() {
        if (Manager.current > current)
        {
            UpdateScore();
        }

        if (current > Manager.current)
        {
            current = Manager.current;
        }

        if (Manager.Instance.game.state != lastState)
        {
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
    }

    IEnumerator LoadScore()
    {
        if(continuePanel.activeSelf)
            yield return StartCoroutine(AnimationManager.Instance.Continue(false));
        scorePanel.SetActive(true);
        menuPanel.SetActive(true);
    }

    void UpdateScore()
    {
        current = Manager.current;

        score.Increment();

        if (score.fV.value >= Preferences.highScore.value)
        {
            Preferences.Instance.UpdateHighScore(score.fV);
        }
    }

    public void Reset()
    {
        Preferences.Instance.Reset();
    }
}
