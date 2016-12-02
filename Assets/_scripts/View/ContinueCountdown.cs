using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class ContinueCountdown : MonoBehaviour {

        public Text label;

        void Update() {
            label.text = Manager.Instance.game.TimeRemaining.ToString();
        }

    }
}
