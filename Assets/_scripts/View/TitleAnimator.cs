using System.Collections;
using System.IO;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Path = Utility.Path;

namespace View
{
    public class TitleAnimator : MonoBehaviour {

        private bool animating;
        private Animator[] animators;
        private const float Delay = 0.05f;
        private int numLetters;
        [SerializeField] private Path touchyPath;
        [SerializeField] private Path numbersPath;

        private void Awake()
        {
            animating = false;
            animators = GetComponentsInChildren<Animator>();
        }

        public IEnumerator DropTitle()
        {
            foreach (var animator in animators)
            {
                animator.SetTrigger("drop");
                yield return new WaitForSeconds(Delay);
            }

            yield return new WaitForSeconds(Delay);
        }

        public IEnumerator TweenTitle()
        {
            var letters = GetComponentsInChildren<Text>();
            var dropSequence = DOTween.Sequence();
            var timePosition = 0f;
            animating = true;

            for (var i = 0; i < letters.Length; i++)
            {
                var l = letters[i];
                var endPoint = i < 6 ? touchyPath.Points.Last() : numbersPath.Points.Last();
                dropSequence.Insert(timePosition, l.rectTransform.DOLocalMoveY(endPoint.y, 0.2f));
                timePosition += Delay;
            }

            dropSequence.OnComplete(() => animating = false);

            while (animating)
            {
                yield return null;
            }
        }

        public IEnumerator LeaveTitle()
        {
            animating = true;
            numLetters = animators.Length;

            foreach (var animator in animators)
            {
                animator.SetTrigger("drop");
            }

            while (animating)
                yield return null;
        }

        public void Callback()
        {
            numLetters--;
            if(numLetters <= 0)
                animating = false;
        }
    }
}
