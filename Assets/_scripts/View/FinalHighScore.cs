using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class FinalHighScore : MonoBehaviour {

        public Text label;

        private FinalScore finalScore;

        void OnEnable() {
            finalScore = transform.parent.GetComponentInChildren<FinalScore>();
        }

        void Update() {
            if (finalScore.fV.Value > Manager.Instance.game.HighScore)
                label.text = finalScore.label.text;
            else
                label.text = Manager.Instance.game.HighScore.ToString();
        }
    }
}

