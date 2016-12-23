using RSG;
using UnityEngine;
using _scripts.Controller;
using System;
using System.Net.Security;
using System.Runtime.Remoting.Messaging;
using Assets._scripts.Model;
using Assets._scripts.View;
using UnityEngine.UI;
using _scripts.View;
using Random = UnityEngine.Random;

namespace Assets._scripts.Controller
{
    public class Tutorial : Singleton<Tutorial> {
        private readonly PromiseTimer menuTimer = new PromiseTimer();
        private readonly PromiseTimer gameTimer = new PromiseTimer();
        private GameObject tutorialArrow;

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
            tutorialArrow = UiManager.Instance.tutorialArrow;
        }

        private void Update()
        {
            menuTimer.Update(Time.unscaledDeltaTime);
            gameTimer.Update(Time.unscaledDeltaTime);
        }

        public void RunMenuTutorial()
        {
            Random.InitState(1);
            Promise.Sequence(
                HighlightObject("Play"),
                WaitForPlayButton(),
                ToggleArrow(false),
                WaitForMenuAnimation(),
                WaitForMenuAnimation(),
                HighlightObject("MainMode"),
                WaitForMainModeButton(),
                HighlightObject("SubMode"),
                WaitForSubModeButton(),
                HighlightObject("Play"),
                WaitForPlayButton())
            .Done(DeactivateTutorial);
        }

        public void RunGameTutorial()
        {
            Promise.Sequence(
                WaitForSeconds(1.2f),
                PauseGame(),
                ExplainScore(),
                ExplainTimer(),
                PromptUserToTouchNumber(),
                ToggleArrow(false),
                UnpauseGame(),
                WaitForSeconds(0.5f),
                PauseGame(),
                ShowText("This will increase the target number and give you extra game time" +
                         "\n..."),
                WaitForInput(InputDelay),
                ShowText("Letting a target number leave the screen or touching a different number" +
                         "\nwill lose you game time!" +
                         "\n..."),
                WaitForInput(InputDelay),
                ShowText("The game ends when you" +
                         "\nrun out of time." +
                         "\nGood luck!" +
                         "\n..."),
                WaitForInput(InputDelay))
            .Done(Manager.Instance.game.Unpause);
        }

        private void DeactivateTutorial()
        {
            tutorialArrow.SetActive(false);
            Preferences.ShowTutorial = false;
        }

        #region Menu tutorials

        private Func<IPromise> ToggleArrow(bool isOn)
        {
            return () =>
            {
                tutorialArrow.SetActive(isOn);
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
                var pos = new Vector3
                (
                    button.transform.position.x,
                    button.transform.position.y - 1.5f,
                    0
                );
                tutorialArrow.SetActive(true);
                tutorialArrow.transform.SetParent(button.transform);
                tutorialArrow.transform.position = pos;
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

        private Func<IPromise> PromptUserToTouchNumber()
        {
            return () =>
                Promise.Sequence(
                    ShowText("Try touching a number the same as the target"),
                    HighlightObject("Number(Clone)"),
                    WaitForNumberTouch()
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
