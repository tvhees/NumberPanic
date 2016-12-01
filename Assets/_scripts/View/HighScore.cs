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

    public void ChangeText(FaceValue fV)
    {
        text.text = fV.text;
    }
}
