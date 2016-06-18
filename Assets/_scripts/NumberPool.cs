using UnityEngine;
using System.Collections;

public class NumberPool : ObjectPool {

    protected override void Init()
    {
        Manager.numberPool = this;
        poolSize = 20;
        homePosition = Vector3.zero;
    }

}
