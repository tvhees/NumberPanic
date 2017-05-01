using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ContinueCountdown : MonoBehaviour {

        public Text label;

        void Update() {
            if (MainManager.Instance.game == null)
            {
                return;
            }
            
            label.text = MainManager.Instance.game.TimeRemaining.ToString();
        }
    }
}
