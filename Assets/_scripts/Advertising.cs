using UnityEngine;
using UnityEngine.Advertisements;

public class Advertising : MonoBehaviour
{
    public string zoneID;
    private int counter;

#if UNITY_ANDROID || UNITY_IOS
    void Awake()
    {
        Manager.Instance.adverts = this;
        counter = 3;
    }
#endif

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
#if UNITY_ANDROID || UNITY_IOS
        if (Advertisement.IsReady())
        {
            if (string.IsNullOrEmpty(zoneID)) zoneID = null;

            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;

            Advertisement.Show(zoneID, options);
        }
#endif
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Manager.Instance.game.Play();
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped.");
                break;
            case ShowResult.Failed:
                Debug.LogError("Video failed to show.");
                break;
        }
    }
}