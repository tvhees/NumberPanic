using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Controller
{
    public class Advertising : MonoBehaviour
    {
        public void ShowAd()
        {
#if UNITY_IOS || UNITY_ANDROID
            if (!Advertisement.IsReady()) return;
            var options = new ShowOptions {resultCallback = HandleShowResult};
            Advertisement.Show(options);
#endif
        }

#if UNITY_IOS || UNITY_ANDROID
        private void HandleShowResult(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                case ShowResult.Skipped:
                    StartCoroutine(AnimationManager.Instance.ContinueButtonPressed(true));
                    break;
                case ShowResult.Failed:
                    StartCoroutine(AnimationManager.Instance.ContinueButtonPressed(false));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("result", result, null);
            }
        }
#endif

        private void Awake()
        {
            Advertisement.Initialize(Advertisement.gameId);
            Manager.Instance.adverts = this;
        }
    }
}