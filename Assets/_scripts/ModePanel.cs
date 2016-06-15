using UnityEngine;
using System.Collections;

public class ModePanel : MonoBehaviour {

    public Animator anim;

    void Awake()
    {
        AnimationManager.Instance.modePanel = this;
    }

}
