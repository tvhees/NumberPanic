using System.Collections;
using UnityEngine;

namespace View
{
    public class TitleAnimator : MonoBehaviour {

        private bool animating;
        private Animator[] animators;
        private const float Delay = 0.05f;
        private int letters;

        private void Awake()
        {
            animating = false;
            animators = GetComponentsInChildren<Animator>();
        }

        public IEnumerator DropTitle()
        {
            foreach (var animator in animators)
            {
                animator.SetTrigger("drop");
                yield return new WaitForSeconds(Delay);
            }

            yield return new WaitForSeconds(Delay);
        }

        public IEnumerator LeaveTitle()
        {
            animating = true;
            letters = animators.Length;

            foreach (var animator in animators)
            {
                animator.SetTrigger("drop");
            }

            while (animating)
                yield return null;
        }

        public void Callback()
        {
            letters--;
            if(letters <= 0)
                animating = false;
        }
    }
}
