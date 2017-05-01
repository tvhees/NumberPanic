using System;
using UnityEngine;

namespace Utility
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private Vector3[] points = { new Vector3(0, 0, 0), new Vector3(1, 0, 0) };
        private Vector3[] startingPoints = { new Vector3(0, 0, 0), new Vector3(1, 0, 0) };
        public float HandleSize = 0.8f;
        public float PickSize = 1.0f;
        public bool RelativeToParent;

        private void Awake()
        {
            startingPoints = (Vector3[]) points.Clone();
        }

        public Vector3[] Points {
            get { if (points.Length < 2){ Reset(); }
                return points;
            }
        }

        [SerializeField]
        private bool reverseDirection;

        public Color PathColor = Color.white;

        public void Reset()
        {
            if (startingPoints.Length >= 2)
            {
                points = (Vector3[]) startingPoints.Clone();
            }
            else
            {
                points = new[] {new Vector3(0, 0, 0), new Vector3(1, 0, 0)};
            }
        }

        public void AddPoint(Vector3 increment = default(Vector3))
        {
            var index = points.Length;
            var point = points[index - 1] + increment;
            Array.Resize(ref points, points.Length + 1);
            points[index] = point;
        }

        public Vector3 GetPoint(int index)
        {
            index = reverseDirection ? points.Length - 1 - index : index;

            return points[index];
        }

        public void SetPoint(int index, Vector3 point)
        {
            points[index] = point;
        }

        public Vector3 GetPoint(float fractionOfPath)
        {
            var index = Mathf.RoundToInt(fractionOfPath * (points.Length - 1));
            return GetPoint(index);
        }
    }
}