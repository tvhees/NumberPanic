using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View
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
