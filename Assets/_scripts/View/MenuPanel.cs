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

        [Serializable]
        public class MenuSettings
        {
            [Range(0.1f, 5)]
            public float Speed;
        }

        public IPromise ScreenEnterAnimation()
        {
            return new Promise((resolve, reject) => ScreenEnterAnimation(resolve));
        }

        public IPromise ScreenExitAnimation()
        {
            return new Promise((resolve, reject) => ScreenExitAnimation(resolve));
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        private void ScreenEnterAnimation(Action resolve)
        {
            ResetMenu();

            AnimatePanel(resolve, 0, Ease.OutBounce);
        }

        private void ScreenExitAnimation(Action resolve)
        {
            AnimatePanel(resolve, 1);
        }

        private void ResetMenu()
        {
            this.rectTransform().localPosition = new Vector2(this.rectTransform().localPosition.x, path.Points.Last().y);
        }

        private void AnimatePanel(Action resolve, int pathIndex, Ease ease = Ease.Linear)
        {
            var duration = 1 / Settings.Menu.Speed;

            this.rectTransform().DOLocalMoveY(path.Points[pathIndex].y, duration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    resolve();
                    OnFinishedAnimation.Invoke();
                });
        }
    }
}
