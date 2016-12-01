using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
    [HideInInspector] public FaceValue fV;

    private Text display;
    private int current;

    void Awake() {
        display = GetComponent<Text>();
        UIManager.Instance.score = this;
        UpdateDisplay();
    }

    public void UpdateDisplay() {
        fV = Manager.Instance.game.GetFaceValue(current);
        display.text = fV.text;
    }

    public void Increment()
    {
        current++;
        UpdateDisplay();
        GetComponent<Animator>().SetTrigger("expand");
    }
}
