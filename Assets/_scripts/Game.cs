using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : ScriptableObject {

    [HideInInspector] public int oldHS;
    [HideInInspector] public float buffer;
    public enum State
    {
        ATTRACT,
        PLAY,
        CRITICAL,
        END,
        SCORE
    }
    [HideInInspector] public State state;

    // Store references to loaded gameobjects here
    [HideInInspector] public Spawner spawner;

    // Timers for end-game continuation and critical recovery
    [HideInInspector] public float timer;
    private float endMax, critMax;

    void OnEnable() {
        critMax = 3.0f;
        endMax = 5.0f;
        state = State.ATTRACT;
        oldHS = Preferences.highScore;
    }

    // Called by Manager.Update every frame because scriptable objects don't get Update calls
    // Timer is reset along with state change. Uses unscaledDeltaTime to prevent inflation from
    // slowmotion effects.
    public void RunTimers() {
        if (state == State.CRITICAL || state == State.END)
        {
            timer += Time.unscaledDeltaTime;
        }

        if (state == State.END && timer > endMax)
            ShowScore();
        else if (state == State.CRITICAL && timer > critMax)
            EndGame();
    }

    // Calculates the time left for UI purposes
    public float TimeRemaining() {
        int t = 0;
        float max = 0f;

        if (state == State.CRITICAL)
            max = critMax;
        else if (state == State.END)
            max = endMax;

        t = Mathf.CeilToInt(Mathf.Max(max - timer, 0));
        return t;
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
                value = (int)Mathf.Pow(current, (Manager.subValue + 2));
                break;
            case Manager.Mode.sequence:
                switch (Manager.subValue)
                {
                    case (int)Manager.Sequence.primes:
                        value = Manager.Instance.data.numberArrays.primes[current];
                        break;
                    case (int)Manager.Sequence.fibbonaci:
                        value = Manager.Instance.data.numberArrays.fibbonaci[current];
                        break;
                }
                break;
        }
        return value;
    }

    public Color ResolveNumber(int value, bool touched = false)
    {
        if (touched)
        {
            if (value == Manager.current) // if we should have touched the number
            {
                return Progress(value);
            }
            else // if we shouldn't have touched it!
            {
                return BadTouch();
            }
        }
        else // if the number has reached the bottom of the screen
        {
            if (value == Manager.current && Manager.current > 0) // if we should have touched this number
            {
                return BadTouch();
            }
            else // if it's a non-important number
                return Color.white;
        }
    }

    Color BadTouch() {
        if (state == State.PLAY)
            Critical();
        else if (state == State.CRITICAL)
            EndGame();

        return Color.red;
    }

    Color Progress(int value)
    {
        if (state != State.END)
        {
            Play();
            Manager.current++;

            if (value == oldHS)
                return Color.green;
            else
                return Color.yellow;
        }
        else
            return Color.white;
    }

    public void Play()
    {
        state = State.PLAY;
        Time.timeScale = 1.0f;
    }

    void Critical()
    {
        timer = 0;
        state = State.CRITICAL;
        Time.timeScale = 0.2f;
    }

    void EndGame()
    {
        Time.timeScale = 0;
        timer = 0;
        state = State.END;
    }

    void ShowScore()
    {
        state = State.SCORE;
    }

    void OnDestroy() {
        state = State.END;
    }
}
