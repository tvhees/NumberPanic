using UnityEngine;
using UnityEngine.UI;
using _scripts.Controller;

namespace _scripts.View
{
    public class ContinueButton : MonoBehaviour {

        public Button button;

        void OnEnable()
        {
            button.interactable = true;
        }
    
        public void ContinueGame() {
#if UNITY_ANDROID || UNITY_IOS
            if(Preferences.ShowAdvertisements)
                Manager.Instance.adverts.ShowAd();
            else
                StartCoroutine(AnimationManager.Instance.Continue(true));

#endif

#if !UNITY_ANDROID && !UNITY_IOS
        StartCoroutine(AnimationManager.Instance.Continue(true));
#endif
        }
    }
}
