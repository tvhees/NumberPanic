using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager> {

    [HideInInspector] public Title title;
    [HideInInspector] public HighScore highScore;
    [HideInInspector] public MainMode modes;
    [HideInInspector] public SubMode subModes;
    [HideInInspector] public Score score;
    [HideInInspector] public GameObject gameEndPanel, scorePanel;

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

        if (Manager.Instance.game.state == Game.State.END)
        {
            gameEndPanel.SetActive(true);
        }
        else
        {
            gameEndPanel.SetActive(false);
        }

        if (Manager.Instance.game.state == Game.State.SCORE)
            scorePanel.SetActive(true);
        else
            scorePanel.SetActive(false);
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
