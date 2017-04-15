using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using GameData;
using RSG;
using UnityEngine;
using UnityEngine.Events;
using Utility;
using View;

namespace GameData
{
    public partial class Settings
    {
        [SerializeField] private MenuPanel.MenuSettings menuSettings;
        public static MenuPanel.MenuSettings Menu {get { return instance.menuSettings; }}
    }
}

namespace View
{
    public class MenuPanel : MonoBehaviour, ITransitionAnimation
    {
        [SerializeField] private Path path;
        public static UnityEvent OnFinishedAnimation = new UnityEvent();
        private bool animating;

        [Serializable]
        public class MenuSettings
        {
            [Range(0.1f, 5)]
            public float Speed;
        }

        public IPromise ScreenEnterAnimation()
        {
            return new Promise((resolve, reject) => StartCoroutine(ScreenEnterAnimation(resolve)));
        }

        public IPromise ScreenExitAnimation()
        {
            return new Promise((resolve, reject) => StartCoroutine(ScreenExitAnimation(resolve)));
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        private void ResetMenu()
        {
            this.rectTransform().localPosition = new Vector2(this.rectTransform().localPosition.x, path.Points.Last().y);
        }

        private IEnumerator ScreenEnterAnimation(Action resolve)
        {
            ResetMenu();

            var duration = 1 / Settings.Menu.Speed;
            animating = true;

            this.rectTransform().DOLocalMoveY(path.Points.First().y, duration)
                .OnComplete(() =>
                {
                    animating = false;
                    OnFinishedAnimation.Invoke();
                });

            while (animating)
            {
                yield return null;
            }

            resolve();
        }

        private IEnumerator ScreenExitAnimation(Action resolve)
        {
            var duration = 1 / Settings.Menu.Speed;
            animating = true;

            this.rectTransform().DOLocalMoveY(path.Points.Last().y, duration)
                .OnComplete(() =>
                {
                    animating = false;
                    OnFinishedAnimation.Invoke();
                });

            while (animating)
            {
                yield return null;
            }

            resolve();
        }
    }
}
