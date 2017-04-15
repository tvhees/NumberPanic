using System;
using System.Linq;
using DG.Tweening;
using GameData;
using RSG;
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
    public class TitleAnimator : MonoBehaviour, ITransitionAnimation
    {

        private bool animating;
        [SerializeField] private Path touchyPath;
        [SerializeField] private Path numbersPath;

        [Serializable]
        public class TitleSettings
        {
            public float DelayBetweenLetters;
            [Range(0.1f, 5)]
            public float LetterSpeed;
        }

        public IPromise ScreenEnterAnimation()
        {
            return new Promise((resolve, reject) =>
            {
                ResetTitle();

                AnimateLetters(resolve, 1, Ease.OutBounce);
            });
        }

        public IPromise ScreenExitAnimation()
        {
            return new Promise((resolve, reject) =>
            {
                AnimateLetters(resolve, 2);
            });
        }

        private void ResetTitle()
        {
            var letters = GetComponentsInChildren<Text>();
            for (var i = 0; i < letters.Length; i++)
            {
                var l = letters[i];
                var home = i < 6 ? touchyPath.Points.First() : numbersPath.Points.First();
                l.rectTransform.localPosition = new Vector2(l.rectTransform.localPosition.x, home.y);
            }
        }

        private void AnimateLetters(Action resolve, int pathIndex, DG.Tweening.Ease ease = Ease.Linear)
        {
            var letters = GetComponentsInChildren<Text>();
            var sequence = DOTween.Sequence();
            var timePosition = 0f;
            var delay = Settings.Title.DelayBetweenLetters;
            var duration = 1.0f / Settings.Title.LetterSpeed;

            for (var i = 0; i < letters.Length; i++)
            {
                var l = letters[i];
                var endPoint = i < 6 ? touchyPath.Points[pathIndex]
                    : numbersPath.Points[pathIndex];

                sequence.Insert(timePosition,
                    l.rectTransform.DOLocalMoveY(endPoint.y,duration).SetEase(ease));

                timePosition += delay;
            }

            sequence.OnComplete(() =>
            {
                resolve();
            });
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
