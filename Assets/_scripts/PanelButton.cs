using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelButton : MonoBehaviour {
    
    public Animator panelAnim;

    public void TogglePanel()
    {
        bool open = panelAnim.GetBool("open");
        AnimationManager.Instance.CloseSubPanels();

        if(!open)
            panelAnim.SetBool("open", true);
    }	
}

