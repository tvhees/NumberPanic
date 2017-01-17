using Controller;
using UnityEngine;

namespace View
{
    public class SubPanel : MonoBehaviour {

        [SerializeField] private Animator anim;

        public void TogglePanel()
        {
            var open = anim.GetBool("open");
            AnimationManager.Instance.CloseSubPanels();

            if (!open)
                anim.SetBool("open", true);
        }

        public void ClosePanel()
        {
            anim.SetBool("open", false);
        }
    }
}
