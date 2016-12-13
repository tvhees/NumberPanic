using RSG;
using UnityEngine;
using _scripts.Controller;
using System;
using System.Net.Security;
using Assets._scripts.Model;
using Assets._scripts.View;
using _scripts.View;
using Random = UnityEngine.Random;

namespace Assets._scripts.Controller
{
    public class Tutorial : Singleton<Tutorial> {
        private readonly PromiseTimer promiseTimer = new PromiseTimer();
        private GameObject tutorialArrow;
        private bool waitingForPlayButton;
        private bool waitingForMenuAnimation;
        private bool waitingForMainModeButton;
        private bool waitingForSubModeButton;
        private bool gameWaiting;
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

        private void Awake()
        {
            NewGameButton.OnButtonPressed.AddListener(() => waitingForPlayButton = false);
            MenuPanel.OnFinishedAnimation.AddListener(() => waitingForMenuAnimation = false);
            MainMode.OnClicked.AddListener(() => waitingForMainModeButton = false);
            SubMode.OnClicked.AddListener(() => waitingForSubModeButton = false);
            Number.OnCorrectNumberTouch.AddListener(() => gameWaiting = false);
            tutorialArrow = UiManager.Instance.tutorialArrow;
        }

        private void Update()
        {
            promiseTimer.Update(Time.unscaledDeltaTime);
        }

        public void RunMenuTutorial()
        {
            Random.InitState(1);
            Promise.Resolved()
                .Then(HighlightButton("Play"))
                .Then(WaitForPlayButton())
                .Then(() => tutorialArrow.SetActive(false))
                .Then(WaitForMenuAnimation())
                .Then(WaitForMenuAnimation())
                .Then(HighlightButton("MainMode"))
                .Then(WaitForMainModeButton())
                .Then(HighlightButton("SubMode"))
                .Then(WaitForSubModeButton())
                .Then(HighlightButton("Play"))
                .Then(WaitForPlayButton())
                .Done(DeactivateTutorial);

#if false
            ShowTextBox("PRESS A NUMBER TO START!", _ => !waiting)
                .Then(() => Manager.Instance.game.UnPause())
                .Then(() => promiseTimer.WaitFor(0.5f))
                .Then(() =>ShowTextBox("YOUR SCORE IS SHOWN HERE\n(touch to continue)", _ => isRecievingInput))
                .Then(() => promiseTimer.WaitFor(0.1f))
                .Then(() =>ShowTextBox("PRESSING THE SAME NUMBER AS YOUR SCORE IS GOOD", _ => isRecievingInput))
                .Then(() => promiseTimer.WaitFor(0.1f))
                .Then(() =>ShowTextBox("PRESSING A DIFFERENT NUMBER IS BAD", _ => isRecievingInput))
                .Then(() => promiseTimer.WaitFor(0.1f))
                .Then(() =>ShowTextBox("THE GAME ENDS WHEN THIS TIMER REACHES ZERO", _ => isRecievingInput))
                .Then(() => promiseTimer.WaitFor(0.1f))
                .Then(() =>ShowTextBox("GOOD LUCK!", _ => isRecievingInput))
                .Then(() => Manager.Instance.game.UnPause())
                .Done(() => Preferences.ShowTutorial = false);
            #endif
        }

        private void DeactivateTutorial()
        {
            tutorialArrow.SetActive(false);
            Preferences.ShowTutorial = false;
        }

        private Func<IPromise> ShowTextBox(string textIn, Func<TimeData, bool> waitCondition)
        {
            UiManager.Instance.tutorialPanel.Display(textIn);
            return PauseUntilCondition(waitCondition);
        }

        private Func<IPromise> WaitUntilCondition(Func<TimeData, bool> waitCondition)
        {
            return () => promiseTimer.WaitUntil(waitCondition);
        }

        private Func<IPromise> PauseUntilCondition(Func<TimeData, bool> waitCondition)
        {
            Manager.Instance.game.Pause();
            gameWaiting = true;
            return () => promiseTimer.WaitUntil(waitCondition);
        }

        #region Menu tutorials

        private Func<IPromise> WaitForPlayButton()
        {
            return () =>
            {
                waitingForPlayButton = true;
                return promiseTimer.WaitUntil(_ => !waitingForPlayButton);
            };
        }

        private Func<IPromise> WaitForMenuAnimation()
        {
            return () =>
            {
                waitingForMenuAnimation = true;
                return promiseTimer.WaitUntil(_ => !waitingForMenuAnimation);
            };
        }

        private Func<IPromise> WaitForMainModeButton()
        {
            return () =>
            {
                waitingForMainModeButton = true;
                return promiseTimer.WaitUntil(_ => !waitingForMainModeButton);
            };
        }

        private Func<IPromise> WaitForSubModeButton()
        {
            return () =>
            {
                waitingForSubModeButton = true;
                return promiseTimer.WaitUntil(_ => !waitingForSubModeButton);
            };
        }

        private Func<IPromise> HighlightButton(string buttonName)
        {
            return () =>
            {
                Debug.Log("Highlight: " + buttonName);
                var button = GameObject.Find(buttonName);
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

        private Func<IPromise> HighlightScore()
        {
            return Promise.Resolved;
        }

        private Func<IPromise> HighlightTimer()
        {
            return Promise.Resolved;
        }

        #endregion
    }
}
