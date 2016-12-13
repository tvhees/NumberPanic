using Assets._scripts.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class NewGameButton : MonoBehaviour {

        public static UnityEvent OnButtonPressed = new UnityEvent();
        public Button button;

        private void OnEnable()
        {
            button.interactable = true;
        }

        public void NewGame()
        {
            OnButtonPressed.Invoke();
        }
    }
}
