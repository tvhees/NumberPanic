using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using _scripts.Controller;

namespace Assets._scripts.Controller
{
    public class GameStateEvent : UnityEvent<Game.State> { }

    public static class EventManager {

        public static GameStateEvent OnStateChanged = new GameStateEvent();
        public static UnityEvent OnNumberTouched = new UnityEvent();
    }
}