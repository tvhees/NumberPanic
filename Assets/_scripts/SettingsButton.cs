using UnityEngine;
using System.Collections;

public class SettingsButton : MonoBehaviour {

    public void ToggleSettings()
    {
        bool open = UIManager.Instance.modePanel.GetComponent<Animator>().GetBool("open");
        UIManager.Instance.modePanel.GetComponent<Animator>().SetBool("open", !open);
    }	
}

