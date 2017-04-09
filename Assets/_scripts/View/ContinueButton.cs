using Controller;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ContinueButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        private void OnEnable()
        {
            button.interactable = true;
        }
    
        public void ContinueGame()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Preferences.ShowAdvertisements)
            {
                MainManager.Instance.adverts.ShowAd();
                return;
            }
#endif
            StartCoroutine(AnimationManager.Instance.ContinueButtonPressed(true));
        }
    }
}
