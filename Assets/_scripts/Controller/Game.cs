using System.Collections;
using Assets._scripts.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using _scripts.Model;

namespace _scripts.Controller
{
    public class Game {
        public enum State
        {
            Title,
            Attract,
            Pause,
            Play,
            Critical,
            End,
            Score
        }
        #region Settings

        private readonly Data data;
        private const float EndMax = 5.0f;
        private const float MaximumCriticalTime = 3.0f;

        #endregion Settings

        #region Variables

        [HideInInspector]
        private State state;
        public State GameState
        {
            get { return state; }
            set
            {
                if (state == value) return;
                state = value;
                EventManager.OnStateChanged.Invoke((value));
            }
        }

        [HideInInspector]
        public int OldHs;

        // Timers for end-game continuation and critical recovery
        private float timer;
        private float targetTimeScale;
        private int continuesLeft = 1;
        public Manager.Mode Mode;
        public int SubMode;

        #endregion Variables

        public Game(Data data, Manager.Mode mode, int subMode)
        {
            this.data = data;
            this.Mode = mode;
            this.SubMode = subMode;
            targetTimeScale = 1.0f;
            OldHs = Preferences.Instance.GetHighScore().Value;
            Attract();
        }

        #region Game state

        public void ProcessState()
        {
            if (state == State.Critical || state == State.End)
                timer += Time.unscaledDeltaTime;

            if (state == State.End && timer > EndMax)
                Score();
            else if (state == State.Critical && timer > MaximumCriticalTime)
                OnTimerCompleted();

            // Now we smoothly move towards the appropriate timescale
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime);
        }

        /// <summary>
        /// Change state to Attract and set time scale to 1;
        /// </summary>
        public void Attract()
        {
            targetTimeScale = 1.0f;
            GameState = State.Attract;
        }

        /// <summary>
        /// Change state to Pause and set time scale to 1;
        /// </summary>
        public void Pause()
        {
            targetTimeScale = 0.0f;
            Time.timeScale = targetTimeScale;
            GameState = State.Pause;
        }

        public void Resume()
        {
            Play();
            Time.timeScale = targetTimeScale;
        }

        /// <summary>
        /// Change state to Play and set time scale to 1;
        /// </summary>
        public void Play()
        {
            targetTimeScale = 1.0f;
            GameState = State.Play;
        }

        /// <summary>
        /// Set the state to Critical and start the timer;
        /// </summary>
        public void Critical()
        {
            if (state == State.Critical) return;
            timer = 0;
            targetTimeScale = 1.0f;
            GameState = State.Critical;
        }

        /// <summary>
        /// Set game state to End
        /// </summary>
        public void End()
        {
            GameState = State.End;
        }

        /// <summary>
        /// Set game state to Score
        /// </summary>
        private void Score()
        {
            targetTimeScale = 1.0f;
            GameState = State.Score;
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
                var max = state == State.Critical ? MaximumCriticalTime : EndMax;
                return Mathf.CeilToInt(Mathf.Max(MaximumCriticalTime - timer, 0));
            }
        }

        /// <summary>
        /// Removes a continue if the player has any left, otherwise moves the game to the score screen
        /// </summary>
        public void OnTimerCompleted()
        {
            if (state == State.End) return;
            if (continuesLeft > 0)
            {
                continuesLeft--;
                timer = 0;
                GameState = State.End;
            }
            else
                Score();
        }

        #endregion Timers

        #region Resolving number touches

        public FaceValue GetFaceValue(int current)
        {
            var fV = new FaceValue();
            int[] intArray = null;
            string[] stringArray = null;
            switch (Mode)
            {
                case Manager.Mode.Linear:
                    fV.Value = current * (SubMode + 1);
                    fV.Text = fV.Value.ToString();
                    break;
                case Manager.Mode.Power:
                    fV.Value = (int)Mathf.Pow(current, (SubMode + 2));
                    fV.Text = fV.Value.ToString();
                    break;
                case Manager.Mode.Sequence:
                    switch (SubMode)
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
                    switch (SubMode)
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
                return IsCurrentValue(fV) ? Progress() : BadTouch();
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

        private Color BadTouch()
        {
            if (state == State.Play)
                if (Manager.Instance.timeAttackMode)
                    Manager.Instance.gameTimer.AddTimePenalty();
                else
                    Critical(); // In classic mode, mistakes take you straight to critical time
            else if (state == State.Critical)
                OnTimerCompleted();
            return Color.red;
        }

        private Color Progress()
        {
            Play();
            Manager.Current++;
            if(Manager.Instance.timeAttackMode)
                Manager.Instance.gameTimer.AddTimeBonus();

            return GetFaceValue(Manager.Current).Value > OldHs ? Color.green : Color.yellow;
        }
        #endregion Resolving number touches

    }

    public class FaceValue
    {
        public int Value;
        public string Text;
    }
}