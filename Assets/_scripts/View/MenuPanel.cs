using System.Collections;
using UnityEngine;
using _scripts.Controller;

namespace _scripts.View
{
    public class MenuPanel : MonoBehaviour {

        public Animator anim;
        [HideInInspector] public bool animating;

        void Awake()
        {
            UiManager.Instance.menuPanel = gameObject;
            AnimationManager.Instance.menuPanel = this;
        }

        public IEnumerator DropMenu()
        {
            animating = true;
            anim.SetTrigger("drop");

            while (animating)
                yield return null;
        }

        public void MenuCallback()
        {
            animating = false;
        }
    }
}
