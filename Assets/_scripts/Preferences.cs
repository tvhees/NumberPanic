using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Preferences : Singleton<Preferences> {

    public static FaceValue highScore = new FaceValue();
    public static int mainMode;
    public static int subMode;

	public void Load(){
        mainMode = PlayerPrefs.GetInt("mainMode", 0);
        subMode = PlayerPrefs.GetInt("subMode", 0);
        GetHighScore();
    }

    public void Reset() {
        PlayerPrefs.DeleteAll();
        Load();
    }

    public FaceValue GetHighScore() {
        string scoreName = ((Manager.Mode)mainMode).ToString() + "_" + subMode.ToString();
        highScore.value = PlayerPrefs.GetInt(scoreName + "_value", 0);
        highScore.text = PlayerPrefs.GetString(scoreName + "_text", null);
        Debug.Log(scoreName + " " + highScore.value + " GET");
        return highScore;
    }

    private void SaveHighScore()
    {
        string scoreName = ((Manager.Mode)mainMode).ToString() + "_" + subMode.ToString();
        Debug.Log(scoreName + " " + highScore.value + " SAVE");

        PlayerPrefs.SetInt(scoreName + "_value", highScore.value);
        PlayerPrefs.SetString(scoreName + "_text", highScore.text);
        PlayerPrefs.Save();
    }

    public void UpdateHighScore(FaceValue fV) {
        Debug.Log("UpdatingHighScore");
        highScore = fV;
        SaveHighScore();
    }

    public void Save(){
        Debug.Log("Saving regular preferences");

        PlayerPrefs.SetInt("mainMode", mainMode);
        PlayerPrefs.SetInt("subMode", subMode);
        PlayerPrefs.Save ();
	}

	void OnDisable(){
		Save ();
	}
}
