using UnityEngine;
using System.Collections;

public class AnimationManager : Singleton<AnimationManager> {

    [HideInInspector] public TitleAnimator titleAnimator;
    [HideInInspector] public MenuPanel menuPanel;
    [HideInInspector] public ModePanel modePanel;
    [HideInInspector] public LoadingCover loadingCover;
    [HideInInspector] public ContinuePanel continuePanel;
    [HideInInspector] public ScorePanel scorePanel;
    private bool firstGame;

    void Start()
    {
        StartCoroutine(titleAnimator.DropTitle());
        firstGame = true;
    }

    public IEnumerator NewGame() {

        if (!firstGame)
        {
            yield return StartCoroutine(loadingCover.Enter());
            StartCoroutine(scorePanel.Drop());
        }

        modePanel.anim.SetBool("open", false);
        yield return StartCoroutine(menuPanel.DropMenu());

        if (firstGame)
        {
            titleAnimator.LeaveTitle();
            firstGame = false;
            yield return new WaitForSeconds(0.5f);
        }

        loadingCover.anim.SetBool("open", false);

        Manager.Instance.Restart();
    }

    public IEnumerator Continue(bool use)
    {
        if(use)
            Manager.Instance.game.Play();
        yield return StartCoroutine(continuePanel.Leave(use));
    }

}
