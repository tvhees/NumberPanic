using Controller;
using Managers;
using UnityEngine;

namespace View
{
    public class PanelButton : BaseMonoBehaviour {

        public void TogglePanel()
        {
            ((SubPanel)GetManager<AnimManager>().SubMenus[0]).TogglePanel();
        }
    }
}

