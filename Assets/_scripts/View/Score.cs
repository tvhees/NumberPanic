using Assets._scripts.Controller;
using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;
using _scripts.View;

namespace Assets._scripts.View
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
            Fv = Manager.Instance.game.GetFaceValue(current);
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
