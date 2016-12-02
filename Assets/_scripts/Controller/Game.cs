using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using _scripts.Model;

namespace _scripts.Controller
{
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

        private Data data;
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

        public Game(Data data, Manager.Mode mode, int subMode)
        {
            this.data = data;
            this.mode = mode;
            this.subMode = subMode;
            targetTimeScale = 1.0f;
            oldHS = Preferences.Instance.GetHighScore().Value;
            Debug.Log(oldHS);
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
                case Manager.Mode.Linear:
                    fV.Value = current * (subMode + 1);
                    fV.Text = fV.Value.ToString();
                    break;
                case Manager.Mode.Power:
                    fV.Value = (int)Mathf.Pow(current, (subMode + 2));
                    fV.Text = fV.Value.ToString();
                    break;
                case Manager.Mode.Sequence:
                    switch (subMode)
                    {
                        case (int)Manager.Sequence.Primes:
                            intArray = data.Numbers.Primes;
                            fV.Value = intArray[(int)Mathf.Repeat(current, intArray.Length)];
                            fV.Text = fV.Value.ToString();
                            break;
                        case (int)Manager.Sequence.Fibbonaci:
                            intArray = data.Numbers.Fibbonaci;
                            fV.Value = intArray[(int)Mathf.Repeat(current, intArray.Length)];
                            fV.Text = fV.Value.ToString();
                            break;
                    }
                    break;
                case Manager.Mode.English:
                    switch (subMode)
                    {
                        case (int)Manager.English.Common:
                            stringArray = data.Texts.EnglishWords;
                            fV.Value = current;
                            fV.Text = stringArray[(int)Mathf.Repeat(current, stringArray.Length)];
                            break;
                        case (int)Manager.English.AusAnthem:
                            stringArray = data.Texts.AusAnthem;
                            fV.Value = current;
                            fV.Text = stringArray[(int)Mathf.Repeat(current, stringArray.Length)];
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
                if (IsCurrentValue(fV) && Manager.Current > 0) // if we should have touched this number
                {
                    return BadTouch();
                }
                else // if it's a non-important number
                    return Color.white;
            }
        }

        private bool IsCurrentValue(FaceValue value)
        {
            return value.Value == Manager.Current || value.Text == GetFaceValue(Manager.Current).Text;
        }

        private Color BadTouch() {
            if (Manager.Instance.timeAttackMode) // In time attack mode, mistakes make you lose time
            {
                if (state == State.PLAY)
                    Manager.Instance.gameTimer.AddTimePenalty();
            }
            else
            {
                switch (state)
                {
                    case State.PLAY:
                        Critical(); // In classic mode, mistakes take you straight to critical time
                        break;
                    case State.CRITICAL:
                        OnTimerCompleted();
                        break;
                }
            }
            return Color.red;
        }

        private Color Progress()
        {
            Play();
            Manager.Current++;
            if(Manager.Instance.timeAttackMode)
                Manager.Instance.gameTimer.AddTimeBonus();

            return GetFaceValue(Manager.Current).Value > oldHS ? Color.green : Color.yellow;
        }
        #endregion Resolving number touches
    }

    public class FaceValue
    {
        public int Value;
        public string Text;
    }
}