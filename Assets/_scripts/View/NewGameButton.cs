using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class NewGameButton : MonoBehaviour {

        public Button button;

        private void OnEnable()
        {
            button.interactable = true;
        }

        public void NewGame()
        {
            StartCoroutine(AnimationManager.Instance.NewGame());
        }
    }
}
