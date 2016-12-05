using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialPanel : MonoBehaviour
{

    public Text words;

    public void Display(string textIn)
    {
        words.text = textIn;
    }
}
