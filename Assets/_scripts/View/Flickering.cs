using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class Flickering : MonoBehaviour
    {
        private const float FlickerTimeScale = 1.0f;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float flickerTime;
        [SerializeField] private Gradient colourGradient;
        [SerializeField] private Image target;

        private void Update()
        {
            target.color = GetNewColour(target.color, Time.unscaledTime*FlickerTimeScale);
        }

        private Color GetNewColour(Color color, float time)
        {
            var ordinate = 2 * Mathf.Repeat(time, flickerTime) / flickerTime;
            var index = curve.Evaluate(ordinate);
            return colourGradient.Evaluate(index);
        }
    }
}
