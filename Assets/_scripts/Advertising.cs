using UnityEngine;
using UnityEngine.Advertisements;

public class Advertising : MonoBehaviour
{
    private int counter;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
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
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
#endif
    }
}