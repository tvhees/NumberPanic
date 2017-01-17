using UnityEngine;

namespace Controller
{
    public class MovingObject : MonoBehaviour
    {
        public float Speed;

        private void Update () {
            transform.Translate(Speed * Vector3.down * Time.deltaTime);
        }
    }
}
