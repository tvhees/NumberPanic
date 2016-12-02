using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class NoAdsButton : MonoBehaviour {

        public Button button;

        void Update()
        {
            button.interactable = Preferences.ShowAdvertisements;
        }

    }
}
