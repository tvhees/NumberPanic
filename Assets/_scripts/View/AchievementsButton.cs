using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace View
{
    public class AchievementsButton : MonoBehaviour
    {
        public void ShowAchievements()
        {
            Social.ShowAchievementsUI();
        }
    }
}