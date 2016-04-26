using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
    public int value;
    public Text display;
    public Spawner spawner;

    void Awake() {
        Manager.Instance.game.score = this;
    }

    public void Increment()
    {
        value++;
        Preferences.Instance.UpdateHighScore(value);
        display.text = value.ToString();

        GetComponent<Animator>().SetTrigger("expand");

        if (value == 1)
        {
            Manager.Instance.titleAnimator.SetTrigger("fade");
            spawner.leftBound = 0.05f;
        }
    }
}
