using UnityEngine;
using System.Collections;

public class ContinuePanel : MonoBehaviour {

    public Animator anim;
    private bool animating;

    void Awake()
    {
        AnimationManager.Instance.continuePanel = this;
        UIManager.Instance.continuePanel = gameObject;
    }

    public void Enter()
    {
    }

    public IEnumerator Leave(bool use)
    {
        animating = true;

        if (use)
            anim.SetTrigger("use");
        else
            anim.SetTrigger("drop");

        while (animating)
            yield return null;

        gameObject.SetActive(false);
    }

    public void Callback()
    {
        animating = false;
    }
}

