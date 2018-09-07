using System;
using Managers;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Controller
{
    [ManagerDependency(typeof(ManagerContainer))]
    public class Advertising : BaseMonoBehaviour
    {
        protected override void Awake()
        {
            // base.Awake();
            // Advertisement.Initialize(Advertisement.gameId);
        }

        public void ShowAd()
        {
// #if UNITY_IOS || UNITY_ANDROID
//             if (!Advertisement.IsReady()) return;
//             var options = new ShowOptions {resultCallback = HandleShowResult};
//             Advertisement.Show(options);
// #endif
        }

// #if UNITY_IOS || UNITY_ANDROID
//         private void HandleShowResult(ShowResult result)
//         {
//             switch (result)
//             {
//                 case ShowResult.Finished:
//                 case ShowResult.Skipped:
//                     GetManager<StateManager>().MoveTo(States.Play);
//                     break;
//                 case ShowResult.Failed:
//                     GetManager<StateManager>().MoveTo(States.Score);
//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException("result", result, null);
//             }
//         }
// #endif
    }
}