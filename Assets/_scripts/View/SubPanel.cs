using UnityEngine;
using _scripts.Controller;

namespace _scripts.View
{
    public class SubPanel : MonoBehaviour {

        public Animator anim;

        void Awake()
        {
            AnimationManager.Instance.subPanels.Add(this);
        }

        public void TogglePanel()
        {
            bool open = anim.GetBool("open");
            AnimationManager.Instance.CloseSubPanels();

            if (!open)
                anim.SetBool("open", true);
        }

    }
}
