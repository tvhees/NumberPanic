using System.Collections;
using Assets._scripts.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._scripts.View
{
    public class MenuPanel : MonoBehaviour
    {

        public static UnityEvent OnFinishedAnimation = new UnityEvent();
        public bool Animating { get; private set; }
        [SerializeField] private Animator anim;

        private void Awake()
        {
            AnimationManager.Instance.menuPanel = this;
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
            Debug.Log("Menu Finished Animating");
            Animating = false;
            OnFinishedAnimation.Invoke();
        }
    }
}
