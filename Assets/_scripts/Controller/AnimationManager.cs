using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using View;

namespace Controller
{
    public class AnimationManager : Singleton<AnimationManager> {

        [SerializeField] private SubPanel[] subPanels;
        [SerializeField] private TitleAnimator titleAnimator;
        [SerializeField] private MenuPanel menuPanel;
        [SerializeField] private LoadingCover loadingCover;
        [SerializeField] private ContinuePanel continuePanel;
        [SerializeField] private ScorePanel scorePanel;
        private bool firstGame;

        private void Awake()
        {
            NewGameButton.OnButtonPressed.AddListener(() => StartCoroutine((NewGame())));
        }

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

            CloseSubPanels();
            yield return StartCoroutine(menuPanel.DropMenu());

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
                panel.ClosePanel();
        }

        public IEnumerator ContinueButtonPressed(bool isContinuing)
        {
            if (isContinuing)
            {
                if (Manager.Instance.GameTimer != null)
                    Manager.Instance.NewTimer();
                Manager.Instance.game.EnterAttractState();
            }
            yield return StartCoroutine(continuePanel.Leave(isContinuing));
        }
    }
}
