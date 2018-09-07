using Controller;
using Managers;
using UnityEngine.Events;

namespace View
{
    public class NewGameButton : BaseMonoBehaviour {

        public static UnityEvent OnButtonPressed = new UnityEvent();

        public void NewGame()
        {
            MainManager.Instance.Restart();
            OnButtonPressed.Invoke();
        }
    }
}
