﻿using GooglePlayGames;
using Model;
using RSG;
using UnityEngine;
using Utility;

namespace Controller
{
    public class SocialManager : Singleton<SocialManager>
    {

        private readonly PromiseTimer over9000Timer = new PromiseTimer();
        private readonly PromiseTimer ausAnthemTimer = new PromiseTimer();

        public static bool[] TimesTables;
        public static bool[] Sampler;

        public void Awake()
        {
            Time.timeScale = 0.0f;
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((success) =>
            {
                Time.timeScale = 1.0f;
                Debug.Log("Authenticated: " + success);
            });

            WaitForOver9000();
            WaitForAusAnthem();
        }

        public static void LoadAchievementArrays()
        {
            TimesTables = ExtensionMethods.GetBoolArray("TimesTables", false, 10);
            Sampler = ExtensionMethods.GetBoolArray("Generalist", false, (int)Manager.Mode.NumberOfTypes);
            if (Sampler.Length < (int) Manager.Mode.NumberOfTypes)
                Sampler = GetNewSamplerArray();
        }

        private static bool[] GetNewSamplerArray()
        {
            var newArray = new bool[(int) Manager.Mode.NumberOfTypes];
            Sampler.CopyTo(newArray, 0);
            return newArray;
        }

        public static void SaveAchievementArrays()
        {
            ExtensionMethods.SetBoolArray("TimesTables", TimesTables);
            ExtensionMethods.SetBoolArray("Generalist", Sampler);
        }

        private void WaitForOver9000()
        {
            over9000Timer.WaitUntil(_ => Manager.Instance.game != null &&
                                         Manager.Instance.game.GetFaceValue().Value > 9000)
                .Done(() => Social.ReportProgress(GPGSIds.achievement_its_over_9000, 100.0f, null));
        }

        private void WaitForAusAnthem()
        {
            ausAnthemTimer.WaitUntil(_ => Manager.Instance.game != null &&
                                          Manager.Instance.game.mode == Manager.Mode.English &&
                                          Manager.Instance.game.subMode == (int) Manager.English.AusAnthem &&
                                          Manager.Instance.game.GetFaceValue().Value >= Manager.Instance.data.Texts.AusAnthem.Length)
                .Done(() => Social.ReportProgress(GPGSIds.achievement_now_you_know_the_second_verse, 100.0f, null));
        }

        private void Update()
        {
            over9000Timer.Update(Time.deltaTime);
            ausAnthemTimer.Update(Time.deltaTime);
        }

        public void NewGamePlayed(Manager.Mode mainMode)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_dedicated, 1, null);
            UpdateSamplerArray((int)mainMode);
        }

        public void UpdateSamplerArray(int i)
        {
            if (Sampler[i]) return;
            Sampler[i] = true;
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_sampler, 1, null);
        }

        public static void UpdateTimesTablesArray(Manager.Mode mainMode, int subMode)
        {
            if (mainMode != Manager.Mode.Linear)
                return;

            if (TimesTables[subMode]) return;

            TimesTables[subMode] = true;
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_times_tables, 1, null);
        }
    }
}