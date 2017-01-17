using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class FinalScore : MonoBehaviour {

        public Text label;
        [HideInInspector] public int current;
        [HideInInspector] public FaceValue fV;

        private float counter, countTime;
        private Manager.Mode mode;
        private int subValue;

        void OnEnable() {
            mode = Manager.MainMode;
            subValue = Manager.SubMode;
            current = 0;
            countTime = 1f;
            counter = 0;
        }

        void Update() {
            counter += Time.unscaledDeltaTime;
            current = Mathf.CeilToInt(Manager.Current * Mathf.Clamp01(counter / countTime));
            // Important! We are using STORED values of mode and SubMode here
            // This prevents final scores changing if mode choice is update in settings post-game
            fV = Manager.Instance.game.GetFaceValue(current);
            label.text = fV.Text;
        }

    }
}
