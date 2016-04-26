using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Manager : Singleton<Manager> {

    public Game game;
    public Text highScore, title;
    public Animator titleAnimator;
    public float padding;

    void Awake() {
        Preferences.Instance.Load();
        titleAnimator = title.GetComponent<Animator>();
        StartCoroutine(LoadGame());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void Restart() {
        StartCoroutine(DestroyGame());
    }

    IEnumerator LoadGame() {
        game = ScriptableObject.CreateInstance("Game") as Game;

        if (!SceneManager.GetSceneByName("Game").isLoaded)
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.01f);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
    }

    IEnumerator DestroyGame() {
        yield return new WaitForSeconds(1.2f);
        Time.timeScale = 1f;
        SceneManager.UnloadScene("Game");
        titleAnimator.SetTrigger("fade");
        Destroy(game);
        StartCoroutine(LoadGame());
    }
}
