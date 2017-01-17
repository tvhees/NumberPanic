using UnityEngine.Events;

namespace Controller
{
    public class GameStateEvent : UnityEvent<Game.State> { }

    public static class EventManager {

        public static GameStateEvent OnStateChanged = new GameStateEvent();
        public static UnityEvent OnDropDownClicked = new UnityEvent();
    }
}