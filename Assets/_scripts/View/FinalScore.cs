using Controller;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class FinalScore : MonoBehaviour {

        public Text label;
        [HideInInspector] public int current;
        [HideInInspector] public FaceValue fV;

        private float counter, countTime;
        private MainManager.Mode mode;
        private int subValue;

        void OnEnable() {
            mode = MainManager.MainMode;
            subValue = MainManager.SubMode;
            current = 0;
            countTime = 1f;
            counter = 0;
        }

        void Update() {
            if (MainManager.Instance.game == null)
                return;

            counter += Time.unscaledDeltaTime;
            current = Mathf.CeilToInt(MainManager.Current * Mathf.Clamp01(counter / countTime));
            // Important! We are using STORED values of mode and SubMode here
            // This prevents final scores changing if mode choice is update in settings post-game
            fV = MainManager.Instance.game.GetFaceValue(current);
            label.text = fV.Text;
        }

    }
}
