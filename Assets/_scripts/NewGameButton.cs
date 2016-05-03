using UnityEngine;
using System.Collections;

public class NewGameButton : MonoBehaviour {

    void Awake() {
        UIManager.Instance.menuPanel = transform.parent.gameObject;
    }

    public void NewGame()
    {
        Manager.Instance.Restart();
    }

}
