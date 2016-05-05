using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
    [HideInInspector] public int value;

    private Text display;
    private int current;

    void Awake() {
        display = GetComponent<Text>();
        UIManager.Instance.score = this;
        UpdateDisplay();
    }

    public void UpdateDisplay() {
        value = Manager.Instance.game.GetNumber(current, Manager.mode, Manager.subValue);
        display.text = value.ToString();
    }

    public void Increment()
    {
        current++;
        UpdateDisplay();
        GetComponent<Animator>().SetTrigger("expand");
    }
}
