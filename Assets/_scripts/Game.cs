using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : ScriptableObject {

    [HideInInspector] public int oldHS;
    [HideInInspector] public float buffer;
    public enum State
    {
        TITLE,
        ATTRACT,
        PLAY,
        CRITICAL,
        END,
        SCORE
    }
    [HideInInspector] public State state;

    // Timers for end-game continuation and critical recovery
    [HideInInspector] public float timer;
    private float endMax, critMax, scaleTimer;
    private float[] timeScales;
    private int continuesLeft;

    void OnEnable()
    {
        //if (Manager.Instance.debugMode)
            Debug.Log("Instance of Game created");
        NewTimeScale(1.0f);
        critMax = 3.0f;
        endMax = 5.0f;
        continuesLeft = 1;
        state = State.ATTRACT;
        oldHS = Preferences.Instance.GetHighScore().value;
    }

    // Called by Manager.Update every frame because scriptable objects don't get Update calls
    // Timer is reset along with state change. Uses unscaledDeltaTime to prevent inflation from
    // slowmotion effects.
    public void RunTimers()
    {
        if (state == State.CRITICAL || state == State.END)
        {
            timer += Time.unscaledDeltaTime;
        }

        if (state == State.END && timer > endMax)
        {
            if (Manager.Instance.debugMode)
                Debug.Log("Continue Timer Ran Out");
            ShowScore();

        }
        else if (state == State.CRITICAL && timer > critMax)
        {
            if(Manager.Instance.debugMode)
                Debug.Log("Critical Timer Ran Out");

            EndGame();
        }

        // Now we smoothly move towards the appropriate timescale
        scaleTimer += Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Lerp(timeScales[0], timeScales[1], scaleTimer);
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

    public FaceValue GetFaceValue(int current, Manager.Mode mode, int subValue)
    {
        FaceValue fV = new FaceValue();
        int[] intArray = null;
        string[] stringArray = null;
        switch (mode)
        {
            case Manager.Mode.linear:
                fV.value = current * (subValue + 1);
                fV.text = fV.value.ToString();
                break;
            case Manager.Mode.power:
                fV.value = (int)Mathf.Pow(current, (subValue + 2));
                fV.text = fV.value.ToString();
                break;
            case Manager.Mode.sequence:
                switch (subValue)
                {
                    case (int)Manager.Sequence.primes:
                        intArray = Manager.Instance.data.numberArrays.primes;
                        fV.value = intArray[(int)Mathf.Repeat(current, intArray.Length)];
                        fV.text = fV.value.ToString();
                        break;
                    case (int)Manager.Sequence.fibbonaci:
                        intArray = Manager.Instance.data.numberArrays.fibbonaci;
                        fV.value = intArray[(int)Mathf.Repeat(current, intArray.Length)];
                        fV.text = fV.value.ToString();
                        break;
                }
                break;
            case Manager.Mode.english:
                switch (subValue)
                {
                    case (int)Manager.English.common:
                        stringArray = Manager.Instance.data.textArrays.englishWords;
                        fV.value = current;
                        fV.text = stringArray[(int)Mathf.Repeat(current, stringArray.Length)];
                        break;
                    case (int)Manager.English.aus_anthem:
                        stringArray = Manager.Instance.data.textArrays.ausAnthem;
                        fV.value = current;
                        fV.text = stringArray[(int)Mathf.Repeat(current, stringArray.Length)];
                        break;
                }
                break;
        }
        return fV;
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
        Play();
        Manager.current++;

        if (GetFaceValue(value, Manager.mode, Manager.subValue).value == oldHS)
            return Color.green;
        else
            return Color.yellow;
    }

    public void Play()
    {
        if (Manager.Instance.debugMode)
            Debug.Log("State: Play");
        state = State.PLAY;
        NewTimeScale(1.0f);
    }

    void Critical()
    {
        timer = 0;
        state = State.CRITICAL;
        NewTimeScale(0.2f);
    }

    void EndGame()
    {
        if (continuesLeft > 0)
        {
            continuesLeft--;
            timer = 0;
            state = State.END;
        }
        else
            ShowScore();
    }

    void ShowScore()
    {
        NewTimeScale(1.0f);
        state = State.SCORE;
    }

    void OnDisable()
    {
        if (Manager.Instance != null)
        {
            if (Manager.Instance.debugMode)
                Debug.Log("Instance of Game destroyed");

            state = State.END;
        }
    }

    void NewTimeScale(float newScale) {
        scaleTimer = 0f;
        timeScales = new float[2] { Time.timeScale, newScale };
    }
}

public class FaceValue
{
    public int value = 0;
    public string text = null;
}
