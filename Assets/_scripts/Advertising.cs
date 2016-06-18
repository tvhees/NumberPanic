using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class Advertising : MonoBehaviour
{
    public string zoneID;
    private int counter;

    void Awake()
    {
        Manager.Instance.adverts = this;
        counter = 3;
    }

    public void RegularAd()
    {
        counter--;

        if (counter == 0)
        {
            ShowAd();
            counter = 3;
        }
    }

    public void ShowAd()
    {
        #if UNITY_IOS || UNITY_ANDROID
        if (Advertisement.IsReady())
        {
            if (string.IsNullOrEmpty(zoneID)) zoneID = null;

            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;

            Advertisement.Show(zoneID, options);
        }
#endif
    }

#if UNITY_IOS || UNITY_ANDROID
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                StartCoroutine(AnimationManager.Instance.Continue(true));
                break;
            case ShowResult.Skipped:
            case ShowResult.Failed:
                StartCoroutine(AnimationManager.Instance.Continue(false));
                break;
        }
    }
#endif
}