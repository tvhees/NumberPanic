using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinuePanel : MonoBehaviour {

    public Text continueText;
    public Image continueImage;
    public Sprite playImage;
    public Animator anim;
    private bool animating;

    void Awake()
    {
        AnimationManager.Instance.continuePanel = this;
        UIManager.Instance.continuePanel = gameObject;
    }

    void OnEnable()
    {
        if (!Preferences.advertisements)
        {
            continueText.text = "continue?";
            continueImage.overrideSprite = playImage;
        }
        else
        {
            continueText.text = "continue (and watch advertisement)?";
            continueImage.overrideSprite = null;
        }
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

