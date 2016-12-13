using Assets._scripts.Controller;
using UnityEngine;
using _scripts.Controller;

namespace _scripts.Model
{
    public class ExplosionPool : ObjectPool {

        protected override void Init()
        {
            Manager.explosionPool = this;
            poolSize = 30;
            homePosition = Vector3.zero;
        }

    }
}
