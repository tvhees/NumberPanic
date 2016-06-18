using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueButton : MonoBehaviour {

    public Button button;

    void OnEnable()
    {
        button.interactable = true;
    }
    
    public void ContinueGame() {
#if UNITY_ANDROID || UNITY_IOS
        if(Preferences.advertisements)
            Manager.Instance.adverts.ShowAd();
        else
            StartCoroutine(AnimationManager.Instance.Continue(true));

#endif

#if !UNITY_ANDROID && !UNITY_IOS
        StartCoroutine(AnimationManager.Instance.Continue(true));
#endif
    }
}
