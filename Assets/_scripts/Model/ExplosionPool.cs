using Controller;
using UnityEngine;

namespace Model
{
    public class ExplosionPool : ObjectPool {

        protected override void Init()
        {
            MainManager.explosionPool = this;
            poolSize = 30;
            homePosition = Vector3.zero;
        }

    }
}
