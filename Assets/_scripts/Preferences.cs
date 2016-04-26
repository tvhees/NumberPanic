using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Preferences : Singleton<Preferences> {

    public int highScore;

	public void Load(){
        highScore = PlayerPrefs.GetInt("highScore", 0);
        UpdateHighScore(highScore);
	}

    public void Reset() {
        PlayerPrefs.DeleteAll();
        Load();
    }

    public void UpdateHighScore(int score) {
        if (score >= highScore)
        {
            highScore = score;
            Manager.Instance.highScore.text = highScore.ToString();
            PlayerPrefs.Save();
        }
    }

    public void Save(){
        PlayerPrefs.SetInt("highScore", highScore);
		PlayerPrefs.Save ();
	}

	void OnDisable(){
		Save ();
	}
}
