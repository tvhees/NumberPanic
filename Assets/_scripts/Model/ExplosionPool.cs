using UnityEngine;
using System.Collections;

public class ExplosionPool : ObjectPool {

    protected override void Init()
    {
        Manager.explosionPool = this;
        poolSize = 30;
        homePosition = Vector3.zero;
    }

}
