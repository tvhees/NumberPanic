using System;
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
        public ITransitionAnimation Loading {get { return objects.LoadingCover; }}
        public ITransitionAnimation Score {get { return objects.ScorePanel; }}

        [Serializable]
        public class SceneObjects
        {
            public SubPanel[] subPanels;
            public TitleAnimator TitleAnimator;
            public MenuPanel MenuPanel;
            public LoadingCover LoadingCover;
            public ContinuePanel continuePanel;
            public ScorePanel ScorePanel;
        }

        public void SetObjects(SceneObjects objects)
        {
            this.objects = objects;
        }
    }
}