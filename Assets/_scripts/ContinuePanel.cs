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

    public IEnumerator Leave(bool use)
    {
        animating = true;

        // Different animations are triggered if the player uses the continue
        // than if they choose not to
        if (use)
            anim.SetTrigger("use");
        else
            anim.SetTrigger("drop");

        // Wait for the animation callback to occur
        // i.e. the animation to finish
        while (animating)
            yield return null;

        gameObject.SetActive(false);
    }

    // Called by animations when they complete
    public void Callback()
    {
        animating = false;
    }
}

