using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : ScriptableObject {

    public float buffer;
    public enum State
    {
        ATTRACT,
        PLAY,
        END
    }
    public State state;

    // Store references to loaded gameobjects here
    public Spawner spawner;

    private int oldHS;

    void Awake() {
        state = State.ATTRACT;
        oldHS = Preferences.highScore;
    }

    public int GetNumber(int current)
    {
        int value = 0;
        switch (Manager.mode)
        {
            case Manager.Mode.linear:
                value = current * (Manager.subValue + 1);
                break;
            case Manager.Mode.power:
                value = (int)Mathf.Pow(current, (Manager.subValue + 1));
                break;
            case Manager.Mode.sequence:
                value = Manager.Instance.data.numberArrays.primes[current];
                break;
        }
        return value;
    }

    public Color ResolveNumber(int value, bool touched = false)
    {
        if (touched)
        {
            if (value == Manager.current)
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
            if (value == Manager.current && Manager.current > 0)
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
        state = State.PLAY;
        Manager.current++;
    }

    void EndGame()
    {
        state = State.END;
        Manager.Instance.spawner.spawn = false;
        Time.timeScale = 0.2f;
        Manager.Instance.Restart();
    }

    void OnDestroy() {
        state = State.END;
    }
}
