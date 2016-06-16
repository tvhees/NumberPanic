using UnityEngine;
using System.Collections;

public class SubPanel : MonoBehaviour {

    public Animator anim;

    void Awake()
    {
        AnimationManager.Instance.subPanels.Add(this);
    }

}
