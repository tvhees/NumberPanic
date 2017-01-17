using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class NoAdsButton : MonoBehaviour {

        public Button button;

        void Update()
        {
            button.interactable = Preferences.ShowAdvertisements;
        }

    }
}
