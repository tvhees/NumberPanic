using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScore : MonoBehaviour {

    private Text text;
    private Animator animator;

    void Awake()
    {
        text = GetComponent<Text>();
        animator = GetComponent<Animator>();
        UIManager.Instance.highScore = this;
    }

    public void Fade()
    {
        animator.SetTrigger("fade");
    }

    public void SwitchHighScore() {
        int hs = Preferences.Instance.GetHighScore();
        if(Manager.Instance.game != null)
            Manager.Instance.game.oldHS = hs;
        ChangeText(hs);
    }

    public void ChangeText(int value)
    {
        text.text = value.ToString();
    }

    public void ChangeText(string value)
    {
        text.text = value;
    }
}
