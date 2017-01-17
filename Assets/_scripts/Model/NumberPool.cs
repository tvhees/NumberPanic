using Controller;
using UnityEngine;

namespace Model
{
    public class NumberPool : ObjectPool
    {
        public Color colour;

        protected override void Init()
        {
            poolSize = 20;
            homePosition = Vector3.zero;
        }
    }
}
