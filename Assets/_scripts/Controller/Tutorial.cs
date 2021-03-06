﻿using System;
using Model;
using RSG;
using UnityEngine;
using Utility;
using View;
using Random = UnityEngine.Random;

namespace Controller
{
    public class Tutorial : Singleton<Tutorial> {
        private readonly PromiseTimer menuTimer = new PromiseTimer();
        private readonly PromiseTimer gameTimer = new PromiseTimer();
        private readonly PromiseTimer toggleTimer = new PromiseTimer();
        private GameObject tutorialBox;

        public bool IsRunning;

        #region Waiting Flags

        private const float InputDelay = 0.5f;
        private bool waitingForPlayButton;
        private bool waitingForMenuAnimation;
        private bool waitingForMainModeButton;
        private bool waitingForSubModeButton;
        private bool waitingForNumberTouch;
        private bool isRecievingInput
        {
            get
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                return Input.GetMouseButtonDown(0);
#endif

#if UNITY_ANDROID || UNITY_IOS
                return Input.touchCount > 0;
#endif
            }
        }
        #endregion Waiting Flags

        private void Awake()
        {
            NewGameButton.OnButtonPressed.AddListener(() => waitingForPlayButton = false);
            MenuPanel.OnFinishedAnimation.AddListener(() => waitingForMenuAnimation = false);
            MainMode.OnClicked.AddListener(() => waitingForMainModeButton = false);
            SubMode.OnClicked.AddListener(() => waitingForSubModeButton = false);
            Number.OnCorrectNumberTouch.AddListener(() => waitingForNumberTouch = false);
            tutorialBox = UiManager.Instance.tutorialArrow;
        }

        private void Update()
        {
            menuTimer.Update(Time.unscaledDeltaTime);
            gameTimer.Update(Time.unscaledDeltaTime);
            toggleTimer.Update(Time.unscaledDeltaTime);

        }

        public void RunMenuTutorial()
        {
            if (IsRunning)
                return;
            IsRunning = true;
            Random.InitState(1);
            Promise.Race(MenuTutorial(),
                    toggleTimer.WaitUntil(_ => !Preferences.ShowTutorial))
            .Done(DeactivateTutorial);
        }

        private IPromise MenuTutorial()
        {
            return Promise.Sequence(
                HighlightObject("Play"),
                WaitForPlayButton(),
                ToggleArrow(false),
                WaitForMenuAnimation(),
                WaitForMenuAnimation(),
                HighlightObject("MainMode"),
                WaitForMainModeButton(),
                HighlightObject("SubMode"),
                WaitForSubModeButton());
        }

        public void RunGameTutorial()
        {
            Promise.Sequence(
                WaitForSeconds(1.5f),
                PauseGame(),
                ExplainScore(),
                ExplainTimer(),
                ToggleArrow(false),
                ShowText("Touching a number the same as the target will increase the target number and" +
                         " give you more game time" +
                         "\n..."),
                WaitForInput(InputDelay),
                ShowText("Letting a target number leave the screen or touching a different number" +
                         " will lose you game time!" +
                         "\n..."),
                WaitForInput(InputDelay),
                ShowText("The game ends when you run out of time." +
                         " Good luck!" +
                         "\n..."),
                WaitForInput(InputDelay))
            .Done(Manager.Instance.game.EnterAttractState);
        }

        private void DeactivateTutorial()
        {
            tutorialBox.SetActive(false);
            Preferences.Instance.ToggleTutorial(false);
            IsRunning = false;
        }

        #region Menu tutorials

        private Func<IPromise> ToggleArrow(bool isOn)
        {
            return () =>
            {
                tutorialBox.SetActive(isOn);
                return Promise.Resolved();
            };
        }

        private Func<IPromise> WaitForPlayButton()
        {
            return () =>
            {
                waitingForPlayButton = true;
                return menuTimer.WaitUntil(_ => !waitingForPlayButton);
            };
        }

        private Func<IPromise> WaitForMenuAnimation()
        {
            return () =>
            {
                waitingForMenuAnimation = true;
                return menuTimer.WaitUntil(_ => !waitingForMenuAnimation);
            };
        }

        private Func<IPromise> WaitForMainModeButton()
        {
            return () =>
            {
                waitingForMainModeButton = true;
                return menuTimer.WaitUntil(_ => !waitingForMainModeButton);
            };
        }

        private Func<IPromise> WaitForSubModeButton()
        {
            return () =>
            {
                waitingForSubModeButton = true;
                return menuTimer.WaitUntil(_ => !waitingForSubModeButton);
            };
        }

        private Func<IPromise> HighlightObject(string objectName)
        {
            return () =>
            {
                var button = GameObject.Find(objectName);
                tutorialBox.SetActive(true);
                tutorialBox.transform.SetParent(button.transform);
                var rect = (RectTransform) tutorialBox.transform;
                rect.offsetMax = Vector2.zero;
                rect.offsetMin = Vector2.zero;
                rect.localScale = Vector3.one;
                return Promise.Resolved();
            };
        }

        #endregion

        #region Game tutorials

        private Func<IPromise> ExplainScore()
        {
            return () =>
                Promise.Sequence(
                    HighlightObject("GameScore"),
                    ShowText("This is the target number" +
                             "\n(touch to continue)" +
                             "\n..."),
                    WaitForInput(InputDelay)
                );
        }

        private Func<IPromise> ExplainTimer()
        {
            return () =>
                Promise.Sequence(
                    HighlightObject("TimerDisplay"),
                    ShowText("This is the remaining game time" +
                             "\n..."),
                    WaitForInput(InputDelay)
                );
        }

        private Func<IPromise> ShowText(string textIn)
        {
            return () =>
            {
                UiManager.Instance.tutorialPanel.Display(textIn);
                return Promise.Resolved();
            };
        }

        private Func<IPromise> PauseGame()
        {
            return () =>
            {
                Manager.Instance.game.Pause();
                return Promise.Resolved();
            };
        }

        private Func<IPromise> UnpauseGame()
        {
            return () =>
            {
                Manager.Instance.game.Unpause();
                return Promise.Resolved();
            };
        }

        private Func<IPromise> WaitForInput()
        {
            return () => gameTimer.WaitUntil(_ => isRecievingInput);
        }

        private Func<IPromise> WaitForInput(float delay)
        {
            return () => Promise.Sequence(WaitForSeconds(delay),
                                            WaitForInput());
        }

        private Func<IPromise> WaitForSeconds(float t)
        {
            return () => gameTimer.WaitFor(t);
        }

        private Func<IPromise> WaitForNumberTouch()
        {
            return () =>
            {
                waitingForNumberTouch = true;
                return gameTimer.WaitUntil(_ => !waitingForNumberTouch);
            };
        }

        #endregion
    }
}
