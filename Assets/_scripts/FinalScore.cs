using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalScore : MonoBehaviour {

    public Text label;
    public int current;

    private float counter, countTime;

    void Awake() {
        UIManager.Instance.scorePanel = transform.parent.gameObject;
    }

    void OnEnable() {
        current = 0;
        countTime = 1f;
        counter = 0;
    }

    void Update() {
        counter += Time.unscaledDeltaTime;
        current = Mathf.CeilToInt(UIManager.Instance.score.current * Mathf.Clamp01(counter / countTime));
        current = Manager.Instance.game.GetNumber(current);
        label.text = current.ToString();
    }

}
