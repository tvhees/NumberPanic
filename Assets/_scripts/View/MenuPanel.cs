using System;
using DG.Tweening;
using GameData;
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
    public class MenuPanel : TransitionAnimatedPanel
    {
        public static UnityEvent OnFinishedAnimation = new UnityEvent();

        [Serializable]
        public class MenuSettings
        {
            [Range(0.1f, 5)]
            public float Speed;
        }

        protected override void ScreenEnterAnimation(Action resolve)
        {
            ResetPanel();
            AnimatePanel(resolve, 0, Ease.InOutQuad);
        }

        protected override void ScreenExitAnimation(Action resolve)
        {
            AnimatePanel(resolve, 1, Ease.InOutQuad);
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
