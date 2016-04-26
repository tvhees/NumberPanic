using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

    ParticleSystem.LimitVelocityOverLifetimeModule lim;

    public void Init(float speed, Color colour)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.startSpeed = speed;
        ps.startLifetime = speed;
        ps.startColor = colour;
        lim = ps.limitVelocityOverLifetime;
        lim.limit = new ParticleSystem.MinMaxCurve(speed / 2f);

        ps.Emit(15 * (int)speed);
        Destroy(gameObject, speed);
    }

}
