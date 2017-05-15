using System;
using DG.Tweening;
using GameData;
using RSG;
using UnityEngine;
using Utility;
using View;

namespace GameData
{
    public partial class Settings
    {
        [SerializeField] private SubPanel.SubPanelSettings subpanelSettings;
        public static SubPanel.SubPanelSettings SubPanelSettings {get { return instance.subpanelSettings; }}
    }
}

namespace View
{
    public class SubPanel : TransitionAnimatedPanel
    {
        private bool open;
        [SerializeField] private float[] scales;

        [Serializable]
        public class SubPanelSettings
        {
            [Range(0.1f, 5)]
            public float Speed;
        }

        public IPromise TogglePanel()
        {
            return open ? new Promise((resolve, reject) => {ScreenExitAnimation(resolve);})
                : new Promise((resolve, reject) => {ScreenEnterAnimation(resolve);});
        }

        protected override void ResetPanel()
        {
            this.rectTransform().sizeDelta = new Vector2(0, scales[0]);
        }

        protected override void ScreenEnterAnimation(Action resolve)
        {
            ResetPanel();
            AnimatePanel(resolve, 1);
            open = true;
        }

        protected override void ScreenExitAnimation(Action resolve)
        {
            AnimatePanel(resolve, 0);
            open = false;
        }

        private void AnimatePanel(Action resolve, int scaleIndex, Ease ease = Ease.Linear)
        {
            var duration = 1 / Settings.Menu.Speed;

            DOTween.To(() => this.rectTransform().sizeDelta,
                vec => this.rectTransform().sizeDelta = vec,
                new Vector2(0, scales[scaleIndex]), duration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    resolve();
                });
        }
    }
}
