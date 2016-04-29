using UnityEngine;
using UnityEngine.Advertisements;

public class Advertising : MonoBehaviour
{

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
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
#endif
    }
}