using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class Score : MonoBehaviour {
        [HideInInspector] public FaceValue fV;

        private Text display;
        private int current;

        void Awake() {
            display = GetComponent<Text>();
            UiManager.Instance.score = this;
            UpdateDisplay();
        }

        public void UpdateDisplay() {
            fV = Manager.Instance.game.GetFaceValue(current);
            display.text = fV.Text;
        }

        public void Increment()
        {
            current++;
            UpdateDisplay();
            GetComponent<Animator>().SetTrigger("expand");
        }
    }
}
