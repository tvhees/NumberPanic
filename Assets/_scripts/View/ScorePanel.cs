using System;
using DG.Tweening;
using GameData;
using UnityEngine;
using Utility;
using View;

namespace GameData
{
    public partial class Settings
    {
        [SerializeField] private ScorePanel.ScoreSettings scoreSettings;
        public static ScorePanel.ScoreSettings Score {get { return instance.scoreSettings; }}
    }

}

namespace View
{
    public class ScorePanel : TransitionAnimatedPanel {
        [Serializable]
        public class ScoreSettings
        {
            [Range(0.1f, 5f)]
            public float Speed;
        }

        protected override void ScreenEnterAnimation(Action resolve)
        {
            ResetPanel();

            AnimatePanel(resolve, 0, Ease.OutBounce);
        }

        protected override void ScreenExitAnimation(Action resolve)
        {
            AnimatePanel(resolve, 1);
        }

        private void AnimatePanel(Action resolve, int pathIndex, Ease ease = Ease.Linear)
        {
            var duration = 1 / Settings.Score.Speed;

            this.rectTransform().DOLocalMoveY(path.Points[pathIndex].y, duration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    resolve();
                });
        }
    }
}
