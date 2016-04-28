using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Preferences : Singleton<Preferences> {

    public static int highScore;
    public static int mainMode;
    public static int subMode;

	public void Load(){
        highScore = PlayerPrefs.GetInt("highScore", 0);
        mainMode = PlayerPrefs.GetInt("mainMode", 0);
        subMode = PlayerPrefs.GetInt("subMode", 0);
	}

    public void Reset() {
        PlayerPrefs.DeleteAll();
        Load();
    }

    public void UpdateHighScore(int score) {
            highScore = score;
            PlayerPrefs.Save();
    }

    public void Save(){
        PlayerPrefs.SetInt("highScore", highScore);
        PlayerPrefs.SetInt("mainMode", mainMode);
        PlayerPrefs.SetInt("subMode", subMode);

        PlayerPrefs.Save ();
	}

	void OnDisable(){
		Save ();
	}
}
