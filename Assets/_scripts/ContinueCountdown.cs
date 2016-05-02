using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueCountdown : MonoBehaviour {

    public Text label;

    void Awake()
    {
        UIManager.Instance.gameEndPanel = transform.parent.gameObject;   
    }

    void Update() {
        label.text = "CONTINUE: " + Manager.Instance.game.TimeRemaining().ToString();
    }

}
