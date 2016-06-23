using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Preferences : Singleton<Preferences> {

    public static FaceValue highScore = new FaceValue();
    public static int mainMode;
    public static int subMode;
    public static bool advertisements;

    void Awake()
    {
        string salt = PlayerPrefs.GetString("salt");
        if (string.IsNullOrEmpty(salt))
        {
            salt = ExtensionMethods.Md5Sum(Random.Range(0, 100000000).ToString());
            PlayerPrefs.SetString("salt", salt);
        }

        if (Manager.Instance.debugMode)
            Debug.Log(salt);

        ZPlayerPrefs.Initialize("NK6KzW8Tz9rpANca", salt);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Reset();
    }

    void Reset()
    {
        ZPlayerPrefs.DeleteAll();
        Load();
    }
#endif

    public void Load(){
        mainMode = PlayerPrefs.GetInt("mainMode", 0);
        subMode = PlayerPrefs.GetInt("subMode", 0);
        advertisements = ExtensionMethods.GetBool("advertisements", true);
        GetHighScore();
    }

    public FaceValue GetHighScore() {
        string scoreName = ((Manager.Mode)mainMode).ToString() + "_" + subMode.ToString();
        highScore.value = ZPlayerPrefs.GetInt(scoreName + "_value", 0);
        highScore.text = ZPlayerPrefs.GetString(scoreName + "_text", "");

        if (Manager.Instance.debugMode)
            Debug.Log(scoreName + " " + highScore.value + " GET");

        return highScore;
    }

    private void SaveHighScore()
    {
        string scoreName = ((Manager.Mode)mainMode).ToString() + "_" + subMode.ToString();
        if(Manager.Instance.debugMode)
            Debug.Log(scoreName + " " + highScore.value + " SAVE");

        ZPlayerPrefs.SetInt(scoreName + "_value", highScore.value);
        ZPlayerPrefs.SetString(scoreName + "_text", highScore.text);
    }

    public void UpdateHighScore(FaceValue fV) {
        if (Manager.Instance.debugMode)
            Debug.Log("UpdatingHighScore");

        highScore = fV;
    }

    public void Save(){
        // Save mode choices without encryption
        PlayerPrefs.SetInt("mainMode", mainMode);
        PlayerPrefs.SetInt("subMode", subMode);

        // Save Noads purchase flag with encryption
        ExtensionMethods.SetBool("advertisements", advertisements);

        // Save high score with encryption
        SaveHighScore();

        // Save all preferences
        ZPlayerPrefs.Save ();
	}

    public void BuyNoAds()
    {
        advertisements = false;
        Save();
    }
}
