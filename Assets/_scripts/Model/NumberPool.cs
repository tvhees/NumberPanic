using Controller;
using UnityEngine;

namespace Model
{
    public class NumberPool : ObjectPool
    {
        public Color Colour;

        protected override void Init()
        {
            PoolSize = 20;
            HomePosition = Vector3.zero;
        }
    }
}
