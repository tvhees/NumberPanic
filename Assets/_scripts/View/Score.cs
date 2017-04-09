using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class Score : MonoBehaviour {
        [HideInInspector] public FaceValue Fv;

        private Text display;
        private int current;

        private void Awake() {
            display = GetComponent<Text>();
            UiManager.Instance.score = this;
            UpdateDisplay();
        }

        public void UpdateDisplay() {
            Fv = MainManager.Instance.game.GetFaceValue(current);
            display.text = Fv.Text;
        }

        public void Increment()
        {
            current++;
            GetComponent<Animator>().SetTrigger("expand");
            UpdateDisplay();
        }
    }
}
