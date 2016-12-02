using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class FinalHighScore : MonoBehaviour {

        public Text label;

        private FinalScore finalScore;

        void OnEnable() {
            finalScore = transform.parent.GetComponentInChildren<FinalScore>();
        }

        void Update() {
            if (finalScore.fV.Value > Manager.Instance.game.oldHS)
                label.text = finalScore.label.text;
            else
                label.text = Manager.Instance.game.oldHS.ToString();
        }
    }
}

