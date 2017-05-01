using System;
using Managers;
using Model;
using RSG;
using UnityEngine;
using View;

namespace Controller
{
    public class Game {
        public readonly MainManager.Mode Mode;
        public readonly int SubMode;
        public int HighScore { get; private set; }

        private readonly StateManager stateManager;
        private static float _targetTimeScale;

        // Timers for end-game continue countdown
        private const float MaxCountdownTime = 5.0f;
        private float countdownTimer;
        private int continuesLeft = 1;

        public Game(MainManager.Mode mode, int subMode, StateManager stateManager)
        {
            this.Mode = mode;
            this.SubMode = subMode;
            this.stateManager = stateManager;
            _targetTimeScale = 1.0f;
            HighScore = Preferences.Instance.GetHighScore().Value;
            stateManager.MoveTo(States.Attract);
            SocialManager.Instance.NewGamePlayed(mode);
        }

        public bool IsInPlayState {
            get { return stateManager.CurrentStateIs(States.Attract, States.Play); }
        }

        public static IPromise SetTargetTimeScale(float value)
        {
            _targetTimeScale = value;
            return Promise.Resolved();
        }

        public void ProcessState()
        {
            if (stateManager.CurrentStateIs(States.End))
            {
                countdownTimer += Time.unscaledDeltaTime;

                if (countdownTimer > MaxCountdownTime)
                {
                    EnterScoreState();
                }
            }

            // Now we smoothly move towards the appropriate timescale
            Time.timeScale = Mathf.Lerp(Time.timeScale, _targetTimeScale, Time.unscaledDeltaTime);
        }

        /// <summary>
        /// Set game state to End
        /// </summary>
        public void End()
        {
            stateManager.MoveTo(States.End);
        }

        /// <summary>
        /// Set game state to Score
        /// </summary>
        private void EnterScoreState()
        {
            stateManager.MoveTo(States.Score);
        }

        /// <summary>
        /// Returns the current game timer
        /// </summary>
        public float TimeRemaining
        {
            get
            {
                return Mathf.CeilToInt(Mathf.Max(MaxCountdownTime - countdownTimer, 0));
            }
        }

        /// <summary>
        /// Removes a continue if the player has any left, otherwise moves the game to the score screen
        /// </summary>
        public void ProcessGameLoss()
        {
            if (stateManager.CurrentStateIs(States.End))
            {
                return;
            }

            if (continuesLeft > 0)
            {
                continuesLeft--;
                countdownTimer = 0;
                stateManager.MoveTo(States.End);
            }
            else
                EnterScoreState();
        }

        /// <summary>
        /// Returns the FaceValue struct corresponding to the player's score for the current game mode.
        /// </summary>
        public FaceValue GetFaceValue()
        {
            return GetFaceValue(MainManager.Current);
        }

        /// <summary>
        /// Returns the FaceValue struct corresponding to nth number in the current game mode
        /// </summary>
        public FaceValue GetFaceValue(int n)
        {
            var fV = new FaceValue();
            switch (Mode)
            {
                case MainManager.Mode.Linear:
                    fV.Value = n * (SubMode + 1);
                    fV.Text = fV.Value.ToString();
                    break;
                case MainManager.Mode.Power:
                    fV.Value = (int)Mathf.Pow(n, (SubMode + 2));
                    fV.Text = fV.Value.ToString();
                    break;
                case MainManager.Mode.Sequence:
                    int[] seq;
                    switch (SubMode)
                    {
                        case (int) MainManager.Sequence.Primes:
                            seq = Data.Numbers.Primes;
                            break;
                        case (int) MainManager.Sequence.Fibbonaci:
                            seq = Data.Numbers.Fibbonaci;
                            break;
                        case (int) MainManager.Sequence.Pi:
                            seq = Data.Numbers.Pi;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    fV.Value = seq[(int) Mathf.Repeat(n, seq.Length)];
                    fV.Text = fV.Value.ToString();
                    break;
                case MainManager.Mode.English:
                    string[] words;
                    switch (SubMode)
                    {
                        case (int) MainManager.English.Alphabet:
                            words = Data.Texts.Alphabet;
                            break;
                        case (int)MainManager.English.CommonWords:
                            words = Data.Texts.EnglishWords;
                            break;
                        case (int)MainManager.English.AusAnthem:
                            words = Data.Texts.AusAnthem;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    fV.Value = n;
                    fV.Text = words[(int)Mathf.Repeat(n, words.Length)];
                    break;
                case MainManager.Mode.NumberOfTypes:
                    throw new ArgumentOutOfRangeException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return fV;
        }

        public void ProcessGameEvent(bool isPositiveEvent, Camera gameCam)
        {
            if(isPositiveEvent)
                PositiveGameEvent();
            else
                NegativeGameEvent(gameCam);
        }

        private void NegativeGameEvent(Camera gameCam)
        {
            MainManager.Instance.audioManager.PlayNegativeSound();
            if(Preferences.ShakeCamera)
                gameCam.GetComponent<Shake>().Punch();

            if (stateManager.CurrentStateIs(States.Play))
                MainManager.Instance.GameTimer.AddTimePenalty();
        }

        /// <summary>
        /// Increment player score, set game state to Play() and add a time bonus in time attack mode
        /// </summary>
        private void PositiveGameEvent()
        {
            MainManager.Instance.audioManager.PlayPositiveSound();
            MainManager.Current++;
            if(MainManager.Current > 12)
                SocialManager.UpdateTimesTablesArray(Mode, SubMode);

            stateManager.MoveTo(States.Play);
            MainManager.Instance.GameTimer.AddTimeBonus();
        }
    }
}