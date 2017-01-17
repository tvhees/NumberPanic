using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ContinueCountdown : MonoBehaviour {

        public Text label;

        void Update() {
            label.text = Manager.Instance.game.TimeRemaining.ToString();
        }
    }
}
