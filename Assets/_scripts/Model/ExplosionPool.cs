using Controller;
using UnityEngine;

namespace Model
{
    public class ExplosionPool : ObjectPool {

        protected override void Awake()
        {
            MainManager.ExplosionPool = this;
            PoolSize = 30;
            HomePosition = Vector3.zero;
        }
    }
}
