using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueCountdown : MonoBehaviour {

    public Text label;

    void Update() {
        label.text = Manager.Instance.game.TimeRemaining.ToString();
    }

}
