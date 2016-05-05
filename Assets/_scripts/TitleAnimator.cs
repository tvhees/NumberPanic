using UnityEngine;
using System.Collections;

public class TitleAnimator : MonoBehaviour {

    public Animator[] animators;
    private float delay = 0.1f;

    void Awake()
    {
        animators = GetComponentsInChildren<Animator>();

        StartCoroutine(DropTitle());
    }

    IEnumerator DropTitle()
    {
        // Start from 1 because we don't want to drop the background here
        for (int i = 1; i < animators.Length; i++)
        {
            animators[i].SetTrigger("drop");
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(LeaveTitle());
    }

    IEnumerator LeaveTitle()
    {
        // Start from 1 because we don't want to drop the background here
        for (int i = 1; i < animators.Length; i++)
        {
            animators[i].SetTrigger("drop");
            yield return new WaitForSeconds(delay);
        }

        // Drop the background
        animators[0].SetTrigger("drop");
    }

}
