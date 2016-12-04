using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using _scripts.Controller;

public class GameStateEvent : UnityEvent<Game.State> { }

public static class EventManager {

    public static GameStateEvent onStateChanged = new GameStateEvent();
}
