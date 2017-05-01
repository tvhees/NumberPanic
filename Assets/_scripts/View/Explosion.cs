using Controller;
using UnityEngine;

namespace View
{
    public class Explosion : MonoBehaviour
    {

        private float timer;
        private float lifetime;

        public void Init(Vector3 position, float speed, Color colour)
        {
            transform.position = position;

            var ps = GetComponent<ParticleSystem>();
            var main = ps.main;
            var lim = ps.limitVelocityOverLifetime;
            main.startSpeed = speed;
            main.startLifetime = speed;
            main.startColor = colour;

            lim.limit = new ParticleSystem.MinMaxCurve(speed / 2f);
            ps.Emit(15 * (int)speed);

            timer = 0;
            lifetime = speed;
        }

        private void Update() {
            timer += Time.deltaTime;

            if(timer >= lifetime)
                MainManager.ExplosionPool.ReturnObject(gameObject);
        }
    }
}
