using System.Collections;
using Controller;
using UnityEngine;

namespace View
{
    public class ScorePanel : MonoBehaviour {
        [SerializeField] private Animator anim;
        private bool animating;

        public IEnumerator Drop()
        {
            animating = true;
            anim.SetTrigger("drop");

            while (animating)
                yield return null;
        }

        public void Callback()
        {
            animating = false;
        }
    }
}
