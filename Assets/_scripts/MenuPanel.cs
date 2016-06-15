using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour {

    public Animator anim;
    [HideInInspector] public bool animating;

    void Awake()
    {
        UIManager.Instance.menuPanel = gameObject;
        AnimationManager.Instance.menuPanel = this;
    }

    public IEnumerator DropMenu()
    {
        animating = true;
        anim.SetTrigger("drop");

        while (animating)
            yield return null;
    }

    public void MenuCallback()
    {
        animating = false;
    }
}
