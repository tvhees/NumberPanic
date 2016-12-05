using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _scripts.Controller;
using _scripts.View;

namespace Assets._scripts.Controller
{
    public class AnimationManager : Singleton<AnimationManager> {

        public TitleAnimator titleAnimator;
        public MenuPanel menuPanel;
        [HideInInspector] public List<SubPanel> subPanels = new List<SubPanel>();
        public LoadingCover loadingCover;
        public ContinuePanel continuePanel;
        public ScorePanel scorePanel;
        private bool firstGame;

        private void Start()
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
            foreach (var panel in subPanels)
            {
                panel.anim.SetBool("open", false);
            }
        }

        public IEnumerator Continue(bool use)
        {
            if (use)
            {
                if (Manager.Instance.gameTimer != null)
                    Manager.Instance.NewTimer();
                Manager.Instance.game.Attract();
            }
            yield return StartCoroutine(continuePanel.Leave(use));
        }
    }
}
