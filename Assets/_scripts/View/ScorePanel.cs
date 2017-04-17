using System;
using System.Collections;
using System.Linq;
using Controller;
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
        [SerializeField] private ScorePanel.ScoreSettings scoreSettings;
        public static ScorePanel.ScoreSettings Score {get { return instance.scoreSettings; }}
    }

}

namespace View
{
    public class ScorePanel : MonoBehaviour, ITransitionAnimation {
        [SerializeField] private Path path;

        [Serializable]
        public class ScoreSettings
        {
            [Range(0.1f, 5f)]
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

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
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
