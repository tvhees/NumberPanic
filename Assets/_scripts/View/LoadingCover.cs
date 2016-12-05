using System.Collections;
using Assets._scripts.Controller;
using UnityEngine;
using _scripts.Controller;

namespace _scripts.View
{
    public class LoadingCover : MonoBehaviour {

        public Animator anim;
        private bool animating;

        void Awake()
        {
            AnimationManager.Instance.loadingCover = this;
        }

        public IEnumerator Enter()
        {
            animating = true;

            anim.SetBool("open", true);

            while (animating)
                yield return null;
        }

        // Callback function for background animation.
        public void Callback()
        {
            animating = false;
        }
    }
}
