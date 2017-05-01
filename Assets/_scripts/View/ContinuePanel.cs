using System;
using System.Collections;
using Controller;
using DG.Tweening;
using GameData;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using View;

namespace GameData
{
    public partial class Settings
    {
        [SerializeField] private ContinuePanel.ContinueSettings continueSettings;
        public static ContinuePanel.ContinueSettings Continue { get { return instance.continueSettings; } }
    }
}

namespace View
{
    public class ContinuePanel : TransitionAnimatedPanel {

        public Text continueText;
        public Image continueImage;

        [Serializable]
        public class ContinueSettings
        {
            public Sprite PlayImage;
            [Range(0.1f, 5.0f)]
            public float Speed;
        }

        private void SetTextAndImage()
        {
            if (!Preferences.ShowAdvertisements)
            {
                continueText.text = "continue?";
                continueImage.overrideSprite = Settings.Continue.PlayImage;
            }
            else
            {
                continueText.text = "continue (and watch advertisement)?";
                continueImage.overrideSprite = null;
            }
        }

        protected override void ScreenEnterAnimation(Action resolve)
        {
            SetTextAndImage();

            AnimatePanel(resolve, 0, Ease.OutBounce);
        }

        protected override void ScreenExitAnimation(Action resolve)
        {
            AnimatePanel(resolve, 1);
        }

        private void AnimatePanel(Action resolve, int pathIndex, Ease ease = Ease.Linear)
        {
            var duration = 1 / Settings.Continue.Speed;

            this.rectTransform().DOLocalMoveY(path.Points[pathIndex].y, duration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    resolve();
                });
        }
    }
}

