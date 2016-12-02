using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class NewGameButton : MonoBehaviour {

        public Button button;

        void Awake() {
            UiManager.Instance.menuPanel = transform.parent.gameObject;
        }

        void OnEnable()
        {
            button.interactable = true;
        }

        public void NewGame()
        {
            StartCoroutine(AnimationManager.Instance.NewGame());
        }
    }
}
