using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSG;

public class Tutorial : Singleton<Tutorial> {

    PromiseTimer promiseTimer = new PromiseTimer();
    bool waiting;

    void RunTutorial()
    {
        PrepTextBox("Tutorial 1")
            .Done(() => Time.timeScale = 1.0f);
    }

    IPromise PrepTextBox(string text)
    {
        Debug.Log("Showing text: " + text);
        return WaitForNumberTouch();
    }

    IPromise WaitForNumberTouch()
    {
        Time.timeScale = 0.0f;
        return promiseTimer.WaitUntil(_ => !waiting);
    }

}
