using System;
using System.Linq;
using RSG;
using UnityEngine;
using Utility;

namespace View
{
    public abstract class TransitionAnimatedPanel : MonoBehaviour, ITransitionAnimation
    {
        [SerializeField] protected Path path;

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public IPromise ScreenEnterAnimation()
        {
            return new Promise((resolve, reject) => ScreenEnterAnimation(resolve));
        }

        public IPromise ScreenExitAnimation()
        {
            return new Promise((resolve, reject) => ScreenExitAnimation(resolve));
        }

        protected abstract void ScreenEnterAnimation(Action resolve);

        protected abstract void ScreenExitAnimation(Action resolve);

        protected virtual void ResetPanel()
        {
            this.rectTransform().localPosition = new Vector2(this.rectTransform().localPosition.x, path.Points.Last().y);
        }

        private void Awake()
        {
            ResetPanel();
        }
    }
}