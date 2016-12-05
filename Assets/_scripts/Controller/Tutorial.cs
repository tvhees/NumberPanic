using RSG;
using UnityEngine;
using _scripts.Controller;
using System;
using System.Runtime.InteropServices.ComTypes;

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
            promiseTimer.Update(Time.deltaTime);
        }

        public void RunTutorial()
        {
            ShowTextBox("Press a number!", _ => !waiting)
                .Then(() => Manager.Instance.game.Resume())
                .Then(() => promiseTimer.WaitFor(0.5f))
                .Then(() =>ShowTextBox("The number you need to press is always shown here in yellow. This is also your score", _ => RecievedGenericInput()))
                .Then(() =>ShowTextBox("Pressing the right number adds to your timer.", _ => RecievedGenericInput()))
                .Then(() =>ShowTextBox("Pressing the wrong number or letting the right number hit the bottom of the screen decreases the timer", _ => RecievedGenericInput()))
                .Then(() =>ShowTextBox("The game ends when the timer runs out. Try to get the highest score you can, good luck!", _ => RecievedGenericInput()))
                .Done(() => Manager.Instance.game.Resume());
        }

        private IPromise ShowTextBox(string text, Func<TimeData, bool> waitCondition)
        {
            Debug.Log("Showing text: " + text);
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
            return Input.GetMouseButton(0);
            #endif

            #if UNITY_ANDROID || UNITY_IOS
            return Input.touchCount > 0;
            #endif
        }
    }
}
