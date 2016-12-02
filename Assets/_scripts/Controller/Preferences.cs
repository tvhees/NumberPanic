using UnityEngine;

namespace _scripts.Controller
{
    public class Preferences : Singleton<Preferences> {

        public static FaceValue HighScore = new FaceValue();
        public static int MainMode;
        public static int SubMode;
        public static bool ShowAdvertisements;

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

        public void Load(){
            MainMode = PlayerPrefs.GetInt("mainMode", 0);
            SubMode = PlayerPrefs.GetInt("subMode", 0);
            ShowAdvertisements = ExtensionMethods.GetBool("ShowAdvertisements", true);
            GetHighScore();
        }

        public FaceValue GetHighScore() {
            var scoreName = ((Manager.Mode)MainMode).ToString() + "_" + SubMode.ToString();
            HighScore.Value = PlayerPrefs.GetInt(scoreName + "_value", 0);
            HighScore.Text = PlayerPrefs.GetString(scoreName + "_text", "");
            Debug.Log("Getting high score " + scoreName + " " + HighScore.Value);

            return HighScore;
        }

        private static void SaveHighScore()
        {
            var scoreName = ((Manager.Mode)MainMode).ToString() + "_" + SubMode.ToString();
            Debug.Log("Setting high score " + scoreName + " " + HighScore.Value);
            PlayerPrefs.SetInt(scoreName + "_value", HighScore.Value);
            PlayerPrefs.SetString(scoreName + "_text", HighScore.Text);
        }

        public void UpdateHighScore(FaceValue fV) {
            Debug.Log("Updating high score");
            HighScore = fV;
        }

        public void Save(){
            Debug.Log("Saving preferences and score");
            // Save mode choices without encryption
            PlayerPrefs.SetInt("mainMode", MainMode);
            PlayerPrefs.SetInt("subMode", SubMode);

            // Save high score without encryption
            SaveHighScore();

            // Save Noads purchase flag with encryption
            ExtensionMethods.SetBool("ShowAdvertisements", ShowAdvertisements);

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
