using UnityEngine;
using System.Collections;

public class TitleAnimator : MonoBehaviour {

    [HideInInspector] public bool animating;
    public Animator[] animators;
    private float delay = 0.1f;

    void Awake()
    {
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

    public void LeaveTitle()
    {
        Manager.Instance.game.state = Game.State.TITLE;

        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetTrigger("drop");
        }
    }
}
