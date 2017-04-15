﻿using System;
using UnityEngine;
using View;

namespace Managers
{
    [CreateAssetMenu(fileName = "AnimationManager.asset", menuName = "Manager/Animation")]
    [ManagerDependency(typeof(StateManager))]
    public class AnimManager : Manager
    {
        private SceneObjects objects;

        public ITransitionAnimation Title {get { return objects.TitleAnimator; }}
        public ITransitionAnimation Menu {get { return objects.MenuPanel; }}

        [Serializable]
        public class SceneObjects
        {
            public SubPanel[] subPanels;
            public TitleAnimator TitleAnimator;
            public MenuPanel MenuPanel;
            public LoadingCover loadingCover;
            public ContinuePanel continuePanel;
            public ScorePanel scorePanel;
        }

        public void SetObjects(SceneObjects objects)
        {
            this.objects = objects;
        }
    }
}