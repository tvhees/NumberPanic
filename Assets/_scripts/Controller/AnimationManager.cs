using System.Collections;
using UnityEngine;
using Utility;
using View;

namespace Controller
{
    public class AnimationManager : Singleton<AnimationManager> {

        [SerializeField] private SubPanel[] subPanels;
        [SerializeField] private ContinuePanel continuePanel;
        [SerializeField] private ScorePanel scorePanel;
        private bool firstGame;

        private void Awake()
        {
            NewGameButton.OnButtonPressed.AddListener(() => StartCoroutine((NewGame())));
        }

        private void Start()
        {
            firstGame = true;
        }

        public IEnumerator NewGame() {

            if (!firstGame)
            {
                yield return null;
                StartCoroutine(scorePanel.Drop());
            }

            CloseSubPanels();

            if (firstGame)
            {
                firstGame = false;
            }

            MainManager.Instance.Restart();
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
                if (MainManager.Instance.GameTimer != null)
                    MainManager.Instance.NewTimer();
                MainManager.Instance.game.EnterAttractState();
            }
            yield return StartCoroutine(continuePanel.Leave(isContinuing));
        }
    }
}
