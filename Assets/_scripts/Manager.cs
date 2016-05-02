using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Manager : Singleton<Manager> {

    
    [HideInInspector] public UIManager ui;
    [HideInInspector] public Game game;
    [HideInInspector] public Spawner spawner;
    [HideInInspector] public Advertising adverts;
    [HideInInspector] public MainMode modes;
    [HideInInspector] public Text highScore, title;
    [HideInInspector] public Animator titleAnimator;

    // Variables used to control pace of the game - set in inspector while testing
    [SerializeField] private float setPadding = 20f;
    [SerializeField] private float setWait = 1f;
    [SerializeField] private float setSpeed = 3f;
    public static float padding;
    public static float waitFactor;
    public static float speedFactor;

    // Data holders
    public Data data = new Data();

    // Game state variables
    public static Mode mode;
    public static int current;

    // Enums
    // Main modes
    public enum Mode
    {
        linear,
        power,
        sequence,
        NumberOfTypes
    }

    // Sub modes
    public static int subValue;
    public enum Sequence
    {
        primes,
        fibbonaci,
        NumberOfTypes
    }

    void Awake() {
        padding = setPadding;
        waitFactor = setWait;
        speedFactor = setSpeed;

        data.AssignArrays();
        StartCoroutine(LoadGame());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Restart();
            //Application.Quit();

        if (game != null)
            game.RunTimers();
    }

    public void Restart() {
        DestroyGame();
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame() {
        current = 0;

        game = ScriptableObject.CreateInstance("Game") as Game;

        if (!SceneManager.GetSceneByName("Game").isLoaded)
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);

        yield return new WaitForSeconds(0.01f);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
    }

    void DestroyGame() {
        Preferences.Instance.Save();
        Time.timeScale = 1f;
        SceneManager.UnloadScene("Game");
        Destroy(game);

        // Show an ad at regular intervals
		if(adverts != null)
	        adverts.RegularAd();
    }
}
