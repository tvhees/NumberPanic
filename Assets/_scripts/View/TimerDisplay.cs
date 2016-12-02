using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class TimerDisplay : MonoBehaviour 
    {
        private Text timerText;
        public Color lotsOfTimeColour;
        public Color noTimeColour;


        void Awake()
        {
            UiManager.Instance.timerDisplay = this;
            timerText = GetComponent<Text>();
        }

        public void SetRemainingTime(float remainingTime, string remainingTimeString)
        {
            timerText.text = remainingTimeString;
            timerText.color = Color.Lerp(noTimeColour, lotsOfTimeColour, remainingTime/(GameTimer.startingTime * 2f));
        }
    }
}