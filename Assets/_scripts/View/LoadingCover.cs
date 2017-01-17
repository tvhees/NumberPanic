using System.Collections;
using Controller;
using UnityEngine;

namespace View
{
    public class LoadingCover : MonoBehaviour {

        public Animator anim;
        private bool animating;

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
