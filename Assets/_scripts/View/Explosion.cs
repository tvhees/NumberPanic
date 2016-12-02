using UnityEngine;
using _scripts.Controller;

namespace _scripts.View
{
    public class Explosion : MonoBehaviour {

        private ParticleSystem.LimitVelocityOverLifetimeModule lim;
        private float timer, lifetime;

        public void Init(Vector3 position, float speed, Color colour)
        {
            transform.position = position;

            ParticleSystem ps = GetComponent<ParticleSystem>();
            ps.startSpeed = speed;
            ps.startLifetime = speed;
            ps.startColor = colour;
            lim = ps.limitVelocityOverLifetime;
            lim.limit = new ParticleSystem.MinMaxCurve(speed / 2f);
            ps.Emit(15 * (int)speed);

            timer = 0;
            lifetime = speed;
        }

        void Update() {
            timer += Time.deltaTime;

            if(timer >= lifetime)
                Manager.explosionPool.ReturnObject(gameObject);
        }
    }
}
