using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game {
    public enum State
    {
        TITLE,
        ATTRACT,
        PLAY,
        CRITICAL,
        END,
        SCORE
    }
    #region Settings

    const float endMax = 5.0f;
    const float maximumCriticalTime = 3.0f;

    #endregion Settings

    #region Variables

    [HideInInspector]
    public State state = State.ATTRACT;

    [HideInInspector]
    public int oldHS;

    // Timers for end-game continuation and critical recovery
    float timer;

    float targetTimeScale;
    int continuesLeft = 1;
    public Manager.Mode mode;
    public int subMode;

    #endregion Variables

    public Game(Manager.Mode mode, int subMode)
    {
        this.mode = mode;
        this.subMode = subMode;
        targetTimeScale = 1.0f;
        oldHS = Preferences.Instance.GetHighScore().value;
    }

    #region Game state

    public void ProcessState()
    {
        if (state == State.CRITICAL || state == State.END)
            timer += Time.unscaledDeltaTime;

        if (state == State.END && timer > endMax)
            Score();
        else if (state == State.CRITICAL && timer > maximumCriticalTime)
            OnTimerCompleted();

        // Now we smoothly move towards the appropriate timescale
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime);
    }

    /// <summary>
    /// Change state to PLAY and set time scale to 1;
    /// </summary>
    public void Play()
    {
        targetTimeScale = 1.0f;
        state = State.PLAY;
    }

    /// <summary>
    /// Set the state to CRITICAL and start the timer;
    /// </summary>
    public void Critical()
    {
        if (state != State.CRITICAL)
        {
            timer = 0;
            targetTimeScale = 1.0f;
            state = State.CRITICAL;
        }
    }

    /// <summary>
    /// Set game state to END
    /// </summary>
    public void End()
    {
        state = State.END;
    }

    /// <summary>
    /// Set game state to SCORE
    /// </summary>
    void Score()
    {
        targetTimeScale = 1.0f;
        state = State.SCORE;
    }

    #endregion Game state

    #region Timers

    /// <summary>
    /// Returns the current game timer
    /// </summary>
    public float TimeRemaining
    {
        get
        {
            var max = state == State.CRITICAL ? maximumCriticalTime : endMax;
            return Mathf.CeilToInt(Mathf.Max(maximumCriticalTime - timer, 0));
        }
    }

    /// <summary>
    /// Removes a continue if the player has any left, otherwise moves the game to the score screen
    /// </summary>
    public void OnTimerCompleted()
    {
        if (state != State.END)
        {
            if (continuesLeft > 0)
            {
                continuesLeft--;
                timer = 0;
                state = State.END;
            }
            else
                Score();
        }
    }

    /// <summary>
    /// Sets game time scale
    /// </summary>
    void SetTimeScaleTo(float newScale)
    {
        targetTimeScale = newScale;
    }
    #endregion Timers

    #region Resolving number touches

    public FaceValue GetFaceValue(int current)
    {
        FaceValue fV = new FaceValue();
        int[] intArray = null;
        string[] stringArray = null;
        switch (mode)
        {
            case Manager.Mode.linear:
                fV.value = current * (subMode + 1);
                fV.text = fV.value.ToString();
                break;
            case Manager.Mode.power:
                fV.value = (int)Mathf.Pow(current, (subMode + 2));
                fV.text = fV.value.ToString();
                break;
            case Manager.Mode.sequence:
                switch (subMode)
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
                switch (subMode)
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
        var fV = GetFaceValue(value);
        if (touched)
        {
            if (IsCurrentValue(fV)) // if we should have touched the number
            {
                return Progress();
            }
            else // if we shouldn't have touched it!
            {
                return BadTouch();
            }
        }
        else // if the number has reached the bottom of the screen
        {
            if (IsCurrentValue(fV) && Manager.current > 0) // if we should have touched this number
            {
                return BadTouch();
            }
            else // if it's a non-important number
                return Color.white;
        }
    }

    bool IsCurrentValue(FaceValue value)
    {
        return value.value == Manager.current || value.text == GetFaceValue(Manager.current).text;
    }

    Color BadTouch() {
        if (Manager.Instance.timeAttackMode) // In time attack mode, mistakes make you lose time
        {
            if (state == State.PLAY)
                Manager.Instance.gameTimer.AddTimePenalty();
        }
        else
        {
            if (state == State.PLAY)
                Critical(); // In classic mode, mistakes take you straight to critical time
            else if (state == State.CRITICAL)
                OnTimerCompleted();
        }
        return Color.red;
    }

    Color Progress()
    {
        Play();
        Manager.current++;
        if(Manager.Instance.timeAttackMode)
            Manager.Instance.gameTimer.AddTimeBonus();

        if (GetFaceValue(Manager.current).value > oldHS)
            return Color.green;
        else
            return Color.yellow;
    }
    #endregion Resolving number touches
}

public class FaceValue
{
    public int value;
    public string text;
}
