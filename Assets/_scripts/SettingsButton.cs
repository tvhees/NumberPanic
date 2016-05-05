using UnityEngine;
using System.Collections;

public class SettingsButton : MonoBehaviour {

    public void ToggleSettings() {
        bool toggle = UIManager.Instance.modePanel.activeSelf;
        UIManager.Instance.modePanel.SetActive(!toggle);
    }
	
}

