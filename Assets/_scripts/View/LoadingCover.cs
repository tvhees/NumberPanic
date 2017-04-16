using System;
using System.Linq;
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
        [SerializeField] private LoadingCover.LoadingCoverSettings loadingCoverSettings;
        public static LoadingCover.LoadingCoverSettings LoadingCover { get { return instance.loadingCoverSettings; } }
    }
}

namespace View
{
    public class LoadingCover : MonoBehaviour, ITransitionAnimation
    {
        [SerializeField] private Path path;

        [Serializable]
        public class LoadingCoverSettings
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

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        private void ScreenEnterAnimation(Action resolve)
        {
            AnimatePanel(resolve, 0);
        }

        private void ScreenExitAnimation(Action resolve)
        {
            ResetPanel();
            AnimatePanel(resolve, 1);
        }

        private void ResetPanel()
        {
            this.rectTransform().localPosition = new Vector2(this.rectTransform().localPosition.x, path.Points.First().y);
        }

        private void AnimatePanel(Action resolve, int pathIndex, Ease ease = Ease.Linear)
        {
            var duration = 1 / Settings.LoadingCover.Speed;

            this.rectTransform().DOLocalMoveY(path.Points[pathIndex].y, duration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    resolve();
                });
        }
    }
}
