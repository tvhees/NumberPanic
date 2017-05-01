using Controller;
using UnityEngine;

namespace Model
{
    public class ExplosionPool : ObjectPool {

        protected override void Init()
        {
            MainManager.ExplosionPool = this;
            PoolSize = 30;
            HomePosition = Vector3.zero;
        }

    }
}
