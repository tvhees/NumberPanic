using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _scripts.View;

namespace _scripts.Controller
{
    public class AnimationManager : Singleton<AnimationManager> {

        [HideInInspector] public TitleAnimator titleAnimator;
        [HideInInspector] public MenuPanel menuPanel;
        [HideInInspector] public List<SubPanel> subPanels = new List<SubPanel>();
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

            //CloseSubPanels();
            yield return StartCoroutine(menuPanel.DropMenu());
            UiManager.Instance.settingsButton.TogglePanel();

            if (firstGame)
            {
                yield return StartCoroutine(titleAnimator.LeaveTitle());
                firstGame = false;
            }

            loadingCover.anim.SetBool("open", false);

            Manager.Instance.Restart();
        }

        public void CloseSubPanels()
        {
            for (int i = 0; i < subPanels.Count; i++)
            {
                subPanels[i].anim.SetBool("open", false);
            }
        }

        public IEnumerator Continue(bool use)
        {
            if (use)
            {
                if (Manager.Instance.gameTimer != null)
                    Manager.Instance.NewTimer();
                Manager.Instance.game.Play();
            }
            yield return StartCoroutine(continuePanel.Leave(use));
        }
    }
}
