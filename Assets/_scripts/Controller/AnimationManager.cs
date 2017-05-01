﻿using System.Collections;
using Managers;
using UnityEngine;
using Utility;
using View;

namespace Controller
{
    public class AnimationManager : Singleton<AnimationManager> {

        [SerializeField] private SubPanel[] subPanels;
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

            yield return null;

            /*CloseSubPanels();*/

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
                    MainManager.Instance.GameTimer = new GameTimer();
                MainManager.Instance.StateManager.MoveTo(States.Attract);
            }

            yield return null;
        }
    }
}
