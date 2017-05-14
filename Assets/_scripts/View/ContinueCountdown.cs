using Controller;
using GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ContinueCountdown : MonoBehaviour {

        [SerializeField] private Text label;
        [SerializeField] private EndState endState;

        private void Update() {
            label.text = endState.TimeRemaining.ToString();
        }
    }
}
