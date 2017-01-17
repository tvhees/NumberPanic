using UnityEngine;

namespace View
{
    public class FinalLetterCallback : MonoBehaviour {

        TitleAnimator titleAnim;

        void Awake()
        {
            titleAnim = GetComponentInParent<TitleAnimator>();
        }

        public void Callback()
        {
            titleAnim.Callback();
        }
    }
}
