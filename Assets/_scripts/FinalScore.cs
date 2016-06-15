using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalScore : MonoBehaviour {

    public Text label;
    [HideInInspector] public int current;
    [HideInInspector] public FaceValue fV;

    private float counter, countTime;
    private Manager.Mode mode;
    private int subValue;

    void Awake() {
        UIManager.Instance.scorePanel = transform.parent.gameObject;
    }

    void OnEnable() {
        mode = Manager.mode;
        subValue = Manager.subValue;
        current = 0;
        countTime = 1f;
        counter = 0;
    }

    void Update() {
        counter += Time.unscaledDeltaTime;
        current = Mathf.CeilToInt(Manager.current * Mathf.Clamp01(counter / countTime));
        // Important! We are using STORED values of mode and subValue here
        // This prevents final scores changing if mode choice is update in settings post-game
        fV = Manager.Instance.game.GetFaceValue(current, mode, subValue);
        label.text = fV.text;
    }

}
