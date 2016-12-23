using System;
using UnityEngine;
using _scripts.Controller;
using _scripts.Model;

namespace Assets._scripts.Controller
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
        private readonly Manager.Mode mode;
        private readonly int subMode;
        private const float EndMax = 5.0f;
        private const float MaximumCriticalTime = 3.0f;

        #endregion Settings

        #region Variables

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

        public int HighScore { get; private set; }

        // Timers for end-game continuation and critical recovery
        private float timer;
        private float targetTimeScale;
        private int continuesLeft = 1;

        #endregion Variables

        public Game(Data data, Manager.Mode mode, int subMode)
        {
            this.data = data;
            this.mode = mode;
            this.subMode = subMode;
            targetTimeScale = 1.0f;
            HighScore = Preferences.Instance.GetHighScore().Value;
            EnterAttractState();
        }

        #region Game state

        public bool IsInPlayState {
            get { return GameState == State.Attract || GameState == State.Pause
                         || GameState == State.Play || GameState == State.Critical; }
        }

        public void ProcessState()
        {
            if (state == State.Critical || state == State.End)
                timer += Time.unscaledDeltaTime;

            if (state == State.End && timer > EndMax)
                EnterScoreState();
            else if (state == State.Critical && timer > MaximumCriticalTime)
                ProcessGameLoss();

            // Now we smoothly move towards the appropriate timescale
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime);
        }

        /// <summary>
        /// Change state to Attract and set time scale to 1;
        /// </summary>
        public void EnterAttractState()
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

        /// <summary>
        /// Change state to Play and immediately move time scale to 1.0f
        /// </summary>
        public void Unpause()
        {
            Play();
            Time.timeScale = targetTimeScale;
        }

        /// <summary>
        /// Change state to Play and smoothly move to time scale of 1.0f;
        /// </summary>
        public void Play()
        {
            targetTimeScale = 1.0f;
            GameState = State.Play;
        }

        /// <summary>
        /// Set the state to Critical and start the timer;
        /// </summary>
        public void EnterCriticalState()
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
        private void EnterScoreState()
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
                return Mathf.CeilToInt(Mathf.Max(max - timer, 0));
            }
        }

        /// <summary>
        /// Removes a continue if the player has any left, otherwise moves the game to the score screen
        /// </summary>
        public void ProcessGameLoss()
        {
            if (state == State.End) return;
            if (continuesLeft > 0)
            {
                continuesLeft--;
                timer = 0;
                GameState = State.End;
            }
            else
                EnterScoreState();
        }

        #endregion Timers

        #region Resolving number touches

        /// <summary>
        /// Returns the FaceValue struct corresponding to the player's score for the current game mode.
        /// </summary>
        public FaceValue GetFaceValue()
        {
            return GetFaceValue(Manager.Current);
        }

        /// <summary>
        /// Returns the FaceValue struct corresponding to nth number in the current game mode
        /// </summary>
        public FaceValue GetFaceValue(int n)
        {
            var fV = new FaceValue();
            switch (mode)
            {
                case Manager.Mode.Linear:
                    fV.Value = n * (subMode + 1);
                    fV.Text = fV.Value.ToString();
                    break;
                case Manager.Mode.Power:
                    fV.Value = (int)Mathf.Pow(n, (subMode + 2));
                    fV.Text = fV.Value.ToString();
                    break;
                case Manager.Mode.Sequence:
                    int[] seq;
                    switch (subMode)
                    {
                        case (int) Manager.Sequence.Primes:
                            seq = data.Numbers.Primes;
                            break;
                        case (int) Manager.Sequence.Fibbonaci:
                            seq = data.Numbers.Fibbonaci;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    fV.Value = seq[(int) Mathf.Repeat(n, seq.Length)];
                    fV.Text = fV.Value.ToString();
                    break;
                case Manager.Mode.English:
                    string[] words;
                    switch (subMode)
                    {
                        case (int)Manager.English.Common:
                            words = data.Texts.EnglishWords;
                            break;
                        case (int)Manager.English.AusAnthem:
                            words = data.Texts.AusAnthem;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    fV.Value = n;
                    fV.Text = words[(int)Mathf.Repeat(n, words.Length)];
                    break;
                case Manager.Mode.NumberOfTypes:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return fV;
        }

        public void ProcessGameEvent(bool isPositiveEvent)
        {
            if(isPositiveEvent)
                PositiveGameEvent();
            else
                NegativeGameEvent();
        }

        private void NegativeGameEvent()
        {
            Manager.Instance.audioManager.PlayNegativeSound();
            if (state == State.Critical)
                ProcessGameLoss();
            else if (state == State.Play)
                if (Manager.Instance.TimeAttackMode)
                    Manager.Instance.GameTimer.AddTimePenalty();
                else
                    EnterCriticalState(); // In classic mode, mistakes take you straight to critical time
        }

        /// <summary>
        /// Increment player score, set game state to Play() and add a time bonus in time attack mode
        /// </summary>
        private void PositiveGameEvent()
        {
            Manager.Instance.audioManager.PlayPositiveSound();
            Manager.Current++;
            Play();
            if(Manager.Instance.TimeAttackMode)
                Manager.Instance.GameTimer.AddTimeBonus();
        }

        #endregion Resolving number touches
    }
}