using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager> {

    [HideInInspector] public HighScore highScore;
    [HideInInspector] public MainMode modes;
    [HideInInspector] public SubMode subModes;
    [HideInInspector] public Score score;
    [HideInInspector] public GameObject gameEndPanel, scorePanel, menuPanel, modePanel;

    private float current;

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

        switch (Manager.Instance.game.state)
        {
            case Game.State.END:
                gameEndPanel.SetActive(true);
                break;
            case Game.State.SCORE:
                score.gameObject.SetActive(false);
                gameEndPanel.SetActive(false);
                scorePanel.SetActive(true);
                menuPanel.SetActive(true);
                break;
            default:
                if(score != null)
                    score.gameObject.SetActive(true);
                gameEndPanel.SetActive(false);
                scorePanel.SetActive(false);
                menuPanel.SetActive(false);
                modePanel.SetActive(false);
                break;
        }
    }

    void UpdateScore()
    {
        current = Manager.current;

        score.Increment();

        if (score.value >= Preferences.highScore)
        {
            Preferences.Instance.UpdateHighScore(score.value);
        }
    }
}
