using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager> {

    [HideInInspector] public Title title;
    [HideInInspector] public HighScore highScore;
    [HideInInspector] public MainMode modes;
    [HideInInspector] public SubMode subModes;
    [HideInInspector] public Score score;
    [HideInInspector] public GameObject gameEndPanel, scorePanel, menuPanel;

    private float current;

    void Awake()
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
                scorePanel.SetActive(false);
                menuPanel.SetActive(true);
                break;
            case Game.State.SCORE:
                gameEndPanel.SetActive(false);
                scorePanel.SetActive(true);
                menuPanel.SetActive(true);
                break;
            default:
                gameEndPanel.SetActive(false);
                scorePanel.SetActive(false);
                menuPanel.SetActive(false);
                break;
        }
    }

    void UpdateScore()
    {
        current = Manager.current;

        score.Increment();

        if (score.value >= Preferences.highScore)
        {
            highScore.ChangeText(score.value);
            Preferences.Instance.UpdateHighScore(score.value);
        }
    }

    public void FadeUI(bool visible) {
        title.Fade();
        highScore.Fade();
        modes.Fade(visible);
        subModes.Fade(visible);
    }
}
