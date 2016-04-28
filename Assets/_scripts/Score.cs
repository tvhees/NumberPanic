using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
    public int current, value;
    public Text display;
    public Spawner spawner;

    void Awake() {
        UIManager.Instance.score = this;
    }

    public void Increment()
    {
        current++;
        value = Manager.Instance.game.GetNumber(current);
        Preferences.Instance.UpdateHighScore(value);
        display.text = value.ToString();

        GetComponent<Animator>().SetTrigger("expand");

        if (current == 1)
        {
            UIManager.Instance.FadeUI();
            spawner.leftBound = 0.05f;
        }
    }
}
