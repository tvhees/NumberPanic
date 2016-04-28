using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighScore : MonoBehaviour {

    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
        UIManager.Instance.highScore = this;
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
