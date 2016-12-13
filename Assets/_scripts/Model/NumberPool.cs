using Assets._scripts.Controller;
using UnityEngine;
using _scripts.Controller;

namespace _scripts.Model
{
    public class NumberPool : ObjectPool {

        protected override void Init()
        {
            Manager.numberPool = this;
            poolSize = 20;
            homePosition = Vector3.zero;
        }

    }
}
