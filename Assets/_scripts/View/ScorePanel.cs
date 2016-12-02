using System.Collections;
using UnityEngine;
using _scripts.Controller;

namespace _scripts.View
{
    public class ScorePanel : MonoBehaviour {
        public Animator anim;
        private bool animating;

        void Awake() {
            AnimationManager.Instance.scorePanel = this;
        }

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
