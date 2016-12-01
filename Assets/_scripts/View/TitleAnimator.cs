using UnityEngine;
using System.Collections;

public class TitleAnimator : MonoBehaviour {

    [HideInInspector] public bool animating;
    public Animator[] animators;
    private float delay = 0.1f;
    private int letters;

    void Awake()
    {
        animating = false;
        animators = GetComponentsInChildren<Animator>();
        AnimationManager.Instance.titleAnimator = this;
    }

    public IEnumerator DropTitle()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("drop");
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(delay);

        Manager.Instance.game.state = Game.State.TITLE;
    }

    public IEnumerator LeaveTitle()
    {
        animating = true;
        letters = animators.Length;
        Manager.Instance.game.state = Game.State.TITLE;

        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("drop");
        }

        while (animating)
            yield return null;
    }

    public void Callback()
    {
        letters--;
        if(letters <= 0)
            animating = false;
    }
}
