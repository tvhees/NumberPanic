using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NoAdsButton : MonoBehaviour {

    public Button button;

    void Update()
    {
            button.interactable = Preferences.advertisements;
    }

}
