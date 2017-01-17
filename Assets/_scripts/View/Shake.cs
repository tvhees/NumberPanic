using DG.Tweening;
using UnityEngine;

namespace View
{
    public class Shake : MonoBehaviour
    {
        private Vector3 homePos;

        public float PunchStrength;
        public float PunchDuration;

        private void Awake()
        {
            homePos = transform.position;
        }

        public void Punch()
        {
            transform.DOPunchPosition(PunchStrength * Random.insideUnitCircle.normalized, PunchDuration)
                .OnComplete(() => transform.position = homePos);
        }
    }
}
