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
        mainMode = ZPlayerPrefs.GetInt("mainMode", 0);
        subMode = ZPlayerPrefs.GetInt("subMode", 0);
        advertisements = ExtensionMethods.GetBool("advertisements", true);
        GetHighScore();
    }

    public FaceValue GetHighScore() {
        string scoreName = ((Manager.Mode)mainMode).ToString() + "_" + subMode.ToString();
        highScore.value = ZPlayerPrefs.GetInt(scoreName + "_value", 0);
        highScore.text = ZPlayerPrefs.GetString(scoreName + "_text", null);
        Debug.Log(scoreName + " " + highScore.value + " GET");
        return highScore;
    }

    private void SaveHighScore()
    {
        string scoreName = ((Manager.Mode)mainMode).ToString() + "_" + subMode.ToString();
        Debug.Log(scoreName + " " + highScore.value + " SAVE");

        ZPlayerPrefs.SetInt(scoreName + "_value", highScore.value);
        ZPlayerPrefs.SetString(scoreName + "_text", highScore.text);
        ZPlayerPrefs.Save();
    }

    public void UpdateHighScore(FaceValue fV) {
        Debug.Log("UpdatingHighScore");
        highScore = fV;
        SaveHighScore();
    }

    public void Save(){
        Debug.Log("Saving regular preferences");

        ZPlayerPrefs.SetInt("mainMode", mainMode);
        ZPlayerPrefs.SetInt("subMode", subMode);
        ExtensionMethods.SetBool("advertisements", advertisements);
        ZPlayerPrefs.Save ();
	}

    public void BuyNoAds()
    {
        advertisements = false;
        Save();
    }

	void OnDisable(){
		Save ();
	}
}
