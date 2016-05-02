using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
    public int current, value;
    public Text display;
    public Spawner spawner;

    void Awake() {
        UIManager.Instance.score = this;
        UpdateDisplay();
        UIManager.Instance.FadeUI(true);
    }

    public void UpdateDisplay() {
        value = Manager.Instance.game.GetNumber(current);
        display.text = value.ToString();
    }

    public void Increment()
    {
        current++;
        UpdateDisplay();
        GetComponent<Animator>().SetTrigger("expand");

        if (current == 1)
        {
            UIManager.Instance.FadeUI(false);
            spawner.leftBound = 0.05f;
        }
    }
}
