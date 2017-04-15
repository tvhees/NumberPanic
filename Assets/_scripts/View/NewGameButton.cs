using Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace View
{
    public class NewGameButton : BaseMonoBehaviour {

        public static UnityEvent OnButtonPressed = new UnityEvent();
        public Button button;

        private void OnEnable()
        {
            button.interactable = true;
        }

        public void NewGame()
        {
            OnButtonPressed.Invoke();
            GetManager<StateManager>().MoveToState(States.Attract);
        }
    }
}
