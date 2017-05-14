using System.Collections;
using Managers;
using UnityEngine;
using Utility;
using View;

namespace Controller
{
    public class AnimationManager : Singleton<AnimationManager> {

        [SerializeField] private SubPanel[] subPanels;

        public void CloseSubPanels()
        {
            foreach (var panel in subPanels)
                panel.ClosePanel();
        }

        public IEnumerator ContinueButtonPressed(bool isContinuing)
        {
            if (isContinuing)
            {
                MainManager.Instance.StateManager.MoveTo(States.Attract);
            }

            yield return null;
        }
    }
}
