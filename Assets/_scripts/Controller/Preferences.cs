﻿using UnityEngine;

namespace Assets._scripts.Controller
{
    public class Preferences : Singleton<Preferences> {

        public static FaceValue HighScore = new FaceValue();
        public static int MainMode;
        public static int SubMode;
        public static bool ShowAdvertisements;
        public static bool ShowTutorial = true;

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
            return HighScore;
        }

        private static void SaveHighScore()
        {
            var scoreName = ((Manager.Mode)MainMode).ToString() + "_" + SubMode.ToString();
            PlayerPrefs.SetInt(scoreName + "_value", HighScore.Value);
            PlayerPrefs.SetString(scoreName + "_text", HighScore.Text);
        }

        public void UpdateHighScore(FaceValue fV) {
            HighScore = fV;
        }

        public void Save(){
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
