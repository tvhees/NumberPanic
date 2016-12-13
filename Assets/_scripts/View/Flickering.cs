using UnityEngine;

namespace Assets._scripts.View
{
    public class Flickering : MonoBehaviour
    {
        private const float FlickerTimeScale = 1.0f;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float flickerTime;
        [SerializeField] private Gradient colourGradient;
        [SerializeField] private TextMesh text;

        private void Update()
        {
            text.color = GetNewColour(text.color, Time.unscaledTime*FlickerTimeScale);
        }

        private Color GetNewColour(Color color, float time)
        {
            var ordinate = 2 * Mathf.Repeat(time, flickerTime) / flickerTime;
            var index = curve.Evaluate(ordinate);
            return colourGradient.Evaluate(index);
        }
    }
}
