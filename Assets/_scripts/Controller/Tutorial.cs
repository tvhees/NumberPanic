using RSG;
using UnityEngine;
using _scripts.Controller;
using System;
using System.Runtime.InteropServices.ComTypes;
using _scripts.View;

namespace Assets._scripts.Controller
{
    public class Tutorial : Singleton<Tutorial> {
        PromiseTimer promiseTimer = new PromiseTimer();
        private bool waiting;

        private void Awake()
        {
            EventManager.OnNumberTouched.AddListener(() => waiting = false);
        }

        private void Update()
        {
            promiseTimer.Update(Time.unscaledDeltaTime);
        }

        public void RunTutorial()
        {
            ShowTextBox("PRESS A NUMBER TO START!", _ => !waiting)
                .Then(() => Manager.Instance.game.Resume())
                .Then(() => promiseTimer.WaitFor(0.5f))
                .Then(() =>ShowTextBox("YOUR SCORE IS SHOWN HERE\n(touch to continue)", _ => RecievedGenericInput()))
                .Then(() => promiseTimer.WaitFor(0.1f))
                .Then(() =>ShowTextBox("PRESSING THE SAME NUMBER AS YOUR SCORE IS GOOD", _ => RecievedGenericInput()))
                .Then(() => promiseTimer.WaitFor(0.1f))
                .Then(() =>ShowTextBox("PRESSING A DIFFERENT NUMBER IS BAD", _ => RecievedGenericInput()))
                .Then(() => promiseTimer.WaitFor(0.1f))
                .Then(() =>ShowTextBox("THE GAME ENDS WHEN THIS TIMER REACHES ZERO", _ => RecievedGenericInput()))
                .Then(() => promiseTimer.WaitFor(0.1f))
                .Then(() =>ShowTextBox("GOOD LUCK!", _ => RecievedGenericInput()))
                .Done(() => Manager.Instance.game.Resume());
        }

        private IPromise ShowTextBox(string textIn, Func<TimeData, bool> waitCondition)
        {
            UiManager.Instance.tutorialPanel.Display(textIn);
            return WaitForNumberTouch(waitCondition);
        }

        private IPromise WaitForNumberTouch(Func<TimeData, bool> waitCondition)
        {
            Manager.Instance.game.Pause();
            waiting = true;
            return promiseTimer.WaitUntil(waitCondition);
        }

        private bool RecievedGenericInput()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE
            return Input.GetMouseButtonDown(0);
            #endif

            #if UNITY_ANDROID || UNITY_IOS
            return Input.touchCount > 0;
            #endif
        }
    }
}
