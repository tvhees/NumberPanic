using Controller;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ContinueButton : BaseMonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Advertising advertising;

        private void OnEnable()
        {
            button.interactable = true;
        }
    
        public void ContinueGame()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Preferences.ShowAdvertisements)
            {
                advertising.ShowAd();
                return;
            }
#endif
            GetManager<StateManager>().MoveTo(States.Play);
        }
    }
}
