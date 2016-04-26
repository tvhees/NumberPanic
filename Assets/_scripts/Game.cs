using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : ScriptableObject {

    public float buffer;
    public enum State
    {
        PLAY,
        END
    }
    public State state;

    // Store references to loaded gameobjects here
    public Score score;
    public Spawner spawner;

    private int oldHS;

    void Awake() {
        state = State.PLAY;
        oldHS = Preferences.Instance.highScore;
    }

    public Color ResolveNumber(int value, bool touched = false)
    {
        if (touched)
        {
            if (value == score.value)
            {
                Progress();
                if (value == oldHS)
                    return Color.green;
                else
                    return Color.yellow;
            }
            else
            {
                EndGame();
                return Color.red;
            }
        }
        else
        {
            if (value == score.value && value > 0)
            {
                EndGame();
                return Color.red;
            }
            else
                return Color.white;
        }
    }

    void Progress()
    {
        score.Increment();
        spawner.current++;
    }

    void EndGame()
    {
        state = State.END;
        spawner.spawn = false;
        Time.timeScale = 0.2f;
        Manager.Instance.Restart();
    }

    void OnDestroy() {
        state = State.END;
    }
}
