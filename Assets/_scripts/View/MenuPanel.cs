using System.Collections;
using Controller;
using UnityEngine;
using UnityEngine.Events;

namespace View
{
    public class MenuPanel : MonoBehaviour
    {
        public static UnityEvent OnFinishedAnimation = new UnityEvent();
        public bool Animating { get; private set; }
        [SerializeField] private Animator anim;

        private void Awake()
        {
            Animating = true;
        }

        public IEnumerator DropMenu()
        {
            Animating = true;
            anim.SetTrigger("drop");

            while (Animating)
                yield return null;
        }

        public void MenuCallback()
        {
            Animating = false;
            OnFinishedAnimation.Invoke();
        }
    }
}
