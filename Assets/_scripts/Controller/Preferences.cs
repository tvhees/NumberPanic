using Model;
using thirdparty.SecurePlayerPrefs;
using UnityEngine;
using Utility;
using View;

namespace Controller
{
    public class Preferences : Singleton<Preferences>
    {

        public static FaceValue HighScore = FaceValue.Zero();
        public static int MainMode;
        public static int SubMode;
        public static bool ShowAdvertisements;
        public static bool ShowTutorial;
        public static bool ShakeCamera;

        private void Awake()
        {
            var salt = PlayerPrefs.GetString("salt");
            if (string.IsNullOrEmpty(salt))
            {
                salt = ExtensionMethods.Md5Sum(Random.Range(0, 100000000).ToString());
                PlayerPrefs.SetString("salt", salt);
            }

            ZPlayerPrefs.Initialize("NK6KzW8Tz9rpANca", salt);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                Reset();
        }

        private void Reset()
        {
            ZPlayerPrefs.DeleteAll();
            Load();
        }
#endif

        public void Load()
        {
            // SocialManager.LoadAchievementArrays();
            MainMode = PlayerPrefs.GetInt("mainMode", 0);
            SubMode = PlayerPrefs.GetInt("subMode", 0);
            ShowAdvertisements = ExtensionMethods.GetBool("ShowAdvertisements", true);
            ShowTutorial = ExtensionMethods.GetBool("ShowTutorial", true);
            ShakeCamera = ExtensionMethods.GetBool("ShakeCamera", true);
            UiManager.Instance.SetTutorialToggle(ShowTutorial);
            UiManager.Instance.SetShakeToggle(ShakeCamera);
            GetHighScore();
        }

        public FaceValue GetHighScore() {
            var scoreName = ((MainManager.Mode)MainMode).ToString() + "_" + SubMode.ToString();
            HighScore = new FaceValue(
                PlayerPrefs.GetInt(scoreName + "_value", 0),
                PlayerPrefs.GetString(scoreName + "_text", "")
            );
            return HighScore;
        }

        private static void SaveHighScore()
        {
            var scoreName = ((MainManager.Mode)MainMode).ToString() + "_" + SubMode.ToString();
            PlayerPrefs.SetInt(scoreName + "_value", HighScore.Value);
            PlayerPrefs.SetString(scoreName + "_text", HighScore.Text);
        }

        public void UpdateHighScore(FaceValue fV) {
            HighScore = fV;
        }

        public void ToggleTutorial(bool isOn)
        {
            ShowTutorial = isOn;
            UiManager.Instance.SetTutorialToggle(isOn);
            Save();
            if(isOn)
                Tutorial.Instance.RunMenuTutorial();
        }

        public void ToggleShake(bool isOn)
        {
            ShakeCamera = isOn;
            Save();
        }

        public void Save(){
            // Save mode choices without encryption
            PlayerPrefs.SetInt("mainMode", MainMode);
            PlayerPrefs.SetInt("subMode", SubMode);

            // Save high score without encryption
            SaveHighScore();

            // Save Noads purchase flag with encryption and other toggles
            ExtensionMethods.SetBool("ShowAdvertisements", ShowAdvertisements);
            ExtensionMethods.SetBool("ShowTutorial", ShowTutorial);
            ExtensionMethods.SetBool("ShakeCamera", ShakeCamera);

            // Save achievement arrays without encryption
            // SocialManager.SaveAchievementArrays();

            // Save all preferences
            ZPlayerPrefs.Save ();
        }

        public void BuyNoAds()
        {
            ShowAdvertisements = false;
            Save();
        }
    }
}
