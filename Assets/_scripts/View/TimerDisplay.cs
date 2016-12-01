using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TimerDisplay : MonoBehaviour 
{
    private Text timerText;
    public Color lotsOfTimeColour;
    public Color noTimeColour;


    void Awake()
    {
        UIManager.Instance.timerDisplay = this;
        timerText = GetComponent<Text>();
    }

    public void SetRemainingTime(float remainingTime, string remainingTimeString)
    {
        timerText.text = remainingTimeString;
        timerText.color = Color.Lerp(noTimeColour, lotsOfTimeColour, remainingTime/(GameTimer.startingTime * 2f));
    }
}