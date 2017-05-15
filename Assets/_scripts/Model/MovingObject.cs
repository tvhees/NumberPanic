using UnityEngine;

namespace Model
{
    public class MovingObject : MonoBehaviour
    {
        public float Speed;

        private void Update () {
            transform.Translate(Speed * Vector3.down * Time.deltaTime);
        }
    }
}
