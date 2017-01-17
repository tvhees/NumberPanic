using Controller;
using UnityEngine;

namespace View
{
    public class PanelButton : MonoBehaviour {
    
        public Animator panelAnim;

        public void TogglePanel()
        {
            var open = panelAnim.GetBool("open");
            AnimationManager.Instance.CloseSubPanels();
            if(!open)
                panelAnim.SetBool("open", true);
        }
    }
}

