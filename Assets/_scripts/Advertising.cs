using UnityEngine;
using UnityEngine.Advertisements;

public class Advertising : MonoBehaviour
{
    private int counter;

    void Awake()
    {
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
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }
}