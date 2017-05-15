using System;
using System.Linq;
using DG.Tweening;
using GameData;
using UnityEngine;
using UnityEngine.UI;
using View;
using Path = Utility.Path;

namespace GameData {
    public partial class Settings
    {
        [SerializeField]
        private TitleAnimator.TitleSettings title;
        public static TitleAnimator.TitleSettings Title { get { return instance.title; } }
    }
}

namespace View
{
    [RequireComponent(typeof(Path))]
    public class TitleAnimator : TransitionAnimatedPanel
    {
        [SerializeField] private Path secondPath;

        [Serializable]
        public class TitleSettings
        {
            public float DelayBetweenLetters;
            public float DelayBetweenWords;
            [Range(0.1f, 5)]
            public float LetterSpeed;
        }

        protected override void ScreenEnterAnimation(Action resolve)
        {
            ResetPanel();
            AnimateLetters(resolve, 1, Ease.InOutQuad);
        }

        protected override void ScreenExitAnimation(Action resolve)
        {
            AnimateLetters(resolve, 2, Ease.InOutQuad);
        }

        protected override void ResetPanel()
        {
            var letters = GetComponentsInChildren<Text>();
            for (var i = 0; i < letters.Length; i++)
            {
                var l = letters[i];
                var home = i < 6 ? path.Points.First() : secondPath.Points.First();
                l.rectTransform.localPosition = new Vector2(l.rectTransform.localPosition.x, home.y);
            }
        }

        private void AnimateLetters(Action resolve, int pathIndex, Ease ease = Ease.Linear)
        {
            var letters = GetComponentsInChildren<Text>();
            var sequence = DOTween.Sequence();
            var timePosition = 0f;
            var delay = Settings.Title.DelayBetweenLetters;
            var duration = 1.0f / Settings.Title.LetterSpeed;
            var delayBetweenWords = Settings.Title.DelayBetweenWords;
            var lettersInFirstWord = "touchy".Length;
            for (var i = 0; i < letters.Length; i++)
            {
                var l = letters[i];
                var endPoint = i < lettersInFirstWord ? path.Points[pathIndex]
                    : secondPath.Points[pathIndex];

                sequence.Insert(timePosition,
                    l.rectTransform.DOLocalMoveY(endPoint.y,duration).SetEase(ease));

                timePosition += delay;
                if (i == lettersInFirstWord - 1)
                {
                    timePosition += delayBetweenWords;
                }
            }

            sequence.OnComplete(() =>
            {
                resolve();
            });
        }
    }
}
