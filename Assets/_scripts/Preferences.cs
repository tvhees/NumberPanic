using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Preferences : Singleton<Preferences> {

    public static int highScore;
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

    public int GetHighScore() {
        string scoreName = ((Manager.Mode)mainMode).ToString() + "_" + subMode.ToString();
        highScore = PlayerPrefs.GetInt(scoreName, 0);
        return highScore;
    }

    public void UpdateHighScore(int score) {
        highScore = score;
        PlayerPrefs.Save();
    }

    public void Save(){
        string scoreName = ((Manager.Mode)mainMode).ToString() + "_" + subMode.ToString();
        PlayerPrefs.SetInt(scoreName, highScore);
        PlayerPrefs.SetInt("mainMode", mainMode);
        PlayerPrefs.SetInt("subMode", subMode);

        PlayerPrefs.Save ();
	}

	void OnDisable(){
		Save ();
	}
}
