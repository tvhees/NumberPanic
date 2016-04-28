using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Title : MonoBehaviour {

    [HideInInspector] private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        UIManager.Instance.title = this;
    }

    public void Fade()
    {
        animator.SetTrigger("fade");
    }

}
