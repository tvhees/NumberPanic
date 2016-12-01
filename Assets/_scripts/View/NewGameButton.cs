using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewGameButton : MonoBehaviour {

    public Button button;

    void Awake() {
        UIManager.Instance.menuPanel = transform.parent.gameObject;
    }

    void OnEnable()
    {
        button.interactable = true;
    }

    public void NewGame()
    {
        StartCoroutine(AnimationManager.Instance.NewGame());
    }
}
