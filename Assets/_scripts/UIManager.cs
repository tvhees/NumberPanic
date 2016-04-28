using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager> {

    [HideInInspector] public Title title;
    [HideInInspector] public HighScore highScore;
    [HideInInspector] public MainMode modes;
    [HideInInspector] public SubMode subModes;
    [HideInInspector] public Score score;

    private float current;

    void Awake()
    {
        Manager.Instance.ui = this;
        current = Manager.current;
    }

    void Update() {
        if (Manager.current > current)
        {
            if (current == 0)
                FadeUI();

            UpdateScore();
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

    public void FadeUI() {
        title.Fade();
        modes.Fade();
        subModes.Fade();
    }
}
